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

    [HttpPost("")]
    [SwaggerOperation(Summary = "Register user", Description = "")]
    public async Task<UserAccountModel> Register([FromQuery] RegisterUserAccountModel request)
    {
        var user = await userAccountService.Create(request);
        return user;
    }

    [HttpPut("status/{userId}")]
    [SwaggerOperation(Summary = "Change user status", Description = "")]
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

    [HttpGet("status/{userId}")]
    [SwaggerOperation(Summary = "Get status", Description = "")]
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

    [HttpGet("getid/{email}")]
    [SwaggerOperation(Summary = "Get user id", Description = "")]
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
}
