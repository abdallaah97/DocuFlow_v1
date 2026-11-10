using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Auth.DTOs
{
    public class LoginOutput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public LoginRoleOutput Role { get; set; }
        public LoginTokenOutput AccessToken { get; set; }
        public LoginTokenOutput RefreshToken { get; set; }
    }

    public class LoginRoleOutput
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class LoginTokenOutput
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
