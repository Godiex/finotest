using Domain.Enums;

namespace Domain.Entities;

public class Item
{
    public int Index { get; set; }
    public BehaviorType Behavior { get; set; }
    public List<DynamicContent> Contents { get; set; }

    public Item(int index, BehaviorType behavior, List<DynamicContent> contents)
    {
        Index = index;
        Behavior = behavior;
        Contents = contents;
    }

    public Item() { }
}