using Application.Repositories;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<User> GetByEmailWithRoleAsync(string email)
        {
            return await _dbSet.IgnoreQueryFilters().Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
        }

        public async Task<User> GetByIdWithRole(int id)
        {
            return await _dbSet.IgnoreQueryFilters().
                Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        }
    }
}
