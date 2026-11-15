using Domain.Entities;

namespace Application.Requests.DTOs
{
    public class GetDocumentRequestDetailsOutput : DocumentListOutput
    {
        public List<GetDocumentRequestItemOutput> Items { get; set; }
    }

    public class GetDocumentRequestItemOutput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileUrl { get; set; }
        public string RejectReason { get; set; }
        public RequestItemStatus Status { get; set; }
        public string StatusStr { get { return Status.ToString(); } }
    }
}
