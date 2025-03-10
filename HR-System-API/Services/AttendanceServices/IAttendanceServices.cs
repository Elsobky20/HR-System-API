using HR_System.Models;
using HR_System_API.DTO;

namespace HR_System_API.Services.AttendanceServices
{
    public interface IAttendanceServices
    {
        Task<bool> AddAttendance(AttendanceDto model);
        IQueryable<Attendance> GetAllAttendances();
    }
}
