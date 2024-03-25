using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetApiTemplate.Persistence.Postgres.Configurations;

public class OrderConfiguration : BaseEntityConfiguration<Order>
{
    protected override void EntityConfiguration(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => e.OrderId);
        builder.Property(e => e.OrderId).ValueGeneratedNever();
        builder.Property(e => e.OrderDate).HasColumnType("date");
        builder.Property(e => e.Total).HasColumnType("decimal");
    }
}