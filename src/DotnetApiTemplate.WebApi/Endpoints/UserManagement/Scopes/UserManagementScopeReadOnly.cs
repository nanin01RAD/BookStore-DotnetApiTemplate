using DotnetApiTemplate.WebApi.Scopes;

namespace DotnetApiTemplate.WebApi.Endpoints.UserManagement.Scopes;

public class UserManagementScopeReadOnly : IScope
{
    public string ScopeName => $"{nameof(UserManagementScope)}.readonly".ToLower();
}