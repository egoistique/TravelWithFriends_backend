namespace Travel.Context.Entities;

using Microsoft.AspNetCore.Identity;

public class User : IdentityUser<Guid>
{
    public string FullName { get; set; }
    public UserStatus Status { get; set; }
    public virtual ICollection<Trip> CreatedTrips { get; set; }
    public virtual ICollection<Trip> ParticipatedTrips { get; set; }
    public virtual ICollection<Activiti> ParticipatedActivities { get; set; }
    public virtual ICollection<Activiti> PayedActivities { get; set; }
}
