using HR_System_API.Extend;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR_System.Models
{
    public class Absent
    {
        public int Id { get; set; }
        public DateTime day { get; set; }
        public string EmployeeId { get; set; }
        public ApplicationUser Employee { get; set; } = default!;

    }
}
