using DotnetApiTemplate.WebApi.Scopes;

namespace DotnetApiTemplate.WebApi.Endpoints.CartOrder.Scopes;

public class CartOrderScopeReadOnly : IScope
{
    public string ScopeName => $"{nameof(CartOrderScope)}.readonly".ToLower();
}