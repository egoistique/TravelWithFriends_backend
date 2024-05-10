namespace Travel.Api.App;

using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Common.Security;
using Travel.Services.Trips;
using Travel.Services.Logger;

[ApiController]
[Authorize]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Product")]
[Route("v{version:apiVersion}/[controller]")]
public class TripController : ControllerBase
{
    private readonly IAppLogger logger;
    private readonly ITripService tripService;

    public TripController(IAppLogger logger, ITripService tripService)
    {
        this.logger = logger;
        this.tripService = tripService;
    }

    [HttpGet("")]
    // [Authorize(AppScopes.TripsRead)]
    [AllowAnonymous]
    public async Task<IEnumerable<TripModel>> GetAll()
    {
        var result = await tripService.GetAll();

        return result;
    }

    [HttpGet("{id:Guid}")]
    [Authorize(AppScopes.TripsRead)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await tripService.GetById(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("creator/{creatorId:Guid}")]
    [AllowAnonymous]
    public async Task<IEnumerable<TripModel>> GetByCreatorId([FromRoute] Guid creatorId)
    {
        var result = await tripService.GetByCreatorId(creatorId);
        return result;
    }

    [HttpPost("")]
    //[Authorize(AppScopes.TripsWrite)]
    [AllowAnonymous]
    public async Task<TripModel> Create(CreateModel request)
    {
        var result = await tripService.Create(request);

        return result;
    }

    [HttpPut("{id:Guid}")]
    [Authorize(AppScopes.TripsWrite)]
    public async Task Update([FromRoute] Guid id, UpdateModel request)
    {
        await tripService.Update(id, request);
    }

    [HttpDelete("{id:Guid}")]
    [Authorize(AppScopes.TripsWrite)]
    public async Task Delete([FromRoute] Guid id)
    {
        await tripService.Delete(id);
    }

    [HttpGet("publicated/")]
    [AllowAnonymous]
    public async Task<IEnumerable<PublicatedTripModel>> GetPublicated()
    {
        var result = await tripService.GetPublicated();

        return result;
    }

    [HttpGet("usertrips/{userEmail}")]
    [AllowAnonymous]
    public async Task<IEnumerable<TripModel>> GetUsersTrips(string userEmail)
    {
        var result = await tripService.GetUsersTrips(userEmail);

        return result;
    }

}
