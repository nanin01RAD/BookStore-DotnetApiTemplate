using Microsoft.AspNetCore.Mvc;

namespace DotnetApiTemplate.WebApi.Contracts.Requests;

public class DeleteCacheRequest
{
    [FromRoute(Name = "key")] public string Key { get; set; } = null!;
}