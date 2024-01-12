namespace Domain.Entities;

public class Item
{
    public int LanguageIndex { get; set; }
    public string? Title { get; set; }
    public List<DynamicContent> Contents { get; set; }

    public Item(int index, string? title, List<DynamicContent> contents)
    {
        LanguageIndex = index;
        Title = title;
        Contents = contents;
    }

    public Item() { }
}