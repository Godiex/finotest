using Domain.Entities;
using Domain.Exceptions.Common;
using Domain.Ports;

namespace Domain.Services;

[DomainService]
public class ContentService
{
    private readonly IGenericRepository<Content> _contentRepository;

    public ContentService(IGenericRepository<Content> contentRepository) =>
        _contentRepository = contentRepository;

    public async Task CreateAsync(Content content)
    {
        await ValidateExistingTag(content.Tag);
        await _contentRepository.AddAsync(content);
    }

    private async Task ValidateExistingTag(string tag)
    {
        var existTag = await _contentRepository.Exist(content => content.Tag == tag);
        if (existTag)
        {
            var message = string.Format(Messages.AlredyExistException, nameof(tag), tag);
            throw new ResourceAlreadyExistException(message);
        }
    }
}