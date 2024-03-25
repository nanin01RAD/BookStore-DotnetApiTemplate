using DotnetApiTemplate.WebApi.Scopes;

namespace DotnetApiTemplate.WebApi.Endpoints.InventoryManagement.Scopes;

public class InventoryManagementScopeReadOnly : IScope
{
    public string ScopeName => $"{nameof(InventoryManagementScope)}.readonly".ToLower();
}