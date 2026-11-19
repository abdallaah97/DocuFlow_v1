using Application.DTOs;
using Domain.Entities;

namespace Application.Requests.DTOs
{
    public class RequestFilterInput : PaginationInput
    {
        public string? Search { get; set; }
        public RequestStatus? Status { get; set; }
    }
}
