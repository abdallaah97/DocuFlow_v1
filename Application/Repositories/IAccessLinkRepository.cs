using Domain.Entities;

namespace Application.Repositories
{
    public interface IAccessLinkRepository : IGenericRepository<AccessLink>
    {
        // Ignores the tenant filter: used by anonymous clients (no tenant in the token)
        public Task<AccessLink> GetByTokenWithItems(string token);
    }
}
