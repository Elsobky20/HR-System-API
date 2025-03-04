using System.ComponentModel.DataAnnotations;

namespace HR_System.Models
{
    public class Leave
    {
        
        public int Id { get; set; }
        public DateTime TimeOfLeave { get; set; }

        public string Location { get; set; } = default!;

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = default!;

    }
}
