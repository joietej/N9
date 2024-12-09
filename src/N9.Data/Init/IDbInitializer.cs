namespace N9.Data.Init;

public interface IDbInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken = default);
}