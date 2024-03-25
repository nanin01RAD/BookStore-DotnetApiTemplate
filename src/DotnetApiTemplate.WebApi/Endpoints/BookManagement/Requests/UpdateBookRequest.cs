using Microsoft.AspNetCore.Mvc;

namespace DotnetApiTemplate.WebApi.Endpoints.BookManagement.Requests;

public class UpdateBookRequest
{
    public UpdateBookRequest()
    {
        UpdateBookRequestPayload = new UpdateBookRequestPayload();
    }

    [FromRoute(Name = "BookId")] public Guid BookId { get; set; }
    [FromBody] public UpdateBookRequestPayload UpdateBookRequestPayload { get; set; }
}

public class UpdateBookRequestPayload
{
    public string? Code { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Publisher { get; set; }
    public int? YearPublish { get; set; }
    public string? Genre { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
}