using System.ComponentModel.DataAnnotations;
namespace HR_System_API.ViewModels
{

    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime DateOfBarth { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "National ID is required.")]
        public string Nationalid { get; set; }

        [Required(ErrorMessage = "Salary is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive value.")]
        public double Salary { get; set; }

        public DateTime TimeOfAttend { get; set; }

        public DateTime TimeOfLeave { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Date of Work is required.")]
        public DateTime DateOfWork { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }
    }

}
