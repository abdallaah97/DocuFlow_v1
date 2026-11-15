using Application.Clients;
using Application.Clients.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;
        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [Authorize(Roles = $"{RoleNames.PlatformAdmin},{RoleNames.Owner}")]
        [HttpPost("CreateClient")]
        public async Task<IActionResult> CreateClient([FromBody] SaveClientInput input)
        {
            try
            {
                await _clientService.CreateClient(input);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = $"{RoleNames.PlatformAdmin},{RoleNames.Owner}")]
        [HttpPut("UpdateClient/{id}")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] SaveClientInput input)
        {
            try
            {
                await _clientService.UpdateClient(id, input);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = $"{RoleNames.PlatformAdmin},{RoleNames.Owner}")]
        [HttpGet("GetClients")]
        public async Task<IActionResult> GetClients([FromQuery] ClientFilterInput input)
        {
            try
            {
                var clients = await _clientService.GetClients(input);
                return Ok(clients);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = RoleNames.Employee)]
        [HttpGet("GetMyClients")]
        public async Task<IActionResult> GetMyClients([FromQuery] ClientFilterInput input)
        {
            try
            {
                var clients = await _clientService.GetMyClients(input);
                return Ok(clients);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = $"{RoleNames.PlatformAdmin},{RoleNames.Owner}")]
        [HttpGet("GetClientById/{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            try
            {
                var client = await _clientService.GetClientById(id);
                return Ok(client);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = $"{RoleNames.PlatformAdmin},{RoleNames.Owner}")]
        [HttpDelete("DeleteClient/{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            try
            {
                await _clientService.DeleteClient(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = $"{RoleNames.PlatformAdmin},{RoleNames.Owner}")]
        [HttpPut("AssignEmployee/{id}")]
        public async Task<IActionResult> AssignEmployee(int id, [FromBody] AssignEmployeeInput input)
        {
            try
            {
                await _clientService.AssignEmployee(id, input);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
