using Application.Common.Exceptions;
using Domain;
using Domain.Entities.Base;
using Domain.Exceptions.Common;
using Domain.Ports;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Adapters.Repository;

public class IdempotencyRepository<T, TId> : IIdempotencyRepository<T, TId> where T : EntityIdempotencyId<TId>
{
    private readonly PersistenceContext _context;
    public IdempotencyRepository(PersistenceContext context)
    {
        _context = context;
    }
    
    public async Task<T> AddAsync(T entity)
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity), Messages.EntityCannotBeNull);
        _context.Set<T>().Add(entity);
        await CommitAsync();
        return entity;
    }

    public async Task ValidateEntityId(TId id)
    {
        var exist = await _context.Set<T>().AnyAsync(entity => entity.Id!.Equals(id));
        
        if (!exist)
        {
            throw new ResourceNotFoundException(string.Format(Messages.ResourceNotFoundException, id));
        }
    }

    public async Task RemoveAsync(TId id)
    {
        var entity = await _context.Set<T>().FirstOrDefaultAsync(entity => entity.Id!.Equals(id));
        _context.Set<T>().Remove(entity!);
        await CommitAsync();
    }

    private async Task CommitAsync()
    {
        await _context.CommitAsync().ConfigureAwait(false);
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }
    
}