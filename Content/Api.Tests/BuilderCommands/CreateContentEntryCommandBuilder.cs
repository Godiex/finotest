using Application.UseCases.Contents.Commands.CreateContent;
using Microsoft.AspNetCore.Http;

namespace Api.Tests.BuilderCommands;

public class CreateContentEntryCommandBuilder
{
    private  string _tag = "tag";
    private IFormFile? _logo = null;
    private List<IFormFile>? _carousel = null;
    private List<ContentCommand>? _contents = null;
    private StylesCommand? _styles = null;
    private List<string>? _languages = null;
    
    public CreateContentEntryCommandBuilder WithTag(string tag)
    {
        _tag = tag;
        return this;
    }
    
    public CreateContentEntryCommand Build()
    {
        return new CreateContentEntryCommand(_tag,_logo,_carousel,_contents,_styles,_languages);
    }

}