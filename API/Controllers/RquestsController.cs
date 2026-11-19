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
                var file = new UploadFileInput
                {
                    FileName = input.File.FileName,
                    ContentType = input.File.ContentType,
                    Length = input.File.Length,
                    Content = input.File.OpenReadStream()
                };

                await _requestService.UploadAttachments(input.Token, input.ItemId, file);
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

        [Authorize(Roles = RoleNames.Employee)]
        [HttpPost("UpdateRequestItemStatus")]
        public async Task<IActionResult> UpdateRequestItemStatus([FromBody] UpdateRequestItemStatusInput input)
        {
            try
            {
                await _requestService.UpdateRequestItemStatus(input);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
