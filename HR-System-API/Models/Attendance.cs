using System.ComponentModel.DataAnnotations;

namespace HR_System.Models
{
    public class Attendance
    {
        
        public int Id { get; set; }
        public DateTime TimeOfAttend { get; set; }
        public string Location { get; set; } = default!;
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = default!;

    }
}
