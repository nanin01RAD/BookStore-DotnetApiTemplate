using DotnetApiTemplate.Core;
using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Infrastructure.Services;
using DotnetApiTemplate.Persistence.Postgres;
using DotnetApiTemplate.Shared.Abstractions.Encryption;
using DotnetApiTemplate.Shared.Infrastructure;
using DotnetApiTemplate.Shared.Infrastructure.Api;
using DotnetApiTemplate.Shared.Infrastructure.Clock;
using DotnetApiTemplate.Shared.Infrastructure.Contexts;
using DotnetApiTemplate.Shared.Infrastructure.Encryption;
using DotnetApiTemplate.Shared.Infrastructure.Files.FileSystems;
using DotnetApiTemplate.Shared.Infrastructure.Initializer;
using DotnetApiTemplate.Shared.Infrastructure.Localization;
using DotnetApiTemplate.Shared.Infrastructure.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DotnetApiTemplate.UnitTests")]

namespace DotnetApiTemplate.Infrastructure;

public static class ServiceCollection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore();
        services.AddSharedInfrastructure();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IFileRepositoryService, FileRepositoryService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<ICartOrderService, CartOrderService>();
        services.AddScoped<IInventoryService, InventoryService>();
        services.AddSingleton<ISalter, Salter>();

        //use one of these
        //services.AddSqlServerDbContext(configuration, "sqlserver");
        services.AddPostgresDbContext(configuration, "postgres");

        services.AddFileSystemService();
        services.AddJsonSerialization();
        services.AddClock();
        services.AddContext();
        services.AddEncryption();
        services.AddCors();
        services.AddCorsPolicy();
        services.AddLocalizerJson();

        //if use azure blob service
        //make sure app setting "azureBlobService":"" is filled
        //services.AddSingleton<IAzureBlobService, AzureBlobService>();

        services.AddInitializer<CoreInitializer>();
        services.AddApplicationInitializer();
    }
}