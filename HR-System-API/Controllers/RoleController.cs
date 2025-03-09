using BLL.Services.RolesServices;
using HR_System.ViewModels;
using HR_System_API.Extend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TestCoreApp.Models;
using JsonException = Newtonsoft.Json.JsonException;

[Authorize(Roles = clsRoles.roleAdmin)] // Ensure only admins can access this API
[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _user;
    private readonly RoleManager<IdentityRole> _roles;
    private readonly IRolesServices rolesServ;

    public RoleController(UserManager<ApplicationUser> user, RoleManager<IdentityRole> roles , IRolesServices rolesServ)
    {
        _user = user;
        _roles = roles;
        this.rolesServ = rolesServ;
    }

   

    [HttpGet]
    public IActionResult GetRoles()
    {
        var allRoles = rolesServ.GetAll();
        return Ok(allRoles);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoleById(string id)
    {
        var role = await rolesServ.GetByID(id);
        if (role == null)
        {
            return NotFound(new { Success = false, Message = "Role not found." });
        }
        return Ok(role);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] IdentityRole model)
    {
        if (ModelState.IsValid)
        {
            var result = await rolesServ.Create(model);
            if (result)
            {
                return Ok(new { Success = true, Message = "Role created successfully." });
            }
            return BadRequest(new { Success = false, Message = "Error creating role." });
        }
        return BadRequest(ModelState);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var role = await rolesServ.GetByID(id);
        if (role == null)
        {
            return NotFound(new { Success = false, Message = "Role not found." });
        }

        var result = await rolesServ.Delete(id);
        if (result)
        {
            return Ok(new { Success = true, Message = "Role deleted successfully." });
        }

        return BadRequest(new { Success = false, Message = "Error deleting role." });
    }

    // GET: api/Roles/addRoles/{userId}
    [HttpGet("addRoles/{userId}")]
    public async Task<IActionResult> GetUserRoles(string userId)
    {
        var user = await _user.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new { Message = "User not found." });
        }

        var userRoles = await _user.GetRolesAsync(user);
        var allRoles = await _roles.Roles.ToListAsync();

        if (allRoles == null)
        {
            return NotFound(new { Message = "No roles found." });
        }

        var roleList = allRoles.Select(r => new roleViewModel
        {
            roleId = r.Id,
            roleName = r.Name,
            useRole = userRoles.Any(x => x == r.Name)
        }).ToList();

        var response = new
        {
            UserName = user.UserName,
            UserId = userId,
            Roles = roleList
        };

        return Ok(response); // Return user and roles as JSON
    }

    // POST: api/Roles/addRoles/{userId}
    [HttpPost("addRoles/{userId}")]
    public async Task<IActionResult> UpdateUserRoles(string userId, [FromBody] string jsonRoles)
    {
        var user = await _user.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new { Message = "User not found." });
        }

        List<roleViewModel> myRoles;
        try
        {
            myRoles = JsonConvert.DeserializeObject<List<roleViewModel>>(jsonRoles);
        }
        catch (JsonException)
        {
            return BadRequest(new { Message = "Invalid JSON format for roles." });
        }

        var userRoles = await _user.GetRolesAsync(user);

        foreach (var role in myRoles)
        {
            if (userRoles.Any(x => x == role.roleName.Trim()) && !role.useRole)
            {
                await _user.RemoveFromRoleAsync(user, role.roleName.Trim());
            }

            if (!userRoles.Any(x => x == role.roleName.Trim()) && role.useRole)
            {
                await _user.AddToRoleAsync(user, role.roleName.Trim());
            }
        }

        return Ok(new { Message = "Roles updated successfully." });
    }
}

