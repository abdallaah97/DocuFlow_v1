using Application.DTOs;

namespace Application.Requests.DTOs
{
    public class RequestFilterInput : PaginationInput
    {
        public string? Search { get; set; }
    }
}
