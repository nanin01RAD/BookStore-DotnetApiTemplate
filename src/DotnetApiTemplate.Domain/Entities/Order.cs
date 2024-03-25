using DotnetApiTemplate.Domain.Enums;
using DotnetApiTemplate.Shared.Abstractions.Entities;

namespace DotnetApiTemplate.Domain.Entities;

public sealed class Order : BaseEntity
{
    public Guid OrderId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public DateOnly? OrderDate { get; set; }
    public OrderStatus? Status { get; set; }
    public decimal? Total { get; set; }
    public User? User { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();
}