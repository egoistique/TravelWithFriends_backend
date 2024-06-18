namespace Travel.API.Controllers;

using AutoMapper;
using Travel.Services.UserAccount;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Travel.Common.Exceptions;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Product")]
[Route("v{version:apiVersion}/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ILogger<AccountsController> logger;
    private readonly IUserAccountService userAccountService;

    public AccountsController(IMapper mapper, ILogger<AccountsController> logger, IUserAccountService userAccountService)
    {
        this.mapper = mapper;
        this.logger = logger;
        this.userAccountService = userAccountService;
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="request">The user registration details.</param>
    /// <returns>The registered user account details.</returns>
    [HttpPost("")]
    [SwaggerOperation(Summary = "Register user", Description = "Registers a new user account with the provided details.")]
    [SwaggerResponse(200, "Success", typeof(UserAccountModel))]
    [SwaggerResponse(400, "Bad Request", typeof(string))]
    public async Task<UserAccountModel> Register([FromQuery] RegisterUserAccountModel request)
    {
        var user = await userAccountService.Create(request);
        return user;
    }

    /// <summary>
    /// Changes the status of a user.
    /// </summary>
    /// <param name="userId">The ID of the user whose status is to be changed.</param>
    /// <param name="model">The new status details.</param>
    /// <returns>A success message if the status was changed successfully.</returns>
    [HttpPut("status/{userId}")]
    [SwaggerOperation(Summary = "Change user status", Description = "Changes the status of an existing user.")]
    [SwaggerResponse(200, "Success", typeof(string))]
    [SwaggerResponse(400, "Bad Request", typeof(string))]
    [SwaggerResponse(404, "Not Found", typeof(string))]
    public async Task<IActionResult> ChangeUserStatus(Guid userId, [FromBody] ChangeUserStatusModel model)
    {
        try
        {
            await userAccountService.ChangeUserStatus(userId, model.NewStatus);
            return Ok("User status changed successfully.");
        }
        catch (ProcessException ex)
        {
            return BadRequest($"Failed to change user status: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets the status of a user.
    /// </summary>
    /// <param name="userId">The ID of the user whose status is to be retrieved.</param>
    /// <returns>The current status of the user.</returns>
    [HttpGet("status/{userId}")]
    [SwaggerOperation(Summary = "Get status", Description = "Retrieves the current status of the specified user.")]
    [SwaggerResponse(200, "Success", typeof(string))]
    [SwaggerResponse(400, "Bad Request", typeof(string))]
    [SwaggerResponse(404, "Not Found", typeof(string))]
    public async Task<IActionResult> GetStatus(Guid userId)
    {
        try
        {
            var status = await userAccountService.GetStatus(userId);
            return Ok(status);
        }
        catch (ProcessException ex)
        {
            return BadRequest($"Failed to get user status: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets the user ID by email.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <returns>The ID of the user.</returns>
    [HttpGet("getid/{email}")]
    [SwaggerOperation(Summary = "Get user id", Description = "Retrieves the user ID by the provided email address.")]
    [SwaggerResponse(200, "Success", typeof(Guid))]
    [SwaggerResponse(400, "Bad Request", typeof(string))]
    [SwaggerResponse(404, "Not Found", typeof(string))]
    public async Task<IActionResult> GetId(String email)
    {
        try
        {
            var userId = await userAccountService.GetUserIdByEmail(email);
            return Ok(userId);
        }
        catch (ProcessException ex)
        {
            return NotFound($"User not found: {ex.Message}");
        }
    }
    /// <summary>
    /// Deletes a user.
    /// </summary>
    /// <param name="email">The email address of the user to delete.</param>
    /// <returns>A success message if the user was deleted successfully.</returns>
    [HttpDelete("{email}")]
    [SwaggerOperation(Summary = "Delete user", Description = "Deletes a user if they have no trips or only trips with a single participant.")]
    [SwaggerResponse(200, "Success", typeof(string))]
    [SwaggerResponse(400, "Bad Request", typeof(string))]
    [SwaggerResponse(404, "Not Found", typeof(string))]
    public async Task<IActionResult> DeleteUser(string email)
    {
        try
        {
            var result = await userAccountService.Delete(email);
            if (result)
            {
                return Ok("User deleted successfully.");
            }
            else
            {
                return BadRequest("Cannot delete user because they have trips with multiple participants.");
            }
        }
        catch (ProcessException ex)
        {
            return BadRequest($"Failed to delete user: {ex.Message}");
        }
    }


}
