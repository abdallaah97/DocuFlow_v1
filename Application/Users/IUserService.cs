using Application.Users.DTOs;

namespace Application.Users
{
    public interface IUserService
    {
        public Task ChangePassword(ChangePasswordInput input);
        public Task CreateUser(CreateUserInput input);
    }
}
