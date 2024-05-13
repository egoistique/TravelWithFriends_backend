namespace Travel.Services.Statistics;

public interface IStatisticsService
{
    Task<StatisticsModel> GetAll(Guid tripId);
}
