using Domain.Enums;

namespace Domain.Entities.ValueObjects;

public class AccordionDetail
{
    public int Index { get; set; }
    public BehaviorType Behavior { get; set; }
    public string? Label { get; set; }
    public object Data { get; set; }

    public AccordionDetail(int index, BehaviorType behavior, string? label, object data)
    {
        Index = index;
        Behavior = behavior;
        Label = label;
        Data = data;
    }
}