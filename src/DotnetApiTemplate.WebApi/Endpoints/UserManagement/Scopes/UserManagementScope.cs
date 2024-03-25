using DotnetApiTemplate.WebApi.Scopes;

namespace DotnetApiTemplate.WebApi.Endpoints.UserManagement.Scopes;

public class UserManagementScope : IScope
{
    public string ScopeName => nameof(UserManagementScope).ToLower();
}