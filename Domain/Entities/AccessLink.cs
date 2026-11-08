namespace Domain.Entities
{
    public class AccessLink : IBaseEntity, ITenantEntity, ISoftDeleteEntity
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public int DocumentRequestId { get; set; }
        public DocumentRequest DocumentRequest { get; set; }
        public int TenantId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
