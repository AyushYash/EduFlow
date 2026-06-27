using EduFlow.Models;
using EduFlow.DTOs;

namespace EduFlow.Services.Interfaces;

public interface ITenantService
{
    Task<List<Tenant>> GetAllTenantsAsync();
    Task<Tenant?> GetTenantByIdAsync(Guid id);
    Task<Tenant> CreateTenantAsync(CreateTenantDto dto);
}