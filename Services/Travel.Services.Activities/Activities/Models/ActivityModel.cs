using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Travel.Context.Entities;
using Travel.Context;

namespace Travel.Services.Activities;

public class ActivityModel
{
    public Guid Id { get; set; }
    public Guid DayId { get; set; }
    public int DayNumber { get; set; }
    public string Title { get; set; }
    public bool FromSearch { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryTitle { get; set; }
    public int PricePerOne { get; set; }
    public int TotalPrice { get; set; }
    public IEnumerable<string>? Payers { get; set; }
    public IEnumerable<string>? Participants { get; set; }
}

public class ActivityModelProfile : Profile
{
    public ActivityModelProfile()
    {
        CreateMap<Activiti, ActivityModel>()
            .BeforeMap<ActivityModelActions>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DayId, opt => opt.Ignore())
            .ForMember(dest => dest.DayNumber, opt => opt.Ignore()) 
            .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
            .ForMember(dest => dest.CategoryTitle, opt => opt.Ignore())
            .ForMember(dest => dest.Payers, opt => opt.Ignore())
            .ForMember(dest => dest.Participants, opt => opt.Ignore())
            ;
    }
    public class ActivityModelActions : IMappingAction<Activiti, ActivityModel>
    {
        private readonly IDbContextFactory<MainDbContext> contextFactory;

        public ActivityModelActions(IDbContextFactory<MainDbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Process(Activiti source, ActivityModel destination, ResolutionContext context)
        {
            using var db = contextFactory.CreateDbContext();

            var activity = db.Activities
                .Include(x => x.Day)
                .Include(x => x.Category)
                .FirstOrDefault(x => x.Id == source.Id);


            destination.Id = activity.Uid;
            destination.DayId = activity.Day.Uid;
            destination.DayNumber = activity.Day.Number;           
            destination.CategoryId = activity.Category.Uid;
            destination.CategoryTitle = activity.Category.Title;

            destination.Payers = activity.Payers?.Select(x => x.Email);
            destination.Participants = activity.Participants?.Select(x => x.Email);
        }
    }
}

