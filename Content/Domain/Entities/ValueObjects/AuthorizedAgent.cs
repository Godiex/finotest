using Domain.Services.Companies.Dto;

namespace Domain.Entities.ValueObjects;

public class AuthorizedAgent : ValueObject
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public Identity Identity { get; set; }

    public AuthorizedAgent() { }

    public AuthorizedAgent(string name, string surname, string email, Identity identity)
    {
        Name = name;
        Surname = surname;
        Email = email;
        Identity = identity;
    }

    public void Update(AuthorizeAgentToUpdateDto authorizeAgentToUpdate)
    {
        Name = authorizeAgentToUpdate.Name;
        Surname = authorizeAgentToUpdate.Surname;
        Email = authorizeAgentToUpdate.Email;
        Identity.Update(authorizeAgentToUpdate.Identity);
    }
}