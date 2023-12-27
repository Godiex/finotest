namespace Domain.Entities;

public class Content : EntityBase<Guid>, IAggregateRoot
{
    public string Tag { get; set; }
    public string? LogoUrl { get; set; }
    public List<string>? Multimedia { get; set; }
    public List<string>? Languages { get; set; }

    public Content(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public void Update(string name, string description)
    {
        if (Name.Equals(name) is not true) Name = name;
        if (Description.Equals(description) is not true) Description = description;
    }
}