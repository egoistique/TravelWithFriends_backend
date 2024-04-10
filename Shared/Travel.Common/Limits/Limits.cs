namespace Travel.Common.Limits;

public class Limits
{
    public enum SearchLimit
    {
        MaxActivitiesPerDay = 5
    }

    public enum NonSearchLimit
    {
        MaxActsPerDayStatus0 = 15,
        MaxActsPerDayStatus2 = 20
    }

    public enum TripsLimit
    {
        MaxTripsPerUserStatus0 = 5,
        MaxTripsPerUserStatus2 = 15
    }

    public enum ParticipantsLimit
    {
        MaxParticipantsPerCreatorStatus0 = 7,
        MaxParticipantsPerCreatorStatus2 = 15
    }
}
