using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetApiTemplate.Persistence.Postgres.Configurations;

public class InventoryConfiguration : BaseEntityConfiguration<Inventory>
{
    protected override void EntityConfiguration(EntityTypeBuilder<Inventory> builder)
    {
        builder.HasKey(e => e.InventoryId);
        builder.Property(e => e.InventoryId).ValueGeneratedNever();
        builder.Property(e => e.QtyCurrent).HasColumnType("int");
    }
}