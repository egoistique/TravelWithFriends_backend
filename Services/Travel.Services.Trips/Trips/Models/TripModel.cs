using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Travel.Context.Entities;
using Travel.Context;
using System.Drawing;

namespace Travel.Services.Trips;

public class TripModel
{
    public Guid Id { get; set; }

    public Guid CreatorId { get; set; }
    public string CreatorName { get; set; }

    public string Title { get; set; }
    public int NumOfParticipants { get; set; }
    public string DateStart { get; set; }
    public string DateEnd { get; set; }
    public string City { get; set; }
    public string HotelTitle { get; set; }
    public bool IsPublicated { get; set; }

    public IEnumerable<string> Participants { get; set; }
    public IEnumerable<string> Days { get; set; }

    public string ErrorMessage { get; set; } // Добавляем поле для сообщения об ошибке
}


public class TripModelProfile : Profile
{
    public TripModelProfile()
    {
        CreateMap<Trip, TripModel>()
            .BeforeMap<TripModelActions>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatorId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatorName, opt => opt.Ignore())
            .ForMember(dest => dest.Participants, opt => opt.Ignore())
            .ForMember(dest => dest.Days, opt => opt.Ignore())
            ;
    }
    public class TripModelActions : IMappingAction<Trip, TripModel>
    {
        private readonly IDbContextFactory<MainDbContext> contextFactory;

        public TripModelActions(IDbContextFactory<MainDbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Process(Trip source, TripModel destination, ResolutionContext context)
        {
            using var db = contextFactory.CreateDbContext();

            var trip = db.Trips
                      .Include(x => x.Creator)
                      .Include(x => x.Days)  // Включаем дни поездки
                      .ThenInclude(d => d.Activities)  // Включаем активности для каждого дня
                      .FirstOrDefault(x => x.Id == source.Id);

            destination.Id = trip.Uid;
            destination.CreatorId = trip.Creator.Id;
            destination.CreatorName = trip.Creator.FullName;

            destination.Participants = trip.Participants?.Select(x => x.Email);

            destination.Days = trip.Days.Select(day =>
            {
                var activities = string.Join(", ", day.Activities.Select(a => a.Title));
                return $"Day {day.Number} - {activities}";
            });
        }
    }
}
