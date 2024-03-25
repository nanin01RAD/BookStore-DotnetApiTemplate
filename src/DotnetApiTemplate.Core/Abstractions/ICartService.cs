using DotnetApiTemplate.Domain.Entities;

namespace DotnetApiTemplate.Core.Abstractions;

/// <summary>
/// Default implementation is AsNoTracking true.
/// </summary>
public interface ICartService : IEntityService<Cart>
{
}