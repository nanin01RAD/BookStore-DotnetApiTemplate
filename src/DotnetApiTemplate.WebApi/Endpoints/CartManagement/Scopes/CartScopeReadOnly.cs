using DotnetApiTemplate.WebApi.Scopes;

namespace DotnetApiTemplate.WebApi.Endpoints.CartManagement.Scopes;

public class CartScopeReadOnly : IScope
{
    public string ScopeName => $"{nameof(CartScope)}.readonly".ToLower();
}