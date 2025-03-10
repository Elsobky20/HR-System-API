using HR_System.Models;
using HR_System_API.DTO;

namespace HR_System_API.Services.LeaveServices
{
    public interface ILeaveServices
    {
        Task<bool> AddLeave(LeaveDTO model);
        IQueryable<Leave> GetAllLeaves();
    }
}
