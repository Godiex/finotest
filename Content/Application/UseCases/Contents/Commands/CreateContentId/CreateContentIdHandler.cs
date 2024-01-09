using Domain.Entities.Idempotency;
using Domain.Ports;

namespace Application.UseCases.Contents.Commands.CreateContentId;

public class CreateContentIdHandler : IRequestHandler<CreateContentIdCommand, CreateContentIdDto>
{
    private readonly IIdempotencyRepository<ContentId, Guid> _repository;

    public CreateContentIdHandler(IIdempotencyRepository<ContentId, Guid> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<CreateContentIdDto> Handle(CreateContentIdCommand request, CancellationToken cancellationToken)
    {
        var contentId = await _repository.AddAsync(new ContentId());
        return new CreateContentIdDto(contentId.Id);
    }

}