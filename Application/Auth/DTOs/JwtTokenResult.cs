namespace Application.Auth.DTOs
{
    public class JwtTokenResult
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
