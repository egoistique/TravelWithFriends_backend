using AutoMapper;
using FluentValidation;
using Travel.Settings;
using Travel.Context.Entities;

namespace Travel.Services.Trips;

public class UpdateModel
{
    public string Title { get; set; }
    public int NumOfParticipants { get; set; }
    public string DateStart { get; set; }
    public string DateEnd { get; set; }
    public string City { get; set; }
    public string HotelTitle { get; set; }
    public bool IsPublicated { get; set; }

}

public class UpdateModelProfile : Profile
{
    public UpdateModelProfile()
    {
        CreateMap<UpdateModel, Trip>();
    }
}

public class UpdateModelValidator : AbstractValidator<UpdateModel>
{
    public UpdateModelValidator()
    {
        RuleFor(x => x.Title).TripTitle();

    }
}