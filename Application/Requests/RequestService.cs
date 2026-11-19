using Application.Auth;
using Application.DTOs;
using Application.Files;
using Application.Files.DTOs;
using Application.Repositories;
using Application.Requests.DTOs;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace Application.Requests
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IGenericRepository<DocumentRequestItem> _documentRequestItemRepository;
        private readonly IAccessLinkRepository _accessLinkRepository;
        private readonly IFileStorage _fileStorage;
        private readonly IConfiguration _configuration;
        private readonly ICurrentUser _currentUser;
        public RequestService(IRequestRepository requestRepository, IGenericRepository<DocumentRequestItem> documentRequestItemRepository, IAccessLinkRepository accessLinkRepository,
            IFileStorage fileStorage, IConfiguration configuration, ICurrentUser currentUser)
        {
            _requestRepository = requestRepository;
            _documentRequestItemRepository = documentRequestItemRepository;
            _accessLinkRepository = accessLinkRepository;
            _fileStorage = fileStorage;
            _configuration = configuration;
            _currentUser = currentUser;
        }
        public async Task CreateRequest(SaveRequestInput input)
        {
            var request = new DocumentRequest
            {
                Title = input.Title,
                Description = input.Description,
                DueDate = input.DueDate,
                ClientId = input.ClientId,
                Status = RequestStatus.InProgress
            };

            await _requestRepository.Insert(request);
            await _requestRepository.SaveChanges();

            var items = new List<DocumentRequestItem>();
            items = input.Items.Select(i => new DocumentRequestItem
            {
                Name = i.Name,
                Description = i.Description,
                DocumentRequestId = request.Id
            }).ToList();

            await _documentRequestItemRepository.InsertRange(items);

            var accessLink = new AccessLink
            {
                DocumentRequestId = request.Id,
                IsActive = true,
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
            };

            await _accessLinkRepository.Insert(accessLink);

            await _documentRequestItemRepository.SaveChanges();
        }

        public async Task<GetDocumentRequestDetailsOutput> GetDocumentRequestDetails(string token)
        {
            var accessLink = await _accessLinkRepository.FirstOrDefault(x => x.Token == token, true);
            if (!(accessLink.IsActive && accessLink.ExpiryDate.Date >= DateTime.UtcNow.Date))
            {
                throw new Exception("Invalid URL");
            }

            var request = await _requestRepository.GetByIdWithClient(accessLink.DocumentRequestId, true);

            return new GetDocumentRequestDetailsOutput
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                ClientName = request.Client.Name,
                Status = request.Status,
                Items = request.DocumentRequestItems.Select(i => new GetDocumentRequestItemOutput
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    Status = i.Status,
                    FileUrl = i.FileUrl,
                    RejectReason = i.RejectReason
                }).ToList()
            };
        }

        public async Task UploadAttachments(string token, int itemId, UploadFileInput file)
        {
            var accessLink = await _accessLinkRepository.GetByTokenWithItems(token);
            if (accessLink == null || !(accessLink.IsActive && accessLink.ExpiryDate.Date >= DateTime.UtcNow.Date))
            {
                throw new Exception("Invalid URL");
            }

            var item = accessLink.DocumentRequest.DocumentRequestItems
                .FirstOrDefault(i => i.Id == itemId && !i.IsDeleted);
            if (item == null)
            {
                throw new Exception("Request item not found");
            }

            if (item.Status == RequestItemStatus.Approved)
            {
                throw new Exception("This item is already approved");
            }

            ValidateFiles(file);

            var folder = $"{item.TenantId}/{item.Id}";
            var oldPaths = SplitFileUrl(item.FileUrl);
            var savedPaths = new List<string>();

            try
            {
                var path = await _fileStorage.Save(file.Content, folder, Path.GetExtension(file.FileName).ToLower());
                savedPaths.Add(path);

                var fileUrl = string.Join(FileUrlSeparator, savedPaths);
                if (fileUrl.Length > FileUrlMaxLength)
                {
                    throw new Exception("Too many files, please upload fewer files");
                }

                item.FileUrl = fileUrl;
                item.Status = RequestItemStatus.Submitted;
            }
            catch
            {
                foreach (var path in savedPaths)
                {
                    _fileStorage.Delete(path);
                }
                throw;
            }

            foreach (var path in oldPaths)
            {
                _fileStorage.Delete(path);
            }

            if (!accessLink.DocumentRequest.DocumentRequestItems.Any(x => x.Status != RequestItemStatus.Submitted))
            {
                var request = await _requestRepository.FirstOrDefault(x => x.Id == accessLink.DocumentRequest.Id, true);
                request.Status = RequestStatus.Submitted;
                _requestRepository.Update(request);
            }

            await _accessLinkRepository.SaveChanges();
        }

        private const string FileUrlSeparator = ";";
        private const int FileUrlMaxLength = 500;

        private static List<string> SplitFileUrl(string? fileUrl)
        {
            return string.IsNullOrEmpty(fileUrl)
                ? new List<string>()
                : fileUrl.Split(FileUrlSeparator, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        private void ValidateFiles(UploadFileInput file)
        {
            if (file == null)
            {
                throw new Exception("No file uploaded");
            }

            var maxFileSize = long.Parse(_configuration["Attachments:MaxFileSizeMB"]) * 1024 * 1024;
            var allowedExtensions = _configuration.GetSection("Attachments:AllowedExtensions")
                .GetChildren().Select(x => x.Value.ToLower()).ToList();

            if (file.Length == 0)
            {
                throw new Exception($"File {file.FileName} is empty");
            }

            if (file.Length > maxFileSize)
            {
                throw new Exception($"File {file.FileName} exceeds {_configuration["Attachments:MaxFileSizeMB"]} MB");
            }

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                throw new Exception($"File type of {file.FileName} is not allowed");
            }
        }

        public async Task<PaginationOutput<DocumentListOutput>> GetDocumentRequestPagination(RequestFilterInput input)
        {
            int? employeeId;
            if (_currentUser.Role == RoleNames.Employee)
            {
                employeeId = _currentUser.UserId;
            }
            else
            {
                employeeId = null;
            }
            var paginationResult = await _requestRepository.GetDocumentRequestPagination(input, employeeId);
            var documentList = paginationResult.Items.Select(r => new DocumentListOutput
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                DueDate = r.DueDate,
                ClientName = r.Client.Name,
                Status = r.Status
            }).ToList();

            return new PaginationOutput<DocumentListOutput>
            {
                Items = documentList,
                TotalCount = paginationResult.TotalCount
            };
        }

        public async Task UpdateRequestItemStatus(UpdateRequestItemStatusInput input)
        {
            var item = await _documentRequestItemRepository.FirstOrDefault(x => x.Id == input.RequestItemId);
            var documentRequest = await _requestRepository.GetByIdWithClient(item.DocumentRequestId);

            if (item == null)
            {
                throw new Exception("Request item not found");
            }
            if (item.Status == RequestItemStatus.Approved)
            {
                throw new Exception("This item is already approved");
            }
            item.Status = input.Status;
            if (item.Status == RequestItemStatus.Rejected)
            {
                if (string.IsNullOrEmpty(input.RejectReason))
                {
                    throw new Exception("Reject reason needed");
                }
                item.RejectReason = input.RejectReason;
            }
            _documentRequestItemRepository.Update(item);

            if (!documentRequest.DocumentRequestItems.Any(x => x.Status != RequestItemStatus.Approved))
            {
                documentRequest.Status = RequestStatus.Completed;
                _requestRepository.Update(documentRequest);
            }

            await _documentRequestItemRepository.SaveChanges();
        }
    }
}
