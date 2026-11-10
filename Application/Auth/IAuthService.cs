using Application.Auth.DTOs;

namespace Application.Auth
{
    public interface IAuthService
    {
        Task<LoginOutput> Login(LoginInput input);
        Task<RefreshTokenOutput> RefreshToken(string token);
    }
}
