using Application.DTOs;

namespace Application.Clients.DTOs
{
    public class ClientFilterInput : PaginationInput
    {
        public string? Search { get; set; }
        public int? AssignedEmployeeId { get; set; }
    }
}
