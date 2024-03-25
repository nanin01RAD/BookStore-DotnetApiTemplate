using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Shared.Abstractions.Clock;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Encryption;
using DotnetApiTemplate.WebApi.Common;
using DotnetApiTemplate.WebApi.Endpoints.Identity;
using DotnetApiTemplate.WebApi.Endpoints.Identity.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Xunit.Abstractions;

namespace DotnetApiTemplate.IntegrationTests.Endpoints.Identity;

public class RefreshTokenTests : IClassFixture<RefreshTokenFixture>
{
    private readonly RefreshTokenFixture _fixture;

    public RefreshTokenTests(RefreshTokenFixture fixture, ITestOutputHelper testOutputHelper)
    {
        fixture.SetOutput(testOutputHelper);
        fixture.ConstructFixture();
        _fixture = fixture;
    }

    public static IEnumerable<object[]> GetInvalidRequests()
    {
        yield return new object[]
        {
            // all empty
            new RefreshTokenRequest
            {
                RefreshToken = ""
            }
        };
        yield return new object[]
        {
            // all empty
            new RefreshTokenRequest
            {
                RefreshToken = string.Empty
            }
        };
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public async Task RefreshToken_Given_InvalidRequest_ShouldReturn_BadRequest(RefreshTokenRequest request)
    {
        // Arrange
        using var scope = _fixture.ServiceProvider.CreateScope();

        var refreshToken = new RefreshToken(
            scope.ServiceProvider.GetRequiredService<IDbContext>(),
            scope.ServiceProvider.GetRequiredService<IClock>(),
            scope.ServiceProvider.GetRequiredService<IUserService>(),
            scope.ServiceProvider.GetRequiredService<IAuthManager>(),
            scope.ServiceProvider.GetRequiredService<IStringLocalizer<RefreshToken>>(),
            scope.ServiceProvider.GetRequiredService<IAES>());

        // Act
        var result = await refreshToken.HandleAsync(request);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.Result.ShouldBeOfType(typeof(BadRequestObjectResult));
        var actual = (result.Result as BadRequestObjectResult)!;
        actual.StatusCode.ShouldBe(400);
        actual.Value.ShouldBeOfType<Error>();
    }

    [Fact]
    public async Task RefreshToken_Given_CorrectRequest_WithInvalidValue_ShouldReturn_BadRequest()
    {
        // Arrange
        using var scope = _fixture.ServiceProvider.CreateScope();

        var refreshToken = new RefreshToken(
            scope.ServiceProvider.GetRequiredService<IDbContext>(),
            scope.ServiceProvider.GetRequiredService<IClock>(),
            scope.ServiceProvider.GetRequiredService<IUserService>(),
            scope.ServiceProvider.GetRequiredService<IAuthManager>(),
            scope.ServiceProvider.GetRequiredService<IStringLocalizer<RefreshToken>>(),
            scope.ServiceProvider.GetRequiredService<IAES>());

        // all empty
        var request = new RefreshTokenRequest
        {
            RefreshToken = "Hahihuheho"
        };

        // Act
        var result = await refreshToken.HandleAsync(request);
        // Assert the expected results
        result.ShouldNotBeNull();
        result.Result.ShouldBeOfType(typeof(BadRequestObjectResult));
        var actual = (result.Result as BadRequestObjectResult)!;
        actual.StatusCode.ShouldBe(400);
        actual.Value.ShouldBeOfType<Error>();
    }
}