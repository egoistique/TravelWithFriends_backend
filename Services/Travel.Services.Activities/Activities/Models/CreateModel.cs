using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Travel.Context.Entities;
using Travel.Context;
using Travel.Settings;
using FluentValidation;

namespace Travel.Services.Activities;

public class CreateModel
{
    public Guid DayId { get; set; }
    public string Title { get; set; }
    public bool FromSearch { get; set; }
    public Guid CategoryId { get; set; }
    public int PricePerOne { get; set; }
    public int TotalPrice { get; set; }
    public IEnumerable<string> Participants { get; set; } // список почт участников
    public IEnumerable<string> Payers { get; set; } // список почт плательщиков
}


public class CreateModelProfile : Profile
{
    public CreateModelProfile()
    {
        CreateMap<CreateModel, Activiti>()
            .ForMember(dest => dest.DayId, opt => opt.Ignore())
            .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
            .ForMember(dest => dest.Participants, opt => opt.Ignore())
            .ForMember(dest => dest.Payers, opt => opt.Ignore())
            .AfterMap<CreateModelActions>();
    }

    public class CreateModelActions : IMappingAction<CreateModel, Activiti>
    {
        private readonly IDbContextFactory<MainDbContext> contextFactory;

        public CreateModelActions(IDbContextFactory<MainDbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Process(CreateModel source, Activiti destination, ResolutionContext context)
        {
            using var db = contextFactory.CreateDbContext();

            var day = db.Days.FirstOrDefault(x => x.Uid == source.DayId);

            destination.DayId = day.Id;           
            
            var category = db.Categories.FirstOrDefault(x => x.Uid == source.CategoryId);

            destination.CategoryId = category.Id;                     
        }     
    }
}

public class CreateBookModelValidator : AbstractValidator<CreateModel>
{
    public CreateBookModelValidator(IDbContextFactory<MainDbContext> contextFactory)
    {
        RuleFor(x => x.Title).TripTitle();

        RuleFor(x => x.DayId)
            .NotEmpty().WithMessage("Day is required")
            .Must((id) =>
            {
                using var context = contextFactory.CreateDbContext();
                var found = context.Days.Any(a => a.Uid == id);
                return found;
            }).WithMessage("Day not found"); 
        
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required")
            .Must((id) =>
            {
                using var context = contextFactory.CreateDbContext();
                var found = context.Categories.Any(a => a.Uid == id);
                return found;
            }).WithMessage("Category not found");

        RuleFor(x => x.Participants)
           .Must((emails) =>
           {
               using var context = contextFactory.CreateDbContext();
               return emails.All(email => context.Users.Any(u => u.Email == email));
           }).WithMessage("One or more participants not found");

        RuleFor(x => x.Payers)
            .Must((emails) =>
            {
                using var context = contextFactory.CreateDbContext();
                return emails.All(email => context.Users.Any(u => u.Email == email));
            }).WithMessage("One or more payers not found");

        RuleFor(x => x.Title)
            .MaximumLength(1000).WithMessage("Maximum length is 100");
    }
}