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
        IQueryable<ApplicationUser> GetAll();
        Task<ApplicationUser> GetByID(string id);
        Task<bool> Edit(ApplicationUser model);
        Task<bool> Delete(string id);
        Task<ApplicationUser> Create(CreateUserDTO model);
    }
}
