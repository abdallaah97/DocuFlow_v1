using Application.Clients.DTOs;
using Application.DTOs;
using Application.Repositories;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        public ClientRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Client> GetByIdWithEmployee(int id)
        {
            return await _dbSet.Include(c => c.AssignedEmployee)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<PaginationOutput<Client>> GetClientPagination(ClientFilterInput input)
        {
            var query = _dbSet
                .Include(c => c.AssignedEmployee).AsQueryable();

            if (!string.IsNullOrEmpty(input.Search))
            {
                var search = input.Search.ToLower().Trim();
                query = query.Where
                    (c =>
                        c.Name.ToLower().Contains(search) ||
                        c.Email.ToLower().Contains(search) ||
                        c.PhoneNumber.ToLower().Contains(search)
                    );
            }

            if (input.AssignedEmployeeId.HasValue)
            {
                query = query.Where(c => c.AssignedEmployeeId == input.AssignedEmployeeId);
            }

            var totalCount = await query.CountAsync();

            query = query.OrderByDescending(c => c.CreatedAt);

            query = query.Skip(input.PageIndex * input.PageSize).Take(input.PageSize);

            var clients = await query.ToListAsync();

            return new PaginationOutput<Client>
            {
                Items = clients,
                TotalCount = totalCount
            };
        }
    }
}
