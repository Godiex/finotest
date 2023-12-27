namespace Domain.Ports;

public interface IIdempotencyRepository<T, in TId> : IDisposable
{
    Task<T> AddAsync(T entity);
    Task ValidateEntityId(TId id);
    Task RemoveAsync(TId id);
}