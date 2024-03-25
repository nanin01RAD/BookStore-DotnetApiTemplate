using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Infrastructure.Services;
using DotnetApiTemplate.IntegrationTests.Helpers;
using DotnetApiTemplate.Shared.Abstractions.Clock;
using DotnetApiTemplate.Shared.Abstractions.Contexts;
using DotnetApiTemplate.Shared.Abstractions.Encryption;
using DotnetApiTemplate.Shared.Abstractions.Files;
using DotnetApiTemplate.Shared.Infrastructure.Cache;
using DotnetApiTemplate.Shared.Infrastructure.Clock;
using DotnetApiTemplate.Shared.Infrastructure.Encryption;
using DotnetApiTemplate.Shared.Infrastructure.Localization;
using DotnetApiTemplate.Shared.Infrastructure.Serialization;
using DotnetApiTemplate.WebApi.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AuthManager = DotnetApiTemplate.IntegrationTests.Helpers.AuthManager;

namespace DotnetApiTemplate.IntegrationTests;

public static class ServiceCollection
{
    public static void AddDefaultInjectedServices(this IServiceCollection services)
    {
        services.AddScoped<IContext>(_ => new Context(Guid.Empty));
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IAuthManager, AuthManager>();
        services.AddSingleton<IFileService, FileSystemServiceMock>();
        services.AddInternalMemoryCache();
        services.AddJsonSerialization();
        services.AddSingleton<IClock, Clock>();
        services.AddSingleton<ISalter, Salter>();
        services.AddEncryption();
        services.AddDistributedMemoryCache();
        services.AddLocalizerJson();
        services.AddSingleton(new ClockOptions());
        services.AddScoped<IFileRepositoryService, FileRepositoryService>();
    }

    public static void EnsureDbCreated<T>(this IServiceCollection services) where T : DbContext
    {
        var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<T>();
        context.Database.EnsureCreated();
    }
}