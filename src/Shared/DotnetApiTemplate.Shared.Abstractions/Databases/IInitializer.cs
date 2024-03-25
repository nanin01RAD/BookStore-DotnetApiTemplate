namespace DotnetApiTemplate.Shared.Abstractions.Databases;

public interface IInitializer
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}