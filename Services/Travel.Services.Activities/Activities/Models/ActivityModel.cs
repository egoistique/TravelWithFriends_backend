using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Travel.Context.Entities;
using Travel.Context;

namespace Travel.Services.Activities;

public class ActivityModel
{
    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    public string TripTitle { get; set; }
    public string Title { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryTitle { get; set; }
    public int PricePerOne { get; set; }
    public int TotalPrice { get; set; }
    public Guid PayerId { get; set; }
    public string PayerName { get; set; }
    public IEnumerable<string> Participants { get; set; }
}

public class ActivityModelProfile : Profile
{
    public ActivityModelProfile()
    {
        CreateMap<Activiti, ActivityModel>()
            .BeforeMap<ActivityModelActions>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.TripId, opt => opt.Ignore())
            .ForMember(dest => dest.TripTitle, opt => opt.Ignore()) 
            .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
            .ForMember(dest => dest.CategoryTitle, opt => opt.Ignore())
            .ForMember(dest => dest.PayerId, opt => opt.Ignore())
            .ForMember(dest => dest.PayerName, opt => opt.Ignore())
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
                .Include(x => x.Trip)
                .Include(x => x.Category)
                .Include(x => x.Payer)
                .FirstOrDefault(x => x.Id == source.Id);


            destination.Id = activity.Uid;
            destination.TripId = activity.Trip.Uid;
            destination.TripTitle = activity.Trip.Title;           
            destination.CategoryId = activity.Category.Uid;
            destination.CategoryTitle = activity.Category.Title;  
            destination.PayerId = activity.Payer.Id;
            destination.PayerName = activity.Payer.FullName;

            destination.Participants = activity.Participants?.Select(x => x.FullName);
        }
    }
}

