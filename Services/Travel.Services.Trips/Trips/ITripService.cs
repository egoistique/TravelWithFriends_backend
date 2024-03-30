﻿namespace Travel.Services.Trips;

public interface ITripService
{
    Task<IEnumerable<TripModel>> GetAll();
    Task<TripModel> GetById(Guid id);
    Task<IEnumerable<TripModel>> GetByCreatorId(Guid creatorId);
    Task<TripModel> Create(CreateModel model);
    Task Update(Guid id, UpdateModel model);
    Task Delete(Guid id);
}