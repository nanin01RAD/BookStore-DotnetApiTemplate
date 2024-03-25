using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Encryption;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DotnetApiTemplate.Infrastructure.Services;

public class CartOrderService : ICartOrderService
{
    private readonly IDbContext _dbContext;

    public CartOrderService(IDbContext dbContext, ISalter salter)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Order> GetBaseQuery()
        => _dbContext.Set<Order>()
            .Include(e => e.OrderDetails)
            .AsQueryable();

    public Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(e => e.OrderId == id)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<List<OrderDetail>> GetDetailByIdAsync(Guid OrderId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<OrderDetail>()
            .Include(e => e.Book)
            .Where(e => e.OrderId == OrderId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Order?> CreateAsync(Order entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.InsertAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var CartOrder = await GetByIdAsync(id, cancellationToken);
        if (CartOrder is null)
            throw new Exception("Data not found");

        _dbContext.AttachEntity(CartOrder);

        CartOrder.SetToDeleted();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Order?> GetByExpressionAsync(
        Expression<Func<Order, bool>> predicate,
        Expression<Func<Order, Order>> projection,
        CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(predicate)
            .Select(projection)
            .FirstOrDefaultAsync(cancellationToken);
}