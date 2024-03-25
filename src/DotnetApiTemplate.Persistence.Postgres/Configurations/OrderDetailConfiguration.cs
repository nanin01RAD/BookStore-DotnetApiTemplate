using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetApiTemplate.Persistence.Postgres.Configurations;

public class OrderDetailConfiguration : BaseEntityConfiguration<OrderDetail>
{
    protected override void EntityConfiguration(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.HasKey(e => e.OrderDetailId);
        builder.Property(e => e.OrderDetailId).ValueGeneratedNever();
        builder.Property(e => e.Price).HasColumnType("decimal");
        builder.Property(e => e.Qty).HasColumnType("int");
        builder.Property(e => e.TotalPrice).HasColumnType("decimal");
    }
}