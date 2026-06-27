using Microsoft.EntityFrameworkCore;
using EduFlow.Data;
using EduFlow.Models;
using EduFlow.Repositories.Interfaces;

namespace EduFlow.Repositories;

public class TenantRepository : ITenantRepository
{
    private readonly AppDbContext _context;

    public TenantRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Tenant>> GetAllActiveAsync()
    {
        return await _context.Tenants
            .Where(t =>t.IsActive)
            .ToListAsync();
    }

    public async Task<Tenant?> GetByIdAsync(Guid id)
    {
        return await _context.Tenants.FindAsync(id);
    }

    public async Task<Tenant> AddAsync(Tenant tenant)
    {
        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync();
        return tenant;
    }

    public async Task<bool> SubdomainExistsAsync(string subdomain)
    {
        return await _context.Tenants
            .AnyAsync(t => t.Subdomain == subdomain);
    }
}