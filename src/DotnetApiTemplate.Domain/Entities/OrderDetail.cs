using DotnetApiTemplate.Domain.Enums;
using DotnetApiTemplate.Shared.Abstractions.Entities;

namespace DotnetApiTemplate.Domain.Entities;

public sealed class OrderDetail : BaseEntity
{
    public Guid OrderDetailId { get; set; } = Guid.NewGuid();
    public Guid OrderId { get; set; }
    public Guid BookId { get; set; }
    public decimal? Price { get; set; }
    public int? Qty { get; set; }
    public decimal? TotalPrice { get; set; }
    public Book? Book { get; set; }
    public Order? Order { get; set; }
}