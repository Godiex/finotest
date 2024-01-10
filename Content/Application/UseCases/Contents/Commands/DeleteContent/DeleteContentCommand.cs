namespace Application.UseCases.Contents.Commands.DeleteContent;

public record DeleteContentCommand(Guid Id) : IRequest<Unit>;