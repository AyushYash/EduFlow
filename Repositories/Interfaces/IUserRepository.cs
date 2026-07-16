using EduFlow.Models;

namespace EduFlow.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string Email);
    Task<User> AddAsync(User user);
    Task<bool> EmailExistsAsync(string email);
}