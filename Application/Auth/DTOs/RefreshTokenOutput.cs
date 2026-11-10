namespace Application.Auth.DTOs
{
    public class RefreshTokenOutput
    {
        public LoginTokenOutput AccessToken { get; set; }
        public LoginTokenOutput RefreshToken { get; set; }
    }
}
