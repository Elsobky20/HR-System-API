namespace HR_System_API.DTO
{
    public class LeaveDTO
    {
        public DateTime TimeOfLeave { get; set; }
        public string Location { get; set; } = default!;
        public string EmployeeId { get; set; } = default!;
    }
}
