namespace Application.Clients.DTOs
{
    public class AssignEmployeeInput
    {
        // null = unassign the current employee
        public int? EmployeeId { get; set; }
    }
}
