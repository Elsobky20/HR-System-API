using HR_System.Models;
using HR_System_API.Extend;
using HR_System_API.DTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.UsersServices
{
    public interface IUsersServices
    {
        Task<List<AuthenticationDTO>> GetAllAsync();
        Task<AuthenticationDTO> GetByID(string id);
        Task<bool> Edit(ApplicationUser model);
        Task<bool> Delete(string id);
        Task<AuthenticationDTO> Create(CreateUserDTO model);
    }
}
