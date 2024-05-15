namespace Travel.Api.App;

using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Common.Security;
using Travel.Services.Trips;
using Travel.Services.Logger;
using Travel.Context.Entities;
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerOperation(Summary = "Get all trips ", Description = "Returns all trips")]
    public async Task<IEnumerable<TripModel>> GetAll()
    {
        var result = await tripService.GetAll();

        return result;
    }

    [HttpGet("{id:Guid}")]
    //[Authorize(AppScopes.TripsRead)]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Get trip by ID", Description = "Returns a trip by its unique identifier.")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await tripService.GetById(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("creator/{creatorId:Guid}")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Get trip by Creator ID", Description = "")]
    public async Task<IEnumerable<TripModel>> GetByCreatorId([FromRoute] Guid creatorId)
    {
        var result = await tripService.GetByCreatorId(creatorId);
        return result;
    }

    [HttpPost("")]
    [Authorize(AppScopes.TripsWrite)]
    //[AllowAnonymous]
    [SwaggerOperation(Summary = "Create a new trip", Description = "Creates a new trip.")]
    [SwaggerResponse(StatusCodes.Status201Created, "Returns the created trip.", typeof(TripModel))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid input parameters.")]
    public async Task<TripModel> Create(CreateModel request)
    {
        var result = await tripService.Create(request);

        return result;
    }

    [HttpPut("{id:Guid}")]
    [Authorize(AppScopes.TripsWrite)]
    [SwaggerOperation(Summary = "Update trip by trip ID", Description = "")]
    public async Task Update([FromRoute] Guid id, UpdateModel request)
    {
        await tripService.Update(id, request);
    }

    [HttpDelete("{id:Guid}")]
    [Authorize(AppScopes.TripsWrite)]
    [SwaggerOperation(Summary = "Delete trip by trip ID", Description = "")]
    public async Task Delete([FromRoute] Guid id)
    {
        await tripService.Delete(id);
    }

    [HttpGet("publicated/")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Get publicated trips", Description = "")]
    public async Task<IEnumerable<PublicatedTripModel>> GetPublicated()
    {
        var result = await tripService.GetPublicated();

        return result;
    }

    [HttpGet("usertrips/{userEmail}")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Get the trips in which the user is a member", Description = "User as a participant or creator")]
    public async Task<IEnumerable<TripModel>> GetUsersTrips(string userEmail)
    {
        var result = await tripService.GetUsersTrips(userEmail);

        return result;
    }

    [HttpPost("usertrips/{tripId}/addparticipant")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Add a participant to the trip", Description = "")]
    public async Task<ActionResult<TripModel>> AddUserToTrip(Guid tripId, [FromBody] string userEmail)
    {
        var result = await tripService.AddTripParticipants(tripId, userEmail);

        if (result == null)
        {
            return NotFound(); 
        }

        return Ok(result); 
    }

    [HttpGet("getdays/{id:Guid}")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Get trip days by trip ID", Description = "")]
    public async Task<IActionResult> GetTripDays([FromRoute] Guid id)
    {
        var result = await tripService.GetTripDays(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}
