namespace Travel.Context.Entities;

public class Activiti : BaseEntity
{
    public int? DayId { get; set; }
    public virtual TripDay Day { get; set; }
    public string Title { get; set; }
    public int? CategoryId { get; set; }
    public virtual Category Category { get; set; }
    public virtual ICollection<User> Participants { get; set; }

    public int? PricePerOne { get; set; }
    public int? TotalPrice { get; set; }
    public Guid? PayerId { get; set; }
    public virtual User Payer { get; set; }
}
