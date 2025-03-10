using HR_System_API.Services.LeaveServices;
using HR_System_API.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveServices _leaveService;

        public LeaveController(ILeaveServices leaveService)
        {
            _leaveService = leaveService;
        }

        // POST: api/Leave
        [HttpPost]
        public async Task<IActionResult> AddLeave([FromBody] LeaveDTO leaveDto)
        {
            if (leaveDto == null)
            {
                return BadRequest("Leave data is required.");
            }

            var result = await _leaveService.AddLeave(leaveDto);
            if (result)
            {
                return Ok(new { Message = "Leave added successfully." });
            }
            return StatusCode(500, "Error adding leave.");
        }

       
    }
}

