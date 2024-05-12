using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Travel.Common.Exceptions;
using Travel.Common.Validator;
using Travel.Common.Limits;
using Travel.Context;
using Travel.Context.Entities;
using static Travel.Common.Limits.Limits;

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
            .Include(x => x.Day)
            .Include(x => x.Category)
            .Include(x => x.Payers)
            .Include(x => x.Participants)
            .ToListAsync();

        var result = mapper.Map<IEnumerable<ActivityModel>>(activities);

        return result;
    }

    public async Task<ActivityModel> GetById(Guid id)
    {
        using var context = await dbContextFactory.CreateDbContextAsync();

        var activity = await context.Activities
            .Include(x => x.Day)
            .Include(x => x.Category)
            .Include(x => x.Payers)
            .Include(x => x.Participants)
            .FirstOrDefaultAsync(x => x.Uid == id);

        var result = mapper.Map<ActivityModel>(activity);

        return result;
    }

    public async Task<IEnumerable<ActivityModel>> GetByDayId(Guid dayId)
    {
        using var context = await dbContextFactory.CreateDbContextAsync();

        var activities = await context.Activities
            .Include(x => x.Day)
            .Include(x => x.Category)
            .Include(x => x.Payers)
            .Include(x => x.Participants)
            .Where(x => x.Day.Uid == dayId)
            .ToListAsync();

        var result = mapper.Map<IEnumerable<ActivityModel>>(activities);

        return result;
    }
    public async Task<Guid> GetUserIdByEmail(string email)
    {
        using var context = await dbContextFactory.CreateDbContextAsync();

        var user = await context.Users
            .FirstOrDefaultAsync(x => x.Email == email);

        if (user != null)
        {
            Console.WriteLine($"User with email '{email}' found. UserId: {user.Id}");
        }
        else
        {
            Console.WriteLine($"User with email '{email}' not found.");
        }
        return user.Id;
    }

    public async Task<ActivityModel> Create(CreateModel model)
    {
        await createModelValidator.CheckAsync(model);

        using var context = await dbContextFactory.CreateDbContextAsync();

        var day = await context.Days.FirstOrDefaultAsync(d => d.Uid == model.DayId);
        if (day == null)
        {
            throw new ProcessException($"Day with ID {model.DayId} does not exist.");
        }

        if (day.Trip == null)
        {
            throw new ProcessException($"Trip does not exist for the day with ID {model.DayId}.");
        }

        var creatorStatus = day.Trip.Creator.Status;

        var activitiesCountForDay = day.Activities.Count();

        if (model.FromSearch)
        {
            if (activitiesCountForDay >= (int)SearchLimit.MaxActivitiesPerDay)
            {
                throw new ProcessException($"The limit of {SearchLimit.MaxActivitiesPerDay} activities with FromSearch = true for this day has been reached.");
            }
        }
        else
        {
            int maxActivitiesPerDay = creatorStatus == 0 ? (int)NonSearchLimit.MaxActsPerDayStatus0 : (int)NonSearchLimit.MaxActsPerDayStatus2;
            if (activitiesCountForDay >= maxActivitiesPerDay)
            {
                throw new ProcessException($"The limit of {maxActivitiesPerDay} activities with FromSearch = false for this day has been reached for creator with status {creatorStatus}.");
            }
        }

        var activity = mapper.Map<Activiti>(model);
        activity.Participants = new List<User>();
        activity.Payers = new List<User>();

        await context.Activities.AddAsync(activity);
       // await context.SaveChangesAsync();

        IEnumerable<string> emailsPayers = model.Payers;
        IEnumerable<string> emailsParticipants = model.Participants;

        foreach (var email in emailsPayers)
        {
            var userId = await GetUserIdByEmail(email);
            var user = await context.Users.FindAsync(userId);
            activity.Payers.Add(user);
        }

        foreach (var email in emailsParticipants)
        {
            var userId = await GetUserIdByEmail(email);
            var user = await context.Users.FindAsync(userId);
            activity.Participants.Add(user);
        }

        await context.SaveChangesAsync();

        return mapper.Map<ActivityModel>(activity);
    }

    public async Task Update(Guid id, UpdateModel model)
    {
        await updateModelValidator.CheckAsync(model);

        using var context = await dbContextFactory.CreateDbContextAsync();

        var activity = await context.Activities.Where(x => x.Uid == id).FirstOrDefaultAsync();

        activity = mapper.Map(model, activity);

        activity.Payers.Clear();
        activity.Participants.Clear();

        IEnumerable<string> emailsPayers = model.Payers;
        IEnumerable<string> emailsParticipants = model.Participants;

        foreach (var email in emailsPayers)
        {
            var userId = await GetUserIdByEmail(email);
            var user = await context.Users.FindAsync(userId);
            activity.Payers.Add(user);
        }

        foreach (var email in emailsParticipants)
        {
            var userId = await GetUserIdByEmail(email);
            var user = await context.Users.FindAsync(userId);
            activity.Participants.Add(user);
        }



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
