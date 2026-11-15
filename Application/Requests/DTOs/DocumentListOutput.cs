using Domain.Entities;

namespace Application.Requests.DTOs
{
    public class DocumentListOutput
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public RequestStatus Status { get; set; }
        public string StatusStr { get { return Status.ToString(); } }
        public string ClientName { get; set; }
    }
}
