using EduFlow.Models;
using EduFlow.DTOs;
using EduFlow.Repositories.Interfaces;
using EduFlow.Services.Interfaces;

namespace EduFlow.Services;

public class TenantService : ITenantService
{
    private readonly ITenantRepository _repository;

    public TenantService(ITenantRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Tenant>> GetAllTenantsAsync()
    {
        return await _repository.GetAllActiveAsync();
    }

    public async Task<Tenant?> GetTenantByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Tenant> CreateTenantAsync(CreateTenantDto dto)
    {
        var normalizedSubdomain = dto.Subdomain.Trim().ToLowerInvariant();

        var exists = await _repository.SubdomainExistsAsync(normalizedSubdomain);
        if (exists)
        {
            throw new InvalidOperationException($"A tenant with the subdomain '{normalizedSubdomain}' already exists.");
        }

        var tenant = new Tenant
        {
            Name = dto.Name.Trim(),
            Subdomain = normalizedSubdomain,
            IsActive = true
        };

        return await _repository.AddAsync(tenant);
    }
}