﻿using DotnetApiTemplate.Shared.Abstractions.Entities;

namespace DotnetApiTemplate.Domain.Entities;

public sealed class UserRole : BaseEntity, IEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }

    public Role? Role { get; set; }
    public User? User { get; set; }
}