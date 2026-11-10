using Domain.Entities;

namespace Application.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public Task<User> GetByEmailWithRoleAsync(string email);
        public Task<User> GetByIdWithRole(int id);
    }
}
