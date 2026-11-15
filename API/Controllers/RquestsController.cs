using API.Models;
using Application.Files.DTOs;
using Application.Requests;
using Application.Requests.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RquestsController : ControllerBase
    {
        private readonly IRequestService _requestService;
        public RquestsController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [Authorize(Roles = RoleNames.Employee)]
        [HttpPost("CreateRequest")]
        public async Task<IActionResult> CreateRequest([FromBody] SaveRequestInput input)
        {
            try
            {
                await _requestService.CreateRequest(input);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("GetDocumentRequestDetails/{token}")]
        public async Task<IActionResult> GetDocumentRequestDetails(string token)
        {
            try
            {
                var details = await _requestService.GetDocumentRequestDetails(token);
                return Ok(details);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("UploadAttachments")]
        public async Task<IActionResult> UploadAttachments([FromForm] UploadAttachmentsForm input)
        {
            try
            {
                var files = (input.Files ?? new List<IFormFile>()).Select(f => new UploadFileInput
                {
                    FileName = f.FileName,
                    ContentType = f.ContentType,
                    Length = f.Length,
                    Content = f.OpenReadStream()
                }).ToList();

                await _requestService.UploadAttachments(input.Token, input.ItemId, files);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("GetDocumentRequests")]
        public async Task<IActionResult> GetDocumentRequestPagination([FromBody] RequestFilterInput input)
        {
            try
            {
                var result = await _requestService.GetDocumentRequestPagination(input);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }   
    }
}
