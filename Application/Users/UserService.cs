using Application.Auth;
using Application.DTOs;
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

        public async Task CreateUser(SaveUserInput input)
        {
            var isEmailExists = await _userRepository.Any(u => u.Email.ToLower().Trim() == input.Email.ToLower().Trim());
            if (isEmailExists)
            {
                throw new Exception("Email already exists");
            }
            var isPhoneNumberExists = await _userRepository.Any(u => u.PhoneNumber.ToLower().Trim() == input.PhoneNumber.ToLower().Trim());
            if (isPhoneNumberExists)
            {
                throw new Exception("Phone number already exists");
            }

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

        public async Task UpdateUser(int id, SaveUserInput input)
        {
            var user = await _userRepository.GetById(id);

            var isEmailExists = await _userRepository
                .Any(u => u.Email.ToLower().Trim() == input.Email.ToLower().Trim() && u.Id != id);
            if (isEmailExists)
            {
                throw new Exception("Email already exists");
            }
            var isPhoneNumberExists = await _userRepository
                .Any(u => u.PhoneNumber.ToLower().Trim() == input.PhoneNumber.ToLower().Trim() && u.Id != id);

            if (isPhoneNumberExists)
            {
                throw new Exception("Phone number already exists");
            }
            user.Name = input.Name;
            user.Email = input.Email;
            user.PhoneNumber = input.PhoneNumber;
            user.RoleId = input.RoleId;

            _userRepository.Update(user);
            await _userRepository.SaveChanges();
        }

        public async Task<PaginationOutput<GetUserOutput>> GetUsers(FilterInput input)
        {
            var users = await _userRepository.GetUserPagination(input);

            var result = new PaginationOutput<GetUserOutput>
            {
                Items = users.Items.Select(u => new GetUserOutput
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    RoleName = u.Role.Name
                }).ToList(),
                TotalCount = users.TotalCount
            };

            return result;
        }

        public async Task DeleteUser(int id)
        {
            if (_currentUser.UserId == id)
            {
                throw new Exception("Cannt delete yourself");
            }
            var user = await _userRepository.GetById(id);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            _userRepository.Delete(user);
            await _userRepository.SaveChanges();
        }
    }
}
