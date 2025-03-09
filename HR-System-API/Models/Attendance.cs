using HR_System_API.Extend;
using System.ComponentModel.DataAnnotations;

namespace HR_System.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public DateTime TimeOfAttend { get; set; }
        public string Location { get; set; } = default!;
        public string EmployeeId { get; set; }
        public ApplicationUser Employee { get; set; } = default!;
    }
}
