using DotnetApiTemplate.WebApi.Scopes;

namespace DotnetApiTemplate.WebApi.Endpoints.BookManagement.Scopes;

public class BookManagementScopeReadOnly : IScope
{
    public string ScopeName => $"{nameof(BookManagementScope)}.readonly".ToLower();
}