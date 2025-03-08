using HR_System.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HR_System_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        // GET: api/Account
        [HttpGet("Index")]
        public IActionResult Index()
        {
            return Ok(new { message = "Account Index" });
        }

        // POST: api/Account/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                        if (result.Succeeded)
                        {
                            var userRoles = await _userManager.GetRolesAsync(user);
                            if (userRoles.Contains("Admin"))
                            {
                                return Ok(new { message = "Admin login successful" });
                            }
                            else
                            {
                                return Ok(new { message = "User login successful" });
                            }
                        }
                        else
                        {
                            return Unauthorized(new { message = "Email or Password is incorrect" });
                        }
                    }
                    return Unauthorized(new { message = "Email or Password is incorrect" });
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }

        // POST: api/Account/Logout
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Logged out successfully" });
        }

        // GET: api/Account/Test
        [HttpGet("Test")]
        public IActionResult Test()
        {
            return Ok(new { message = "Test endpoint working" });
        }
    }

}
