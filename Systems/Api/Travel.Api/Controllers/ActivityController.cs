namespace Travel.Api.App;

using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using Travel.Common.Security;
using Travel.Services.Activities;
using Travel.Services.Logger;

/// <summary>
/// Controller for managing activities.
/// </summary>

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Product")]
[Route("v{version:apiVersion}/[controller]")]
public class ActivityController : Controller
{
    private readonly IAppLogger logger;
    private readonly IActivityService activityService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActivityController"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="activityService">The activity service instance.</param>
    public ActivityController(IAppLogger logger, IActivityService activityService)
    {
        this.logger = logger;
        this.activityService = activityService;
    }

    /// <summary>
    /// Gets all activities.
    /// </summary>
    /// <returns>A list of all activities.</returns>
    [HttpGet("")]
    [SwaggerOperation(Summary = "Get all activities", Description = "Retrieves all activities.")]
    [SwaggerResponse(200, "Success", typeof(IEnumerable<ActivityModel>))]
    [SwaggerResponseExample(200, typeof(ActivityModelExample))]
    [SwaggerResponse(400, "Bad Request", typeof(string))]
    [SwaggerResponse(401, "Unauthorized", typeof(string))]
    [SwaggerResponse(404, "Not Found", typeof(string))]
    public async Task<IEnumerable<ActivityModel>> GetAll()
    {
        var result = await activityService.GetAll();

        return result;
    }



    /// <summary>
    /// Gets an activity by ID.
    /// </summary>
    /// <param name="id">The activity ID.</param>
    /// <returns>The activity with the specified ID.</returns>
    [HttpGet("{id:Guid}")]
    [SwaggerOperation(Summary = "Get activitiy by id", Description = "")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await activityService.GetById(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Gets an activity by ID.
    /// </summary>
    /// <param name="id">The activity ID.</param>
    /// <returns>The activity with the specified ID.</returns>
    [HttpGet("day/{dayId:Guid}")]
    [SwaggerOperation(Summary = "Get activitiy by day id", Description = "")]
    public async Task<IEnumerable<ActivityModel>> GetByDayId([FromRoute] Guid dayId)
    {
        var result = await activityService.GetByDayId(dayId);
        return result;
    }

    /// <summary>
    /// Creates a new activity.
    /// </summary>
    /// <param name="request">The create model request.</param>
    /// <returns>The created activity.</returns>
    [HttpPost("")]
    [SwaggerOperation(Summary = "Create activitiy", Description = "")]
    public async Task<ActivityModel> Create(CreateModel request)
    {
        var result = await activityService.Create(request);

        return result;
    }

    /// <summary>
    /// Updates an existing activity.
    /// </summary>
    /// <param name="id">The activity ID.</param>
    /// <param name="request">The update model request.</param>
    [HttpPut("{id:Guid}")]
    [SwaggerOperation(Summary = "Update activitiy", Description = "")]
    public async Task Update([FromRoute] Guid id, UpdateModel request)
    {
        await activityService.Update(id, request);
    }

    /// <summary>
    /// Deletes an activity.
    /// </summary>
    /// <param name="id">The activity ID.</param>
    [HttpDelete("{id:Guid}")]
    [SwaggerOperation(Summary = "Delete activitiy", Description = "")]
    public async Task Delete([FromRoute] Guid id)
    {
        await activityService.Delete(id);
    }

}

