using Application.DTOs;
using Application.Users.DTOs;
using Domain.Entities;

namespace Application.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public Task<User> GetByEmailWithRoleAsync(string email);
        public Task<User> GetByIdWithRole(int id);
        public Task<PaginationOutput<User>> GetUserPagination(FilterInput input);
    }
}
