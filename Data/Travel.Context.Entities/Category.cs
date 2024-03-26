namespace Travel.Context.Entities;

public class Category : BaseEntity
{
    public string Title { get; set; }
    public virtual ICollection<Activiti> Activities { get; set; }
}
