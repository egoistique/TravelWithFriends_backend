﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Globalization;
using Travel.Common.Exceptions;
using Travel.Common.Limits;
using Travel.Common.Validator;
using Travel.Context;
using Travel.Context.Entities;

namespace Travel.Services.Trips;

public class TripService : ITripService
{
    private readonly IDbContextFactory<MainDbContext> dbContextFactory;
    private readonly IMapper mapper;
    private readonly IModelValidator<CreateModel> createModelValidator;
    private readonly IModelValidator<UpdateModel> updateModelValidator;

    public TripService(IDbContextFactory<MainDbContext> dbContextFactory,
        IMapper mapper,
        IModelValidator<CreateModel> createModelValidator,
        IModelValidator<UpdateModel> updateModelValidator
        )
    {
        this.dbContextFactory = dbContextFactory;
        this.mapper = mapper;
        this.createModelValidator = createModelValidator;
        this.updateModelValidator = updateModelValidator;
    }

    public async Task<IEnumerable<TripModel>> GetAll()
    {
        using var context = await dbContextFactory.CreateDbContextAsync();

        var trips = await context.Trips
            .Include(x => x.Creator)
            .Include(x => x.Participants)
            .Include(x => x.Days)
            .ThenInclude(d => d.Activities)
            .ToListAsync();

        var result = mapper.Map<IEnumerable<TripModel>>(trips);

        return result;
    }

    public async Task<TripModel> GetById(Guid id)
    {
        using var context = await dbContextFactory.CreateDbContextAsync();

        var trip = await context.Trips
            .Include(x => x.Creator)
            .Include(x => x.Participants)
            .Include(x => x.Days)
            .ThenInclude(d => d.Activities)
            .FirstOrDefaultAsync(x => x.Uid == id);

        var result = mapper.Map<TripModel>(trip);

        return result;
    }

    public async Task<IEnumerable<TripModel>> GetByCreatorId(Guid creatorId)
    {
        using var context = await dbContextFactory.CreateDbContextAsync();

        var trips = await context.Trips
            .Include(x => x.Creator)
            .Include(x => x.Participants)
            .Include(x => x.Days)
            .ThenInclude(d => d.Activities)
            .Where(x => x.Creator.Id == creatorId)
            .ToListAsync();

        var result = mapper.Map<IEnumerable<TripModel>>(trips);

        return result;
    }


    public async Task<TripModel> Create(CreateModel model)
    {
        await createModelValidator.CheckAsync(model);

        using var context = await dbContextFactory.CreateDbContextAsync();

        var creator = await context.Users
            .Include(u => u.CreatedTrips)
            .FirstOrDefaultAsync(u => u.Id == model.CreatorId);
        if (creator == null)
        {
            throw new ProcessException($"User with ID {model.CreatorId} does not exist.");
        }

        // Получаем текущее количество созданных путешествий у участника
        int currentTripsCount = creator.CreatedTrips.Count;

        // Получаем лимит для текущего статуса создателя путешествия
        int maxTripsLimit = creator.Status == 0 ?
            (int)Limits.ParticipantsLimit.MaxParticipantsPerCreatorStatus0 :
            (int)Limits.ParticipantsLimit.MaxParticipantsPerCreatorStatus2;

        Console.WriteLine($"Current trips count for user {creator.Id}: {currentTripsCount}");
        Console.WriteLine($"Max trips limit for user {creator.Id}: {maxTripsLimit}");

        // Проверяем, не превышает ли текущее количество путешествий у участника лимит
        if (currentTripsCount >= maxTripsLimit)
        {
            throw new TripLimitExceededException();
        }

        var trip = mapper.Map<Trip>(model);

        DateTime startDate = DateTime.ParseExact(model.DateStart, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        DateTime endDate = DateTime.ParseExact(model.DateEnd, "dd.MM.yyyy", CultureInfo.InvariantCulture);

        List<TripDay> tripDays = new List<TripDay>();
        for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        {
            tripDays.Add(new TripDay
            {
                Number = (date - startDate).Days + 1,                
            });
        }

        trip.Days = tripDays;

        await context.Trips.AddAsync(trip);
        await context.SaveChangesAsync();

        return mapper.Map<TripModel>(trip);
    }

    public async Task Update(Guid id, UpdateModel model)
    {
        await updateModelValidator.CheckAsync(model);

        using var context = await dbContextFactory.CreateDbContextAsync();

        var trip = await context.Trips.Where(x => x.Uid == id).FirstOrDefaultAsync();

        trip = mapper.Map(model, trip);

        context.Trips.Update(trip);

        await context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        using var context = await dbContextFactory.CreateDbContextAsync();

        var trip = await context.Trips.Where(x => x.Uid == id).FirstOrDefaultAsync();

        if (trip == null)
            throw new ProcessException($"Trip (ID = {id}) not found.");

        context.Trips.Remove(trip);

        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<PublicatedTripModel>> GetPublicated()
    {
        using var context = await dbContextFactory.CreateDbContextAsync();

        var trips = await context.Trips
            .Include(x => x.Creator)
            .Include(x => x.Days)
            .ThenInclude(d => d.Activities)
            .ToListAsync();

        var result = mapper.Map<IEnumerable<PublicatedTripModel>>(trips);

        return result;
    }

}
