using Application.Auth;
using Application.Repositories;
using Application.Users.DTOs;
using Domain.Entities;

namespace Application.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IPasswordHasher _passwordHasher;
        public UserService(IUserRepository userRepository, ICurrentUser currentUser, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
            _passwordHasher = passwordHasher;
        }

        public async Task ChangePassword(ChangePasswordInput input)
        {
            var user = await _userRepository.GetById(_currentUser.UserId);

            if (!_passwordHasher.Verify(input.OldPassword, user.Password))
            {
                throw new Exception("Incorrect old password");
            }

            if (input.NewPassword != input.ConfirmNewPassword)
            {
                throw new Exception("Confirm new password incorrect");
            }

            user.Password = _passwordHasher.Hash(input.NewPassword);
            _userRepository.Update(user);
            await _userRepository.SaveChanges();
        }

        public async Task CreateUser(CreateUserInput input)
        {
            var user = new User
            {
                Name = input.Name,
                Email = input.Email,
                PhoneNumber = input.PhoneNumber,
                RoleId = input.RoleId,
                CreatedAt = DateTime.UtcNow
            };
            user.Password = _passwordHasher.Hash(input.Password);

            await _userRepository.Insert(user);
            await _userRepository.SaveChanges();
        }
    }
}
