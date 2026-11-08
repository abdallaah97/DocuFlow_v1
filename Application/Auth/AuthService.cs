using Application.Abstraction;
using Application.Auth.DTOs;
using Application.Repositories;
using Domain.Entities;

namespace Application.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly IPasswordHasher _passwordHasher;
        public AuthService(IGenericRepository<User> userRepo, IPasswordHasher passwordHasher)
        {
            _userRepo = userRepo;
            _passwordHasher = passwordHasher;
        }

        public async Task Login(LoginInput input)
        {
            var user = await _userRepo.FirstOrDefault(u => u.Email.ToLower().Trim() == input.Username.ToLower().Trim());
            if (user == null)
            {
                throw new Exception("Invalid username or password");
            }
            if (!_passwordHasher.Verify(input.Password, user.Password))
            {
                throw new Exception("Invalid username or password");
            }

        }
    }
}
