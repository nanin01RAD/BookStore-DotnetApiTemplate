using DotnetApiTemplate.WebApi.Scopes;

namespace DotnetApiTemplate.WebApi.Endpoints.CartOrder.Scopes;

public class CartOrderScope : IScope
{
    public string ScopeName => nameof(CartOrderScope).ToLower();
}