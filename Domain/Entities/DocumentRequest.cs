namespace Domain.Entities
{
    public class DocumentRequest : IBaseEntity, ITenantEntity, ISoftDeleteEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.Draft;
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public int TenantId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<AccessLink> AccessLinks { get; set; }
        public ICollection<DocumentRequestItem> DocumentRequestItems { get; set; }
    }

    public enum RequestStatus
    {
        Draft = 1,
        InProgress = 2,
        Submitted = 3,
        Completed = 4,
    }
}
