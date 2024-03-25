using DotnetApiTemplate.Shared.Abstractions.Entities;

namespace DotnetApiTemplate.Domain.Entities;

public sealed class Book : BaseEntity
{
    public Guid BookId { get; set; } = Guid.NewGuid();
    public string? Code { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Publisher { get; set; }
    public int? YearPublish { get; set; }
    public string? Genre { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? QtyAvailable { get; set; }
}