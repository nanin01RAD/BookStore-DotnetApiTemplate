using DotnetApiTemplate.Domain.Entities;

namespace DotnetApiTemplate.Core.Abstractions;

/// <summary>
/// Default implementation is AsNoTracking true.
/// </summary>
public interface IBookService : IEntityService<Book>
{
    Task<bool> IsBookExistAsync(string Bookname, CancellationToken cancellationToken = default);
    Task<List<OrderDetail>> IsBookOrder(Guid BookId, CancellationToken cancellationToken = default);
}