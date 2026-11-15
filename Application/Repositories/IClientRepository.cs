using Application.Clients.DTOs;
using Application.DTOs;
using Domain.Entities;

namespace Application.Repositories
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        public Task<Client> GetByIdWithEmployee(int id);
        public Task<PaginationOutput<Client>> GetClientPagination(ClientFilterInput input);
    }
}
