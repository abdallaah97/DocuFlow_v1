using Application.DTOs;
using Application.Files.DTOs;
using Application.Requests.DTOs;

namespace Application.Requests
{
    public interface IRequestService
    {
        public Task CreateRequest(SaveRequestInput input);
        public Task<GetDocumentRequestDetailsOutput> GetDocumentRequestDetails(string token);
        public Task UploadAttachments(string token, int itemId, UploadFileInput file);
        Task<PaginationOutput<DocumentListOutput>> GetDocumentRequestPagination(RequestFilterInput input);
        Task UpdateRequestItemStatus(UpdateRequestItemStatusInput input);
    }
}
