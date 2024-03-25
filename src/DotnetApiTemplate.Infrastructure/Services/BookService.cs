using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Encryption;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DotnetApiTemplate.Infrastructure.Services;

public class BookService : IBookService
{
    private readonly IDbContext _dbContext;

    public BookService(IDbContext dbContext, ISalter salter)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Book> GetBaseQuery()
        => _dbContext.Set<Book>()
            .AsQueryable();

    public Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(e => e.BookId == id)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Book?> CreateAsync(Book entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.InsertAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var Book = await GetByIdAsync(id, cancellationToken);
        if (Book is null)
            throw new Exception("Data not found");

        _dbContext.AttachEntity(Book);

        Book.SetToDeleted();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<OrderDetail>> IsBookOrder(Guid id, CancellationToken cancellationToken = default)
    {
        var Book = await _dbContext.Set<OrderDetail>()
            .Where(e => e.BookId == id)
            .ToListAsync(cancellationToken);

        return Book;
    }

    public Task<Book?> GetByExpressionAsync(
        Expression<Func<Book, bool>> predicate,
        Expression<Func<Book, Book>> projection,
        CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(predicate)
            .Select(projection)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<bool> IsBookExistAsync(string Bookname, CancellationToken cancellationToken = default)
    {
        Bookname = Bookname.ToUpper();

        return GetBaseQuery().Where(e => e.Title!.ToUpper() == Bookname)
            .AnyAsync(cancellationToken);
    }
}