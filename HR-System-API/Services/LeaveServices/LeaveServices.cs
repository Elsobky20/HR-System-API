using HR_System.DataBase;
using HR_System.Models;
using HR_System_API.DTO;

namespace HR_System_API.Services.LeaveServices
{

    public class LeaveServices : ILeaveServices

    {
        private readonly ApplicationDbContext _context;

        public LeaveServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddLeave(LeaveDTO model)
        {
            var leave = new Leave
            {
                TimeOfLeave = model.TimeOfLeave,
                Location = model.Location,
                EmployeeId = model.EmployeeId
            };

            _context.Leave.Add(leave);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public IQueryable<Leave> GetAllLeaves()
        {
            return _context.Leave;
        }

    }

}
