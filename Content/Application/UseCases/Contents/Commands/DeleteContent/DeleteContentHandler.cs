using Domain.Services;

namespace Application.UseCases.Contents.Commands.DeleteContent;

public class DeleteContentHandler : IRequestHandler<DeleteContentCommand>
{
    private readonly ContentService _contentCreationService;
    
    public DeleteContentHandler(ContentService service)
    {
        _contentCreationService = service ?? throw new ArgumentNullException(nameof(service));
    }

    public async Task<Unit> Handle(DeleteContentCommand request, CancellationToken cancellationToken)
    {
        await _contentCreationService.DeleteAsync(request.Id);
        return new Unit();
    }
}