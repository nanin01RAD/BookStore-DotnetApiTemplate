using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Shared.Abstractions.Clock;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Encryption;
using DotnetApiTemplate.WebApi.Common;
using DotnetApiTemplate.WebApi.Contracts.Responses;
using DotnetApiTemplate.WebApi.Endpoints.Identity;
using DotnetApiTemplate.WebApi.Endpoints.Identity.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Xunit.Abstractions;

namespace DotnetApiTemplate.IntegrationTests.Endpoints.Identity;

public class SignInTests : IClassFixture<SignInFixture>
{
    private readonly SignInFixture _fixture;

    public SignInTests(SignInFixture fixture, ITestOutputHelper testOutputHelper)
    {
        fixture.SetOutput(testOutputHelper);
        fixture.ConstructFixture();
        _fixture = fixture;
    }

    /// <summary>
    /// Given :
    /// Invalid request payload
    ///
    /// Expected :
    /// Return bad request with value object of type <see cref="Error"/>
    /// </summary>
    [Fact]
    public async Task SignIn_Given_InvalidRequest_ShouldReturn_BadRequest()
    {
        // Arrange
        using var scope = _fixture.ServiceProvider.CreateScope();

        // Act
        var request = new SignInRequest
        {
            Username = string.Empty,
            Password = string.Empty
        };

        var signIn = new SignIn(
            scope.ServiceProvider.GetRequiredService<IDbContext>(),
            scope.ServiceProvider.GetRequiredService<IUserService>(),
            scope.ServiceProvider.GetRequiredService<IClock>(),
            scope.ServiceProvider.GetRequiredService<IAuthManager>(),
            scope.ServiceProvider.GetRequiredService<IStringLocalizer<SignIn>>(),
            scope.ServiceProvider.GetRequiredService<IAES>());

        var result = await signIn.HandleAsync(request);

        // Assert the expected results
        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType(typeof(BadRequestObjectResult));
        var actual = (result.Result! as BadRequestObjectResult)!;
        actual.StatusCode.ShouldBe(400);
        actual.Value.ShouldBeOfType<Error>();
    }

    /// <summary>
    /// Given :
    /// Correct request payload
    ///
    /// Expected :
    /// Return bad request with value object of type <see cref="Error"/>
    /// </summary>
    [Fact]
    public async Task SignIn_Given_CorrectRequest_WithInvalidUsernameOrPassword_ShouldReturn_BadRequest()
    {
        // Arrange
        using var scope = _fixture.ServiceProvider.CreateScope();

        // Act
        var request = new SignInRequest
        {
            Username = "lorep",
            Password = "doloripsum"
        };

        var signIn = new SignIn(
            scope.ServiceProvider.GetRequiredService<IDbContext>(),
            scope.ServiceProvider.GetRequiredService<IUserService>(),
            scope.ServiceProvider.GetRequiredService<IClock>(),
            scope.ServiceProvider.GetRequiredService<IAuthManager>(),
            scope.ServiceProvider.GetRequiredService<IStringLocalizer<SignIn>>(),
            scope.ServiceProvider.GetRequiredService<IAES>());

        var result = await signIn.HandleAsync(request);

        // Assert the expected results
        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType(typeof(BadRequestObjectResult));
        var actual = (result.Result! as BadRequestObjectResult)!;
        actual.StatusCode.ShouldBe(400);
        actual.Value.ShouldBeOfType<Error>();
    }

    /// <summary>
    /// Given :
    /// Correct request payload
    ///
    /// Expected :
    /// Return Ok.
    /// </summary>
    [Fact]
    public async Task SignIn_Given_CorrectRequest_WithCorrectUsernameAndPassword_ShouldReturn_LoginDto()
    {
        // Arrange
        using var scope = _fixture.ServiceProvider.CreateScope();

        // Act
        var request = new SignInRequest
        {
            Username = "admin",
            Password = "Qwerty@1234"
        };

        var signIn = new SignIn(
            scope.ServiceProvider.GetRequiredService<IDbContext>(),
            scope.ServiceProvider.GetRequiredService<IUserService>(),
            scope.ServiceProvider.GetRequiredService<IClock>(),
            scope.ServiceProvider.GetRequiredService<IAuthManager>(),
            scope.ServiceProvider.GetRequiredService<IStringLocalizer<SignIn>>(),
            scope.ServiceProvider.GetRequiredService<IAES>());

        var result = await signIn.HandleAsync(request);

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType(typeof(OkObjectResult));
        var actual = (result.Result! as OkObjectResult)!;
        actual.Value.ShouldNotBeNull();
        actual.Value.ShouldBeOfType<LoginResponse>();

        var dto = (actual.Value! as LoginResponse)!;
        dto.UserId.ShouldBe(default);
        dto.AccessToken.ShouldNotBeNullOrWhiteSpace();
        dto.RefreshToken.ShouldNotBeNullOrWhiteSpace();
        dto.Expiry.ShouldBeGreaterThan(0);
    }
}