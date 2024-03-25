using DotnetApiTemplate.WebApi.Scopes;

namespace DotnetApiTemplate.WebApi.Endpoints.RoleManagement.Scopes;

public class RoleManagementScope : IScope
{
    public string ScopeName => nameof(RoleManagementScope).ToLower();
}