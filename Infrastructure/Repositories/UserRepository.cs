using Application.DTOs;
using Application.Repositories;
using Application.Users.DTOs;
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

        public async Task<PaginationOutput<User>> GetUserPagination(FilterInput input)
        {
            var query = _dbSet
                .Include(u => u.Role).AsQueryable();

            if (!string.IsNullOrEmpty(input.Search))
            {
                query = query.Where
                    (u =>
                        u.Name.ToLower().Trim().Contains(input.Search.ToLower().Trim()) ||
                        u.Email.ToLower().Trim().Contains(input.Search.ToLower().Trim()) ||
                        u.PhoneNumber.ToLower().Trim().Contains(input.Search.ToLower().Trim())
                    );
            }

            if (input.RoleId.HasValue)
            {
                query = query.Where(u => u.RoleId == input.RoleId);
            }

            var totalCount = await query.CountAsync();

            query = query.OrderByDescending(u => u.CreatedAt);

            query = query.Skip(input.PageIndex * input.PageSize).Take(input.PageSize);

            var users = await query.ToListAsync();

            return new PaginationOutput<User>
            {
                Items = users,
                TotalCount = totalCount
            };
        }
    }
}
