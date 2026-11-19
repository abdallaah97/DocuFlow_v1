using Application.Clients.DTOs;
using Application.DTOs;
using Application.Repositories;
using Application.Requests.DTOs;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RequestRepository : GenericRepository<DocumentRequest>, IRequestRepository
    {
        public RequestRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<DocumentRequest> GetByIdWithClient(int id, bool ignoreQueryFilters = false)
        {
            if (ignoreQueryFilters)
            {
                return await _dbSet.IgnoreQueryFilters()
                    .Include(r => r.Client)
                    .Include(r => r.DocumentRequestItems)
                    .FirstOrDefaultAsync(r => r.Id == id);
            }

            return await _dbSet.Include(r => r.Client)
                .Include(r => r.DocumentRequestItems)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<PaginationOutput<DocumentRequest>> GetDocumentRequestPagination(RequestFilterInput input, int? employeeId)
        {
            var query = _dbSet
                .Include(c => c.Client).AsQueryable();

            if (employeeId.HasValue)
            {
                query = query.Where(x => x.Client.AssignedEmployeeId == employeeId);
            }

            if (input.Status.HasValue)
            {
                query = query.Where(x => x.Status == input.Status);
            }

            if (!string.IsNullOrEmpty(input.Search))
            {
                var search = input.Search.ToLower().Trim();
                query = query.Where
                    (c =>
                        c.Title.ToLower().Contains(search) ||
                        c.Description.ToLower().Contains(search) ||
                        c.Client.Email.ToLower().Contains(search)
                    );
            }

            var totalCount = await query.CountAsync();

            query = query.OrderByDescending(c => c.CreatedAt);

            query = query.Skip(input.PageIndex * input.PageSize).Take(input.PageSize);

            var documentRequests = await query.ToListAsync();

            return new PaginationOutput<DocumentRequest>
            {
                Items = documentRequests,
                TotalCount = totalCount
            };
        }
    }
}
