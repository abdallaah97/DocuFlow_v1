namespace Domain.Entities
{
    public class Client : IBaseEntity, ITenantEntity, ISoftDeleteEntity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? AssignedEmployeeId { get; set; }
        public User? AssignedEmployee { get; set; }
        public int TenantId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<DocumentRequest> DocumentRequests { get; set; }

    }
}
