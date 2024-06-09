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

    /// <summary>
    /// Gets all trips.
    /// </summary>
    /// <returns>A list of all trips.</returns>
    [HttpGet("")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Get all trips", Description = "Returns all trips.")]
    [SwaggerResponse(200, "Success", typeof(IEnumerable<TripModel>))]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IEnumerable<TripModel>> GetAll()
    {
        var result = await tripService.GetAll();

        return result;
    }

    /// <summary>
    /// Gets a trip by ID.
    /// </summary>
    /// <param name="id">The trip ID.</param>
    /// <returns>The trip with the specified ID.</returns>
    [HttpGet("{id:Guid}")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Get trip by ID", Description = "Returns a trip by its unique identifier.")]
    [SwaggerResponse(200, "Success", typeof(TripModel))]
    [SwaggerResponse(404, "Not Found")]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await tripService.GetById(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Gets trips by creator ID.
    /// </summary>
    /// <param name="creatorId">The creator ID.</param>
    /// <returns>A list of trips created by the specified user.</returns>
    [HttpGet("creator/{creatorId:Guid}")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Get trips by Creator ID", Description = "Returns trips by the creator's unique identifier.")]
    [SwaggerResponse(200, "Success", typeof(IEnumerable<TripModel>))]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(404, "Not Found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IEnumerable<TripModel>> GetByCreatorId([FromRoute] Guid creatorId)
    {
        var result = await tripService.GetByCreatorId(creatorId);
        return result;
    }

    /// <summary>
    /// Creates a new trip.
    /// </summary>
    /// <param name="request">The create model request.</param>
    /// <returns>The created trip.</returns>
    [HttpPost("")]
    [Authorize(AppScopes.TripsWrite)]
    [SwaggerOperation(Summary = "Create a new trip", Description = "Creates a new trip.")]
    [SwaggerResponse(201, "Returns the created trip.", typeof(TripModel))]
    [SwaggerResponse(400, "Invalid input parameters.")]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<TripModel> Create(CreateModel request)
    {
        var result = await tripService.Create(request);

        return result;
    }

    /// <summary>
    /// Updates an existing trip.
    /// </summary>
    /// <param name="id">The trip ID.</param>
    /// <param name="request">The update model request.</param>
    [HttpPut("{id:Guid}")]
    [Authorize(AppScopes.TripsWrite)]
    [SwaggerOperation(Summary = "Update trip by trip ID", Description = "Updates an existing trip by its unique identifier.")]
    [SwaggerResponse(200, "Success")]
    [SwaggerResponse(400, "Invalid input parameters.")]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(404, "Not Found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task Update([FromRoute] Guid id, UpdateModel request)
    {
        await tripService.Update(id, request);
    }

    /// <summary>
    /// Deletes a trip.
    /// </summary>
    /// <param name="id">The trip ID.</param>
    [HttpDelete("{id:Guid}")]
    [Authorize(AppScopes.TripsWrite)]
    [SwaggerOperation(Summary = "Delete trip by trip ID", Description = "Deletes a trip by its unique identifier.")]
    [SwaggerResponse(200, "Success")]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(404, "Not Found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task Delete([FromRoute] Guid id)
    {
        await tripService.Delete(id);
    }

    /// <summary>
    /// Gets publicated trips.
    /// </summary>
    /// <returns>A list of publicated trips.</returns>
    [HttpGet("publicated/")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Get publicated trips", Description = "Returns all publicated trips.")]
    [SwaggerResponse(200, "Success", typeof(IEnumerable<PublicatedTripModel>))]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(404, "Not Found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IEnumerable<PublicatedTripModel>> GetPublicated()
    {
        var result = await tripService.GetPublicated();

        return result;
    }

    /// <summary>
    /// Gets the trips in which the user is a member.
    /// </summary>
    /// <param name="userEmail">The user's email.</param>
    /// <returns>A list of trips where the user is a participant or creator.</returns>
    [HttpGet("usertrips/{userEmail}")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Get the trips in which the user is a member", Description = "Returns trips where the user is a participant or creator.")]
    [SwaggerResponse(200, "Success", typeof(IEnumerable<TripModel>))]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(404, "Not Found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IEnumerable<TripModel>> GetUsersTrips(string userEmail)
    {
        var result = await tripService.GetUsersTrips(userEmail);

        return result;
    }

    /// <summary>
    /// Adds a participant to the trip.
    /// </summary>
    /// <param name="tripId">The trip ID.</param>
    /// <param name="userEmail">The email of the user to add as a participant.</param>
    /// <returns>The updated trip.</returns>
    [HttpPost("usertrips/{tripId}/addparticipant")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Add a participant to the trip", Description = "Adds a participant to the trip.")]
    [SwaggerResponse(200, "Success", typeof(TripModel))]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(404, "Not Found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<TripModel>> AddUserToTrip(Guid tripId, [FromBody] string userEmail)
    {
        var result = await tripService.AddTripParticipants(tripId, userEmail);

        if (result == null)
        {
            return NotFound(); 
        }

        return Ok(result); 
    }

    /// <summary>
    /// Gets trip days by trip ID.
    /// </summary>
    /// <param name="id">The trip ID.</param>
    /// <returns>The days of the trip with the specified ID.</returns>
    [HttpGet("getdays/{id:Guid}")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Get trip days by trip ID", Description = "Returns the days of the trip by its unique identifier.")]
    [SwaggerResponse(200, "Success")]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(404, "Not Found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> GetTripDays([FromRoute] Guid id)
    {
        var result = await tripService.GetTripDays(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}
