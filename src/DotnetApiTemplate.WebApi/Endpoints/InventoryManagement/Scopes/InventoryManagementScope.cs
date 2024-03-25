using DotnetApiTemplate.WebApi.Scopes;

namespace DotnetApiTemplate.WebApi.Endpoints.InventoryManagement.Scopes;

public class InventoryManagementScope : IScope
{
    public string ScopeName => nameof(InventoryManagementScope).ToLower();
}