using DotnetApiTemplate.Domain.Entities;

namespace DotnetApiTemplate.Core.Abstractions;

/// <summary>
/// Default implementation is AsNoTracking true.
/// </summary>
public interface IInventoryService : IEntityService<Inventory>
{
    Task<bool> IsInventoryExistAsync(Guid BookId, CancellationToken cancellationToken = default);
    Task<List<Inventory>> GetByBookIdAsync(Guid BookId, CancellationToken cancellationToken = default);
}