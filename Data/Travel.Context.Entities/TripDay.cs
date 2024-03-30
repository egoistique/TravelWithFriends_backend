namespace Travel.Context.Entities;

public class TripDay : BaseEntity
{
    public int? TripId { get; set; }
    public virtual Trip Trip { get; set; }
    public int Number { get; set; }
    public virtual ICollection<Activiti> Activities { get; set; }
}
