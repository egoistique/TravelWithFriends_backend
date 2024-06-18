using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Travel.Services.Activities;

public class ActivityModelExample : IExamplesProvider<ActivityModel>
{
    public ActivityModel GetExamples()
    {
        return new ActivityModel
        {
            Id = Guid.NewGuid(),
            DayId = Guid.NewGuid(),
            DayNumber = 1,
            Title = "Sample Activity",
            FromSearch = false,
            CategoryId = Guid.NewGuid(),
            CategoryTitle = "Sample Category",
            PricePerOne = 100,
            TotalPrice = 200,
            Payers = new List<string> { "participant1@example.com", "participant2@example.com" },
            Participants = new List<string> { "participant1@example.com", "participant2@example.com", "participant3@example.com" }
        };
    }
}
