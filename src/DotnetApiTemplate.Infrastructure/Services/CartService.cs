using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Encryption;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DotnetApiTemplate.Infrastructure.Services;

public class CartService : ICartService
{
    private readonly IDbContext _dbContext;

    public CartService(IDbContext dbContext, ISalter salter)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Cart> GetBaseQuery()
        => _dbContext.Set<Cart>()
            .Include(e => e.Book)
            .Include(e => e.User)
            .AsQueryable();

    public Task<Cart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(e => e.CartId == id)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Cart?> CreateAsync(Cart entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.InsertAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var CartCart = await GetByIdAsync(id, cancellationToken);
        if (CartCart is null)
            throw new Exception("Data not found");

        _dbContext.AttachEntity(CartCart);

        CartCart.SetToDeleted();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Cart?> GetByExpressionAsync(
        Expression<Func<Cart, bool>> predicate,
        Expression<Func<Cart, Cart>> projection,
        CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(predicate)
            .Select(projection)
            .FirstOrDefaultAsync(cancellationToken);
}