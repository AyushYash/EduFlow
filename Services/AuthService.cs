using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using EduFlow.DTOs;
using EduFlow.Models;
using EduFlow.Repositories.Interfaces;
using EduFlow.Services.Interfaces;

namespace EduFlow.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _config;
    private AuthService(IUserRepository userRepository, IConfiguration config)
    {
        _userRepository = userRepository;
        _config = config;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();

        //no duplicate emails
        if (await _userRepository.EmailExistsAsync(email))
        {
            throw new InvalidOperationException("A user with this email already exists.");
        }

        //hash password
        var PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User
        {
            Email = email,
            PasswordHash = PasswordHash,
            FullName = dto.FullName.Trim(),
            Role = "Student",
            TenantId = dto.TenantId
        };

        await _userRepository.AddAsync(user);

        //Issuing a token so they're logged in immediately
        var token = GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role
        };

    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();

        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            return null;
        }

        var passwordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!passwordValid)
        {
            return null;
        }

        var token = GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role
        };
    }

    private string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("TenantId", user.TenantId.ToString())
        };

        //Build the signing key from our secret
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentails = new  SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //assemble the token
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                double.Parse(_config["Jwt:ExpireMinutes"]!)),
            signingCredentials: credentails
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}