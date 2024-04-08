using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Travel.Context.Entities;
using Travel.Context;

namespace Travel.Services.Trips;

public class PublicatedTripModel
{
    public Guid Id { get; set; }
    public string CreatorName { get; set; }

    public string Title { get; set; }
    public string DateStart { get; set; }
    public string DateEnd { get; set; }
    public string City { get; set; }
    public bool IsPublicated { get; set; }
    public IEnumerable<string> Days { get; set; }
}
public class PublicatedTripModelProfile : Profile
{
    public PublicatedTripModelProfile()
    {
        CreateMap<Trip, PublicatedTripModel>()
            .BeforeMap<TripModelActions>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatorName, opt => opt.Ignore())
            .ForMember(dest => dest.Days, opt => opt.Ignore())
            ;
    }
    public class TripModelActions : IMappingAction<Trip, PublicatedTripModel>
    {
        private readonly IDbContextFactory<MainDbContext> contextFactory;

        public TripModelActions(IDbContextFactory<MainDbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Process(Trip source, PublicatedTripModel destination, ResolutionContext context)
        {
            using var db = contextFactory.CreateDbContext();

            var trip = db.Trips
                      .Include(x => x.Creator)
                      .Include(x => x.Days)  
                      .ThenInclude(d => d.Activities)  
                      .FirstOrDefault(x => x.Id == source.Id);

            destination.Id = trip.Uid;
            destination.CreatorName = trip.Creator.FullName;

            destination.Days = trip.Days.Select(day =>
            {
                var activities = string.Join(", ", day.Activities.Where(a => a.FromSearch).Select(a => a.Title));
                return $"Day {day.Number} - {activities}";
            });
        }
    }
}
