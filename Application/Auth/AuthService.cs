using Application.Auth.DTOs;
using Application.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Application.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IGenericRepository<RefreshToken> _refreshTokenRepo;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IConfiguration _configuration;
        private readonly ICurrentUser _currentUser;
        public AuthService(IUserRepository userRepo, IConfiguration configuration, IGenericRepository<RefreshToken> refreshTokenRepo, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService, ICurrentUser currentUser)
        {
            _userRepo = userRepo;
            _refreshTokenRepo = refreshTokenRepo;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
            _configuration = configuration;
            _currentUser = currentUser;
        }

        public async Task<LoginOutput> Login(LoginInput input)
        {
            var user = await _userRepo.GetByEmailWithRoleAsync(input.Username.ToLower().Trim());
            if (user == null)
            {
                throw new Exception("Invalid username or password");
            }
            if (!_passwordHasher.Verify(input.Password, user.Password))
            {
                throw new Exception("Invalid username or password");
            }

            // Genreate JWT token and return it to the client
            var jwtTokenResult = _jwtTokenService.GenerateToken(user);
            var refreshTokenResult = _jwtTokenService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshTokenResult,
                UserId = user.Id,
                TenantId = user.TenantId,
                CreatedBy = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(int.Parse(_configuration["refreshToken:ExpiryDays"]))
            };

            await _refreshTokenRepo.Insert(refreshTokenEntity);
            await _refreshTokenRepo.SaveChanges();

            var result = new LoginOutput
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = new LoginRoleOutput
                {
                    Id = user.Role.Id,
                    Name = user.Role.Name
                },
                AccessToken = new LoginTokenOutput
                {
                    Token = jwtTokenResult.AccessToken,
                    ExpiresAt = jwtTokenResult.ExpiresAt
                },
                RefreshToken = new LoginTokenOutput
                {
                    Token = refreshTokenResult,
                    ExpiresAt = refreshTokenEntity.ExpiresAt
                },
            };
            return result;
        }

        public async Task<RefreshTokenOutput> RefreshToken(string token)
        {
            var tokenResult = await _refreshTokenRepo.FirstOrDefault(x => x.Token == token && x.UserId == _currentUser.UserId);
            if (tokenResult == null || tokenResult.ExpiresAt < DateTime.UtcNow || tokenResult.IsRevoked)
            {
                throw new Exception("Invalid refresh token");
            }

            tokenResult.IsRevoked = true;
            _refreshTokenRepo.Update(tokenResult);

            var user = await _userRepo.GetByIdWithRole(tokenResult.UserId);
            var jwtTokenResult = _jwtTokenService.GenerateToken(user);
            var refreshTokenResult = _jwtTokenService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshTokenResult,
                UserId = user.Id,
                TenantId = user.TenantId,
                CreatedBy = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(int.Parse(_configuration["refreshToken:ExpiryDays"]))
            };

            await _refreshTokenRepo.Insert(refreshTokenEntity);
            await _refreshTokenRepo.SaveChanges();

            return new RefreshTokenOutput
            {
                AccessToken = new LoginTokenOutput
                {
                    Token = jwtTokenResult.AccessToken,
                    ExpiresAt = jwtTokenResult.ExpiresAt
                },
                RefreshToken = new LoginTokenOutput
                {
                    Token = refreshTokenResult,
                    ExpiresAt = refreshTokenEntity.ExpiresAt
                }
            };
        }
    }
}
