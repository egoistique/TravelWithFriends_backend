using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Travel.Common.Exceptions;
using Travel.Common.Validator;
using Travel.Context;
using Travel.Context.Entities;

namespace Travel.Services.Activities;

public class ActivityService : IActivityService
{
    private readonly IDbContextFactory<MainDbContext> dbContextFactory;
    private readonly IMapper mapper;
    private readonly IModelValidator<CreateModel> createModelValidator;
    private readonly IModelValidator<UpdateModel> updateModelValidator;

    public ActivityService(IDbContextFactory<MainDbContext> dbContextFactory,
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

    public async Task<IEnumerable<ActivityModel>> GetAll()
    {
        using var context = await dbContextFactory.CreateDbContextAsync();

        var activities = await context.Activities
            .Include(x => x.Trip)
            .Include(x => x.Category)
            .Include(x => x.Payer)
            .Include(x => x.Participants)
            .ToListAsync();

        var result = mapper.Map<IEnumerable<ActivityModel>>(activities);

        return result;
    }

    public async Task<ActivityModel> GetById(Guid id)
    {
        using var context = await dbContextFactory.CreateDbContextAsync();

        var activity = await context.Activities
            .Include(x => x.Trip)
            .Include(x => x.Category)
            .Include(x => x.Payer)
            .Include(x => x.Participants)
            .FirstOrDefaultAsync(x => x.Uid == id);

        var result = mapper.Map<ActivityModel>(activity);

        return result;
    }

    public async Task<IEnumerable<ActivityModel>> GetByTripId(Guid tripId)
    {
        using var context = await dbContextFactory.CreateDbContextAsync();

        var activities = await context.Activities
            .Include(x => x.Trip)
            .Include(x => x.Category)
            .Include(x => x.Payer)
            .Include(x => x.Participants)
            .Where(x => x.Trip.Uid == tripId)
            .ToListAsync();

        var result = mapper.Map<IEnumerable<ActivityModel>>(activities);

        return result;
    }


    public async Task<ActivityModel> Create(CreateModel model)
    {
        await createModelValidator.CheckAsync(model);

        using var context = await dbContextFactory.CreateDbContextAsync();

        var activity = mapper.Map<Activiti>(model);

        await context.Activities.AddAsync(activity);

        await context.SaveChangesAsync();

        return mapper.Map<ActivityModel>(activity);
    }

    public async Task Update(Guid id, UpdateModel model)
    {
        await updateModelValidator.CheckAsync(model);

        using var context = await dbContextFactory.CreateDbContextAsync();

        var activity = await context.Activities.Where(x => x.Uid == id).FirstOrDefaultAsync();

        activity = mapper.Map(model, activity);

        context.Activities.Update(activity);

        await context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        using var context = await dbContextFactory.CreateDbContextAsync();

        var activity = await context.Activities.Where(x => x.Uid == id).FirstOrDefaultAsync();

        if (activity == null)
            throw new ProcessException($"Activity (ID = {id}) not found.");

        context.Activities.Remove(activity);

        await context.SaveChangesAsync();
    }
}
