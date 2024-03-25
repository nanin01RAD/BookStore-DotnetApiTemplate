using DotnetApiTemplate.Shared.Abstractions.Serialization;
using DotnetApiTemplate.Shared.Infrastructure.Serialization.SystemTextJson;
using Microsoft.Extensions.DependencyInjection;

namespace DotnetApiTemplate.Shared.Infrastructure.Serialization;

public static class ServiceCollection
{
    public static void AddJsonSerialization(this IServiceCollection services)
    {
        services.AddSingleton<IJsonSerializer, SystemTextJsonSerializer>();
    }
}