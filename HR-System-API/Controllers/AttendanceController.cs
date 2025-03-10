using HR_System.Models;
using HR_System_API.Services.AttendanceServices;
using HR_System_API.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HR_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {

        private readonly IAttendanceServices _attendanceService;

        public AttendanceController(IAttendanceServices attendanceService)
        {
            _attendanceService = attendanceService;
        }


        [HttpPost]
        public async Task<IActionResult> AddAttendance([FromBody] AttendanceDto attendanceDto)
        {
            if (attendanceDto == null)
            {
                return BadRequest("Attendance data is required.");
            }

            var result = await _attendanceService.AddAttendance(attendanceDto);
            if (result)
            {
                return Ok(new { Message = "Attendance added successfully." });
            }
            return StatusCode(500, "Error adding attendance.");
        }
    }
}

