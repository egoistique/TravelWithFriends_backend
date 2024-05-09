namespace Travel.Api.App;

using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Common.Security;
using Travel.Services.Activities;
using Travel.Services.Logger;

[ApiController]
//[Authorize]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Product")]
[Route("v{version:apiVersion}/[controller]")]
public class ActivityController : Controller
{
    private readonly IAppLogger logger;
    private readonly IActivityService activityService;

    public ActivityController(IAppLogger logger, IActivityService activityService)
    {
        this.logger = logger;
        this.activityService = activityService;
    }

    [HttpGet("")]
    //[Authorize(AppScopes.TripsRead)]
    public async Task<IEnumerable<ActivityModel>> GetAll()
    {
        var result = await activityService.GetAll();

        return result;
    }

    [HttpGet("{id:Guid}")]
    //[Authorize(AppScopes.TripsRead)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await activityService.GetById(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("day/{dayId:Guid}")]
    //[AllowAnonymous]
    public async Task<IEnumerable<ActivityModel>> GetByDayId([FromRoute] Guid dayId)
    {
        var result = await activityService.GetByDayId(dayId);
        return result;
    }

    [HttpPost("")]
    //[Authorize(AppScopes.TripsWrite)]
    public async Task<ActivityModel> Create(CreateModel request)
    {
        var result = await activityService.Create(request);

        return result;
    }

    [HttpPut("{id:Guid}")]
    //[Authorize(AppScopes.TripsWrite)]
    public async Task Update([FromRoute] Guid id, UpdateModel request)
    {
        await activityService.Update(id, request);
    }

    [HttpDelete("{id:Guid}")]
    //[Authorize(AppScopes.TripsWrite)]
    public async Task Delete([FromRoute] Guid id)
    {
        await activityService.Delete(id);
    }

}

