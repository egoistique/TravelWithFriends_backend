namespace Travel.Context.Entities;

public class UserActivity
{
    public int UserId { get; set; }
    public User User { get; set; }

    public int ActivityId { get; set; }
    public Activiti Activiti { get; set; }
}
