namespace Travel.Api.App;

using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Common.Security;
using Travel.Services.Statistics;
using Travel.Services.Logger;
using Travel.Context.Entities;
using Travel.Services.Trips;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
//[Authorize]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Product")]
[Route("v{version:apiVersion}/[controller]")]
public class StatisticsController : Controller
{
    private readonly IAppLogger logger;
    private readonly IStatisticsService statService;

    public StatisticsController(IAppLogger logger, IStatisticsService statService)
    {
        this.logger = logger;
        this.statService = statService;
    }


    /// <summary>
    /// Gets statistics by ID.
    /// </summary>
    /// <param name="id">The statistics ID.</param>
    /// <returns>The statistics with the specified ID.</returns>
    [HttpGet("{id:Guid}")]
    [SwaggerOperation(Summary = "Get statistics", Description = "Retrieves statistics by its ID.")]
    [SwaggerResponse(200, "Success", typeof(StatisticsModel))]
    [SwaggerResponse(400, "Bad Request", typeof(string))]
    [SwaggerResponse(401, "Unauthorized", typeof(string))]
    [SwaggerResponse(404, "Not Found", typeof(string))]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await statService.GetAll(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}

