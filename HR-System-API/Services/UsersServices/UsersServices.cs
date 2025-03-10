
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

        public async Task<ApplicationUser> Create(CreateUserDTO model)
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
                    TimeOfAttend = model.TimeOfAttend,
                    TimeOfLeave = model.TimeOfLeave,
                    Gender = model.Gender,
                    DateOfWork = model.DateOfWork,
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create user: {errors}");
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while creating the user: {ex.Message}");
            }
        }

        public async Task<bool> Delete(string id)
        {

            try
            {
                var user = await GetByID(id);
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
                var user = await GetByID(model.Id);
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

        public IQueryable<ApplicationUser> GetAll()
        {
            var users = _userManager.Users;
            return users;
        }

        public async Task<ApplicationUser> GetByID(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;
        }
    }
}
