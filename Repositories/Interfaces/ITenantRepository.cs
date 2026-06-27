using EduFlow.Models;

namespace EduFlow.Repositories.Interfaces;

public interface ITenantRepository
{
    Task<List<Tenant>> GetAllActiveAsync();
    Task<Tenant?> GetByIdAsync(Guid id);
    Task<Tenant> AddAsync(Tenant tenant);
    Task<bool> SubdomainExistsAsync(string subdomain);
}