namespace Domain.Entities
{
    public class DocumentRequestItem : IBaseEntity, ITenantEntity, ISoftDeleteEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DocumentRequestId { get; set; }
        public DocumentRequest DocumentRequest { get; set; }
        public RequestItemStatus Status { get; set; } = RequestItemStatus.Pending;
        public string? FileUrl { get; set; }
        public string? RejectReason { get; set; }
        public int TenantId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }

    public enum RequestItemStatus
    {
        Pending = 1,
        Submitted = 2,
        Approved = 3,
        Rejected = 4
    }
}
