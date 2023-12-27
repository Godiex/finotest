using Domain.Enums;

namespace Domain.Entities;

public class Item : EntityBase<Guid>, IAggregateRoot
{
    public int Index { get; set; }
    public BehaviorType Behavior { get; set; }
    public List<string>? Multimedia { get; set; }
    public List<string>? Languages { get; set; }

    public Item(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}