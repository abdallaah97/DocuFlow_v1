using Application.Auth;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Auth
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Anonymous requests (e.g. login) have no claims -> 0 instead of throwing
        private static int ParseInt(string? value) => int.TryParse(value, out var v) ? v : 0;

        public int UserId => ParseInt(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        public string Email => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
        public string Name => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
        public int RoleId => ParseInt(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value);
        public string RoleName => _httpContextAccessor.HttpContext?.User?.FindFirst("role_name")?.Value;
        public string PhoneNumber => _httpContextAccessor.HttpContext?.User?.FindFirst("phone_number")?.Value;
        public int TenantId => ParseInt(_httpContextAccessor.HttpContext?.User?.FindFirst("tenant_id")?.Value);
    }
}
