using Domain.Entities;

namespace Application.Requests.DTOs
{
    public class UpdateRequestItemStatusInput
    {
        public RequestItemStatus Status { get; set; }
        public int RequestItemId { get; set; }
        public string? RejectReason { get; set; }
    }
}
