using Domain.Entities;
using Domain.Entities.Idempotency;
using Domain.Enums;
using Domain.Ports;
using Domain.Services;

namespace Application.UseCases.Contents.Commands.CreateContent;

public class CreateContentHandler : IRequestHandler<CreateContentCommand>
{
    private readonly ContentService _contentService;
    private readonly IIdempotencyRepository<ContentId, Guid> _contentIdIdRepository;
    
    public CreateContentHandler(ContentService service, IIdempotencyRepository<ContentId, Guid> contentIdIdReponsitory)
    {
        _contentService = service ?? throw new ArgumentNullException(nameof(service));
        _contentIdIdRepository = contentIdIdReponsitory ?? throw new ArgumentNullException(nameof(contentIdIdReponsitory));
    }

    public async Task<Unit> Handle(CreateContentCommand request, CancellationToken cancellationToken)
    {
        await _contentIdIdRepository.ValidateEntityId(request.Id);
        await _contentIdIdRepository.RemoveAsync(request.Id);
        var content = MapCommandToEntity(request);
        await _contentService.CreateAsync(content);
        return new Unit();
    }

    private static Content MapCommandToEntity(CreateContentCommand request)
    {
        return new Content(
            request.Tag,
            request.LogoUrl,
            request.Multimedia,
            request.Languages,
            request.TitleContent != null ? new DynamicContent(ContentType.PLANE_TEXT, request.TitleContent) : null,
            request.Items?.ConvertAll(
                x => new Item(
                    x.Index, x.Behavior, x.Contents.ConvertAll(
                        section => new DynamicContent(ContentType.SECTION, section)
                    )
                )
            )
        );
    }
}