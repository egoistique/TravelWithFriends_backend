using AutoMapper;
using FluentValidation;
using Travel.Settings;
using Travel.Context.Entities;
using Microsoft.EntityFrameworkCore;
using Travel.Context;

namespace Travel.Services.Activities;

public class UpdateModel
{
    public string Title { get; set; }
    public Guid CategoryId { get; set; }
    public int PricePerOne { get; set; }
    public int TotalPrice { get; set; }
    public IEnumerable<string> Participants { get; set; } // список почт участников
    public IEnumerable<string> Payers { get; set; } // список почт плательщиков

}

public class UpdateModelProfile : Profile
{
    public UpdateModelProfile()
    {
        CreateMap<UpdateModel, Activiti>()
            .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
            .ForMember(dest => dest.Participants, opt => opt.Ignore())
            .ForMember(dest => dest.Payers, opt => opt.Ignore())
            .AfterMap<UpdateModelActions>();
    }

    public class UpdateModelActions : IMappingAction<UpdateModel, Activiti>
    {
        private readonly IDbContextFactory<MainDbContext> contextFactory;

        public UpdateModelActions(IDbContextFactory<MainDbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Process(UpdateModel source, Activiti destination, ResolutionContext context)
        {
            using var db = contextFactory.CreateDbContext();

            var category = db.Categories.FirstOrDefault(x => x.Uid == source.CategoryId);

            destination.CategoryId = category.Id;
        }
    }
}

public class UpdateModelValidator : AbstractValidator<UpdateModel>
{
    public UpdateModelValidator()
    {
        RuleFor(x => x.Title).TripTitle();

    }
}