namespace Travel.Services.Activities;

public interface IActivityService
{
    Task<IEnumerable<ActivityModel>> GetAll();
    Task<ActivityModel> GetById(Guid id);
    Task<IEnumerable<ActivityModel>> GetByDayId(Guid tripId);
    Task<ActivityModel> Create(CreateModel model);
    Task Update(Guid id, UpdateModel model);
    Task Delete(Guid id);
}
