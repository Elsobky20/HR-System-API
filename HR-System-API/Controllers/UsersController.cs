using BLL.Services.UsersServices;
using HR_System.ViewModels;
using HR_System_API.Extend;
using HR_System_API.ViewModels;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUsersServices _usersService;

        public UsersController(UserManager<ApplicationUser> userManager, IUsersServices usersService)
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
        public async Task<IActionResult> Edit(string id, [FromBody] ApplicationUser model)
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


        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var createdUser = await _usersService.Create(model);
                return Ok(new { Message = "User created successfully", UserId = createdUser.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{userId}/roles/{roleName}")]
        public async Task<IActionResult> AddUserToRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            //// Check if the role exists
            //var roleExists = await _userManager.IsInRoleAsync(user, roleName);
            //if (!roleExists)
            //{
            //    return BadRequest(new { Message = $"Role '{roleName}' does not exist." });
            //}

            // Add the user to the role
            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                return Ok(new { Message = $"User added to role '{roleName}' successfully." });
            }

            return BadRequest(new { Message = "Failed to add user to role.", Errors = result.Errors });
        }


    }
}
