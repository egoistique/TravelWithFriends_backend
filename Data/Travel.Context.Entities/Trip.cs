namespace Travel.Context.Entities;

public class Trip : BaseEntity
{
    public Guid? CreatorId { get; set; } //creator
    public virtual User Creator { get; set; }

    public string Title { get; set; }
    public int NumOfParticipants { get; set; }
    public string DateStart { get; set; }
    public string DateEnd { get; set; }
    public string City { get; set; }
    public string HotelTitle { get; set; }
    public virtual ICollection<User> Participants { get; set; }
    public virtual ICollection<Activiti> Activities { get; set; }
}
