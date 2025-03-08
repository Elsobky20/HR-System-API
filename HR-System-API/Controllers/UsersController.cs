using HR_System.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HR_System_API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roles;

        public UsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roles)
        {
            _userManager = userManager;
            _roles = roles;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Users API is running.");
        }

        [HttpGet("AddUser")]
        public async Task<IActionResult> GetAddUser()
        {
            var roles = await _roles.Roles.Select(r => new roleViewModel { roleId = r.Id, roleName = r.Name }).ToListAsync();
            var viewModel = new AddUserViewModel
            {
                roles = roles
            };
            return Ok(viewModel);
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] AddUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!model.roles.Any(r => r.IsSelected))
            {
                ModelState.AddModelError("Roles", "Please select at least one role.");
                return BadRequest(ModelState);
            }

            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("Email", "Email already exists.");
                return BadRequest(ModelState);
            }

            var user = new IdentityUser
            {
                Email = model.Email,
                UserName = model.UserName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }

            await _userManager.AddToRolesAsync(user, model.roles.Where(r => r.IsSelected).Select(r => r.roleName));

            return Ok(new { Message = "User created successfully.", UserId = user.Id });
        }
    }


}
