using DotnetApiTemplate.Shared.Abstractions.Queries;

namespace DotnetApiTemplate.WebApi.Endpoints.BookManagement.Requests;

public class GetAllBookPaginatedRequest : BasePaginationCalculation
{
    public string? Code { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Publisher { get; set; }
    public string? YearPublish { get; set; }
    public string? Genre { get; set; }
}