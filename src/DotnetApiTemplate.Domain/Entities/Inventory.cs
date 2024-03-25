using DotnetApiTemplate.Domain.Enums;
using DotnetApiTemplate.Shared.Abstractions.Entities;

namespace DotnetApiTemplate.Domain.Entities;

public sealed class Inventory : BaseEntity
{
    public Guid InventoryId { get; set; } = Guid.NewGuid();
    public Guid BookId { get; set; }
    public int? QtyCurrent { get; set; }
    public InventoryStatus Status { get; set; }
    public Book? Book { get; set; }
}