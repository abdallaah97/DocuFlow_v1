namespace Application.Users.DTOs
{
    public class SaveUserInput
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public int RoleId { get; set; }
    }
}
