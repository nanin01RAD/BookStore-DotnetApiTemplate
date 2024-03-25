using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetApiTemplate.Persistence.Postgres.Configurations;

public class BookConfiguration : BaseEntityConfiguration<Book>
{
    protected override void EntityConfiguration(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(e => e.BookId);
        builder.Property(e => e.BookId).ValueGeneratedNever();
        builder.Property(e => e.Code).HasMaxLength(256);
        builder.Property(e => e.Title).HasMaxLength(256);
        builder.Property(e => e.Author).HasMaxLength(256);
        builder.Property(e => e.Publisher).HasMaxLength(256);
        builder.Property(e => e.YearPublish).HasColumnType("int");
        builder.Property(e => e.Genre).HasMaxLength(256);
        builder.Property(e => e.Description).HasColumnType("text");
        builder.Property(e => e.Price).HasColumnType("decimal");
        builder.Property(e => e.QtyAvailable).HasColumnType("int");
    }
}