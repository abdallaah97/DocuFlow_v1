using Application.DTOs;
using Application.Users.DTOs;

namespace Application.Users
{
    public interface IUserService
    {
        public Task ChangePassword(ChangePasswordInput input);
        public Task CreateUser(SaveUserInput input);
        public Task UpdateUser(int id, SaveUserInput input);
        public Task<PaginationOutput<GetUserOutput>> GetUsers(FilterInput input);
        public Task DeleteUser(int id);
    }
}
