using Application.Auth.DTOs;
using Domain.Entities;

namespace Application.Auth
{
    public interface IJwtTokenService
    {
        JwtTokenResult GenerateToken(User user);
        string GenerateRefreshToken();
    }
}
