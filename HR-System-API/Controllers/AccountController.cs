using HR_System.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;
using DAL.DTO;
using BLL.Helper;
using HR_System_API.Extend;
using HR_System_API.Services.EmailServices;

namespace HR_System_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailServices;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailService emailServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailServices = emailServices;
        }

        // GET: api/Account
        [HttpGet("Index")]
        public IActionResult Index()
        {
            return Ok(new { message = "Account Index" });
        }


        // POST: api/Account/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
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


        #region Forget Password

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    // Generate token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    // Create password reset link
                    var passwordResetLink = Url.Action("ResetPassword", "Account", new { Email = model.Email, Token = token }, Request.Scheme);

                    var result = await _emailServices.SendEmailAsync(user.Name, passwordResetLink, model.Email);

                    //return Ok(new { Message = "Password reset link sent to email." });
                    return Ok(result);
                }

                return NotFound("User not found.");
            }

            return BadRequest("Invalid model.");
        }

        #endregion


        #region Reset Password

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var resetResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (resetResult.Succeeded)
                    {
                        return Ok(new { Message = "Password reset successful." });
                    }

                    return BadRequest(resetResult.Errors);
                }

                return NotFound("User not found.");
            }

            return BadRequest("Invalid model.");
        }

        #endregion


    }
}


