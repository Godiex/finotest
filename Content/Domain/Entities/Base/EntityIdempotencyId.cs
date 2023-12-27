namespace Domain.Entities.Base;

public class EntityIdempotencyId
{
}

public class EntityIdempotencyId<T>: EntityIdempotencyId, IEntityBase<T>
{
    public T Id { get; init; }
}