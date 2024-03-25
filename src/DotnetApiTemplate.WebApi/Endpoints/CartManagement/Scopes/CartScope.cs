using DotnetApiTemplate.WebApi.Scopes;

namespace DotnetApiTemplate.WebApi.Endpoints.CartManagement.Scopes;

public class CartScope : IScope
{
    public string ScopeName => nameof(CartScope).ToLower();
}