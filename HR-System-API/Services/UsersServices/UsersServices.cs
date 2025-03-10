
using HR_System.DataBase;
using HR_System.Models;
using HR_System_API.Extend;
using HR_System_API.DTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.UsersServices
{
    public class UsersServices : IUsersServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext db;

        public UsersServices(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            this._userManager = userManager;
            this.db = db;
        }

        public async Task<AuthenticationDTO> Create(CreateUserDTO model)
        {
            try
            {
                var user = new ApplicationUser
                {
                    Email = model.Email,
                    Name = model.Name,
                    Address = model.Address,
                    DateOfBarth = model.DateOfBarth,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.UserName,
                    Nationalid = model.Nationalid,
                    Salary = model.Salary,
                    TimeOfAttend = DateTime.Parse(model.TimeOfAttend),
                    TimeOfLeave = DateTime.Parse(model.TimeOfLeave),
                    Gender = model.Gender,
                    DateOfWork = model.DateOfWork,
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return new()
                    {
                        Message = errors
                    };
                }

                return new()
                {
                    Email = user.Email,
                    Name = user.Name,
                    Address = user.Address,
                    DateOfBarth = user.DateOfBarth.ToLongDateString(),
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    Nationalid = user.Nationalid,
                    Salary = user.Salary,
                    TimeOfAttend = user.TimeOfAttend.ToShortTimeString(),
                    TimeOfLeave = user.TimeOfLeave.ToShortTimeString(),
                    Gender = user.Gender,
                    DateOfWork = user.DateOfWork.ToString(),
                    IsAuthenticated = true,
                    Message = "User created successfully!",
                    Id = user.Id,
                    Roles = await _userManager.GetRolesAsync(user)
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    Message = ex.Message
                };
            }
        }

        public async Task<bool> Delete(string id)
        {

            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Edit(ApplicationUser model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.Name = model.Name;
                    user.Address = model.Address;
                    user.DateOfBarth = model.DateOfBarth;
                    user.PhoneNumber = model.PhoneNumber;
                    user.UserName = model.UserName;
                    user.Gender = model.Gender;
                    user.Nationalid = model.Nationalid;
                    user.Salary = model.Salary;
                    user.DateOfWork = model.DateOfWork;
                    user.TimeOfAttend = model.TimeOfAttend;
                    user.TimeOfLeave = model.TimeOfLeave;
                    var result = await _userManager.UpdateAsync(user);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                return true;
            }

        }

        public async Task<List<AuthenticationDTO>> GetAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            var userList = new List<AuthenticationDTO>();

            foreach (var user in users)
            {
                userList.Add(new AuthenticationDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    Nationalid = user.Nationalid,
                    Salary = user.Salary,
                    TimeOfAttend = user.TimeOfAttend.ToString(),
                    TimeOfLeave = user.TimeOfLeave.ToString(),
                    Gender = user.Gender,
                    DateOfWork = user.DateOfWork.ToString(),
                    DateOfBarth = user.DateOfBarth.ToString(),
                    Roles = await _userManager.GetRolesAsync(user)
                });
            }

            return userList;
        }



        public async Task<AuthenticationDTO> GetByID(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new()
                {
                    Message = "User not found"
                };
            }

            return new AuthenticationDTO
            {
                Message = "User found",
                IsAuthenticated = true,
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Nationalid = user.Nationalid,
                Salary = user.Salary,
                TimeOfAttend = user.TimeOfAttend.ToString(),
                TimeOfLeave = user.TimeOfLeave.ToString(),
                Gender = user.Gender,
                DateOfWork = user.DateOfWork.ToString(),
                DateOfBarth = user.DateOfBarth.ToString(),
                Roles = (await _userManager.GetRolesAsync(user)).ToList()
            };
        }
    }
}
