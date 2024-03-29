using AutoMapper;
using FluentValidation;
using Travel.Settings;
using Travel.Context.Entities;

namespace Travel.Services.Activities;

public class UpdateModel
{
    public string Title { get; set; }
    public int PricePerOne { get; set; }
    public int TotalPrice { get; set; }

    public Guid PayerId { get; set; }

}

public class UpdateModelProfile : Profile
{
    public UpdateModelProfile()
    {
        CreateMap<UpdateModel, Activiti>();
    }
}

public class UpdateModelValidator : AbstractValidator<UpdateModel>
{
    public UpdateModelValidator()
    {
        RuleFor(x => x.Title).TripTitle();

    }
}