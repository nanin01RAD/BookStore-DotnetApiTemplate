using Microsoft.AspNetCore.Mvc;

namespace DotnetApiTemplate.WebApi.Endpoints.BookManagement.Requests;

public class GetBookByIdRequest
{
    [FromRoute(Name = "BookId")] public Guid BookId { get; set; }
}