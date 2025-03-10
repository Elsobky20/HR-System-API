using HR_System_API.ViewModels;

namespace HR_System_API.Services.EmailServices
{
    public interface IEmailService
    {
        Task<string> SendEmailAsync(string Name, string Link, string Email);
    }
}
