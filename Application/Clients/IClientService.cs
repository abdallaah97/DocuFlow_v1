using Application.Clients.DTOs;
using Application.DTOs;

namespace Application.Clients
{
    public interface IClientService
    {
        public Task CreateClient(SaveClientInput input);
        public Task UpdateClient(int id, SaveClientInput input);
        public Task<PaginationOutput<GetClientOutput>> GetClients(ClientFilterInput input);
        public Task<PaginationOutput<GetClientOutput>> GetMyClients(ClientFilterInput input);
        public Task<GetClientOutput> GetClientById(int id);
        public Task DeleteClient(int id);
        public Task AssignEmployee(int id, AssignEmployeeInput input);
    }
}
