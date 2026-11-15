namespace Application.Clients.DTOs
{
    public class GetClientOutput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? AssignedEmployeeId { get; set; }
        public string? AssignedEmployeeName { get; set; }
    }
}
