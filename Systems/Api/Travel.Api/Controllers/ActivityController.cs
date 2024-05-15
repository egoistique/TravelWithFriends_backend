namespace Travel.Api.App;

using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Travel.Common.Security;
using Travel.Services.Activities;
using Travel.Services.Logger;

[ApiController]
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
    [SwaggerOperation(Summary = "Get all activities", Description = "")]
    public async Task<IEnumerable<ActivityModel>> GetAll()
    {
        var result = await activityService.GetAll();

        return result;
    }

    [HttpGet("{id:Guid}")]
    [SwaggerOperation(Summary = "Get activitiy by id", Description = "")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await activityService.GetById(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("day/{dayId:Guid}")]
    [SwaggerOperation(Summary = "Get activitiy by day id", Description = "")]
    public async Task<IEnumerable<ActivityModel>> GetByDayId([FromRoute] Guid dayId)
    {
        var result = await activityService.GetByDayId(dayId);
        return result;
    }

    [HttpPost("")]
    [SwaggerOperation(Summary = "Create activitiy", Description = "")]
    public async Task<ActivityModel> Create(CreateModel request)
    {
        var result = await activityService.Create(request);

        return result;
    }

    [HttpPut("{id:Guid}")]
    [SwaggerOperation(Summary = "Update activitiy", Description = "")]
    public async Task Update([FromRoute] Guid id, UpdateModel request)
    {
        await activityService.Update(id, request);
    }

    [HttpDelete("{id:Guid}")]
    [SwaggerOperation(Summary = "Delete activitiy", Description = "")]
    public async Task Delete([FromRoute] Guid id)
    {
        await activityService.Delete(id);
    }

}

