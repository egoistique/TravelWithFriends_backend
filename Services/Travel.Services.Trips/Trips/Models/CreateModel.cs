using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Travel.Context.Entities;
using Travel.Context;
using Travel.Settings;
using FluentValidation;

namespace Travel.Services.Trips;

public class CreateModel
{
    public Guid CreatorId { get; set; }
    public string Title { get; set; }
    public int NumOfParticipants { get; set; }
    public string DateStart { get; set; }
    public string DateEnd { get; set; }
    public string City { get; set; }
    public string HotelTitle { get; set; }
}

public class CreateModelProfile : Profile
{
    public CreateModelProfile()
    {
        CreateMap<CreateModel, Trip>()
            .ForMember(dest => dest.CreatorId, opt => opt.Ignore())
            .AfterMap<CreateModelActions>();
    }

    public class CreateModelActions : IMappingAction<CreateModel, Trip>
    {
        private readonly IDbContextFactory<MainDbContext> contextFactory;

        public CreateModelActions(IDbContextFactory<MainDbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Process(CreateModel source, Trip destination, ResolutionContext context)
        {
            using var db = contextFactory.CreateDbContext();

            var creator = db.Users.FirstOrDefault(x => x.Id == source.CreatorId);

            destination.CreatorId = creator.Id;
        }
    }
}

public class CreateBookModelValidator : AbstractValidator<CreateModel>
{
    public CreateBookModelValidator(IDbContextFactory<MainDbContext> contextFactory)
    {
        RuleFor(x => x.Title).TripTitle();

        RuleFor(x => x.CreatorId)
            .NotEmpty().WithMessage("Creator is required")
            .Must((id) =>
            {
                using var context = contextFactory.CreateDbContext();
                var found = context.Users.Any(a => a.Id == id);
                return found;
            }).WithMessage("Creator not found");

        RuleFor(x => x.Title)
            .MaximumLength(1000).WithMessage("Maximum length is 100");
    }
}