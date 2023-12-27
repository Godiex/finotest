using Domain.Entities.ValueObjects;

namespace Domain.Tests.BuilderEntities;

public class AuthorizedAgentBuilder
{
    private string _name = "Juan";
    private string _surname = "Jimenez";
    private string _email = "test@gmail.com";
    private Identity _identity = new IdentityBuilder().Build();

    public AuthorizedAgentBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public AuthorizedAgentBuilder WithSurname(string surname)
    {
        _surname = surname;
        return this;
    }

    public AuthorizedAgentBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public AuthorizedAgentBuilder WithIdentity(Identity identity)
    {
        _identity = identity;
        return this;
    }

    public AuthorizedAgent Build()
    {
        return new AuthorizedAgent(
            _name,
            _surname,
            _email,
            _identity
        );
    }
}