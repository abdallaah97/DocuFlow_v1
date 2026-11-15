using Application.DTOs;
using Application.Requests.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repositories
{
    public interface IRequestRepository : IGenericRepository<DocumentRequest>
    {
        public Task<DocumentRequest> GetByIdWithClient(int id, bool ignoreQueryFilters = false);
        Task<PaginationOutput<DocumentRequest>> GetDocumentRequestPagination(RequestFilterInput input, int? employeeId);
    }
}
