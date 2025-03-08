﻿using BLL.Services.UsersServices;
using HR_System.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HR_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUsersServices _usersService;

        public UsersController(UserManager<IdentityUser> userManager, IUsersServices usersService)
        {
            _userManager = userManager;
            _usersService = usersService;
        }

        #region Get Users
        // GET: api/Users
        [HttpGet]
        public IActionResult GetUsers()
        {
            var allUsers = _usersService.GetAll();
            return Ok(allUsers);
        }
        #endregion

        #region Get User Details
        // GET: api/Users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(string id)
        {
            var user = await _usersService.GetByID(id);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound(new { Message = "User not found." });
        }
        #endregion

        #region Edit User
        // PUT: api/Users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] IdentityUser model)
        {
            model.Id = id; // Ensure the user ID is correctly set

            if (await _usersService.Edit(model))
            {
                return Ok(new { Success = true, Message = "User updated successfully." });
            }

            return BadRequest(new { Success = false, Message = "Failed to update user." });
        }
        #endregion

        #region Delete User
        // DELETE: api/Users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _usersService.GetByID(id);
            if (user != null)
            {
                var result = await _usersService.Delete(id);
                if (result)
                {
                    return Ok(new { Success = true, Message = "User deleted successfully." });
                }
                return BadRequest(new { Success = false, Message = "Failed to delete user." });
            }
            return NotFound(new { Message = "User not found." });
        }
        #endregion

       
    }
}
