using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetApiTemplate.Persistence.SqlServer.Configurations;

public class UserRoleConfiguration : BaseEntityConfiguration<UserRole>
{
    protected override void EntityConfiguration(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(e => new { e.UserId, e.RoleId });
        builder.Property(e => e.RoleId)
            .HasMaxLength(100);
    }
}