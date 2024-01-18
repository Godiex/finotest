using Application.UseCases.Contents.Commands.CreateContent;
using Microsoft.AspNetCore.Http;

namespace Api.Tests.BuilderCommands;

public class CreateContentCommandBuilder
{
    private Guid _id = Guid.NewGuid();
    private CreateContentEntryCommand _commandEntry = new CreateContentEntryCommandBuilder().Build();

    public CreateContentCommandBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }
    public CreateContentCommandBuilder WithCommandEntry(CreateContentEntryCommand commandEntry)
    {
        _commandEntry = commandEntry;
        return this;
    }

    public CreateContentCommand Build()
    {
        return new CreateContentCommand(_id, _commandEntry);
    }
}