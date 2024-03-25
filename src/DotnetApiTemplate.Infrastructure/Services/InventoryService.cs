using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Encryption;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DotnetApiTemplate.Infrastructure.Services;

public class InventoryService : IInventoryService
{
    private readonly IDbContext _dbContext;

    public InventoryService(IDbContext dbContext, ISalter salter)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Inventory> GetBaseQuery()
        => _dbContext.Set<Inventory>()
            .Include(e => e.Book)
            .AsQueryable();

    public Task<Inventory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(e => e.InventoryId == id)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<List<Inventory>> GetByBookIdAsync(Guid BookId, CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(e => e.BookId == BookId)
            .ToListAsync(cancellationToken);

    public async Task<Inventory?> CreateAsync(Inventory entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.InsertAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var Inventory = await GetByIdAsync(id, cancellationToken);
        if (Inventory is null)
            throw new Exception("Data not found");

        _dbContext.AttachEntity(Inventory);

        Inventory.SetToDeleted();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Inventory?> GetByExpressionAsync(
        Expression<Func<Inventory, bool>> predicate,
        Expression<Func<Inventory, Inventory>> projection,
        CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(predicate)
            .Select(projection)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<bool> IsInventoryExistAsync(Guid BookId, CancellationToken cancellationToken = default)
    {
        return GetBaseQuery().Where(e => e.BookId == BookId)
            .AnyAsync(cancellationToken);
    }
}