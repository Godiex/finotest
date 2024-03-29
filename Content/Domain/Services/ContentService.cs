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
    public async Task DeleteAsync(Guid id)
    {
        var content = await GetContentById(id);
        await _contentRepository.DeleteAsync(content);
    }

    public async Task UpdateAsync(Content content)
    {
        await _contentRepository.UpdateAsync(content);
    }

    public async Task ValidateTagToUpdate(string tag, string tagToUpdate)
    {
        if (tag != tagToUpdate)
        {
            await ValidateExistingTag(tagToUpdate);
        }
    }
    public async Task<Content> GetContentById(Guid id)
    {
        var content = await _contentRepository.GetByIdAsync(id);
        _ = content ??
            throw new ResourceNotFoundException(string.Format(Messages.ResourceNotFoundException, nameof(id), id));
        return content;
    }

    private async Task ValidateExistingTag(string tag)
    {
        bool existTag = await _contentRepository.Exist(content => content.Tag == tag);
        if (existTag)
        {
            string message = string.Format(Messages.AlredyExistException, nameof(tag), tag);
            throw new ResourceAlreadyExistException(message);
        }
    }
}