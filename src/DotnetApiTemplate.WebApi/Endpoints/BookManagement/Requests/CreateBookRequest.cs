namespace DotnetApiTemplate.WebApi.Endpoints.BookManagement.Requests;

public class CreateBookRequest
{
    public string? Code { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Publisher { get; set; }
    public int? YearPublish { get; set; }
    public string? Genre { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? Qty { get; set; }
}