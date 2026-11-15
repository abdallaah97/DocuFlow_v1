using Application.Repositories;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AccessLinkRepository : GenericRepository<AccessLink>, IAccessLinkRepository
    {
        public AccessLinkRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<AccessLink> GetByTokenWithItems(string token)
        {
            // IgnoreQueryFilters also disables soft delete, so IsDeleted is checked manually
            // (items are filtered by IsDeleted in the service)
            return await _dbSet.IgnoreQueryFilters()
                .Include(a => a.DocumentRequest)
                    .ThenInclude(r => r.DocumentRequestItems)
                .FirstOrDefaultAsync(a => a.Token == token && !a.IsDeleted && !a.DocumentRequest.IsDeleted);
        }
    }
}
