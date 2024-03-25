using DotnetApiTemplate.WebApi.Scopes;

namespace DotnetApiTemplate.WebApi.Endpoints.BookManagement.Scopes;

public class BookManagementScope : IScope
{
    public string ScopeName => nameof(BookManagementScope).ToLower();
}