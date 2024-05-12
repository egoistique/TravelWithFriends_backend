namespace Travel.Api.App;

using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Common.Security;
using Travel.Services.Statistics;
using Travel.Services.Logger;
using Travel.Context.Entities;
using Travel.Services.Trips;

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


    [HttpGet("{id:Guid}")]    
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await statService.GetAll(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}

