using Domain.Services;

namespace Application.UseCases.Contents.Commands.DeleteContent;

public class DeleteContentHandler : IRequestHandler<DeleteContentCommand>
{
    private readonly ContentService _contentService;
    
    public DeleteContentHandler(ContentService service)
    {
        _contentService = service ?? throw new ArgumentNullException(nameof(service));
    }

    public async Task<Unit> Handle(DeleteContentCommand request, CancellationToken cancellationToken)
    {
        await _contentService.DeleteAsync(request.Id);
        return new Unit();
    }
}
