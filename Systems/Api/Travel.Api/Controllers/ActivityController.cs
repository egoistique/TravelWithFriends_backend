namespace Travel.Api.App
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;
    using Travel.Common.Security;
    using Travel.Services.Activities;
    using Travel.Services.Logger;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

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
        [SwaggerOperation(Summary = "Get activity by ID", Description = "Retrieves an activity by its ID.")]
        [SwaggerResponse(200, "Success", typeof(ActivityModel))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        [SwaggerResponse(401, "Unauthorized", typeof(string))]
        [SwaggerResponse(404, "Not Found", typeof(string))]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var result = await activityService.GetById(id);
            if (result == null)
                return NotFound("Activity not found.");

            return Ok(result);
        }

        /// <summary>
        /// Gets activities by day ID.
        /// </summary>
        /// <param name="dayId">The day ID.</param>
        /// <returns>A list of activities for the specified day.</returns>
        [HttpGet("day/{dayId:Guid}")]
        [SwaggerOperation(Summary = "Get activities by day ID", Description = "Retrieves activities by day ID.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ActivityModel>))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        [SwaggerResponse(401, "Unauthorized", typeof(string))]
        [SwaggerResponse(404, "Not Found", typeof(string))]
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
        [SwaggerOperation(Summary = "Create activity", Description = "Creates a new activity with the provided details.")]
        [SwaggerResponse(200, "Success", typeof(ActivityModel))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        [SwaggerResponse(401, "Unauthorized", typeof(string))]
        public async Task<ActivityModel> Create([FromBody] CreateModel request)
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
        [SwaggerOperation(Summary = "Update activity", Description = "Updates an existing activity with the provided details.")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        [SwaggerResponse(401, "Unauthorized", typeof(string))]
        [SwaggerResponse(404, "Not Found", typeof(string))]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateModel request)
        {
            await activityService.Update(id, request);
            return Ok("Activity updated successfully.");
        }

        /// <summary>
        /// Deletes an activity.
        /// </summary>
        /// <param name="id">The activity ID.</param>
        [HttpDelete("{id:Guid}")]
        [SwaggerOperation(Summary = "Delete activity", Description = "Deletes an activity by its ID.")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        [SwaggerResponse(401, "Unauthorized", typeof(string))]
        [SwaggerResponse(404, "Not Found", typeof(string))]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await activityService.Delete(id);
            return Ok("Activity deleted successfully.");
        }
    }

}
