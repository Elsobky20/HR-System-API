namespace HR_System_API.DTO
{
    public class AttendanceDto
    {
        public DateTime TimeOfAttend { get; set; }
        public string Location { get; set; } = default!;
        public string EmployeeId { get; set; } = default!;
    }
}
