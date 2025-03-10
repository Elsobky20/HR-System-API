using HR_System.DataBase;
using HR_System.Models;
using HR_System_API.DTO;

namespace HR_System_API.Services.AttendanceServices
{

    public class AttendanceServices : IAttendanceServices
    {
        private readonly ApplicationDbContext _context;

        public AttendanceServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAttendance(AttendanceDto model)
        {
            var attendance = new Attendance
            {
                TimeOfAttend = model.TimeOfAttend,
                Location = model.Location,
                EmployeeId = model.EmployeeId
            };

            _context.Attendance.Add(attendance);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public IQueryable<Attendance> GetAllAttendances()
        {
            return _context.Attendance;
        }

     
    }

}
