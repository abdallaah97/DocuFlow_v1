namespace Application.Clients.DTOs
{
    public class SaveClientInput
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
