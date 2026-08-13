using EduFlow.Models;

namespace EduFlow.Services.Interfaces;

public interface IUserService
{
    Task<List<User>> GetUsersForTenantAsync(Guid tenantId);
}