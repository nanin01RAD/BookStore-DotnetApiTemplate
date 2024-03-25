﻿using DotnetApiTemplate.Domain.Entities;

namespace DotnetApiTemplate.UnitTests.Entities;

public class RoleScopeTests
{
    [Fact]
    public void RoleScope_Ctor_Should_Do_As_Expected()
    {
        var roleScope = new RoleScope();
        roleScope.RoleScopeId.ShouldNotBe(Guid.Empty);
    }
}