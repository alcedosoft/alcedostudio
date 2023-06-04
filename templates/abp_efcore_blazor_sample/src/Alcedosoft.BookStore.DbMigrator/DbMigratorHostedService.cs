namespace Alcedosoft.BookStore;

public class DbMigratorHostedService : IHostedService
{
    private readonly IConfiguration _configuration;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;

    public DbMigratorHostedService(IHostApplicationLifetime hostApplicationLifetime, IConfiguration configuration)
    {
        _configuration = configuration;
        _hostApplicationLifetime = hostApplicationLifetime;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var application = await AbpApplicationFactory.CreateAsync<BookStoreDbMigratorModule>(options =>
        {
            _ = options.Services.AddLogging(c => c.AddSerilog());
            _ = options.Services.ReplaceConfiguration(_configuration);

            options.UseAutofac();
        });

        await application.InitializeAsync();

        await application
            .ServiceProvider
            .GetRequiredService<BookStoreDbMigrationService>()
            .MigrateAsync();

        await application.ShutdownAsync();

        _hostApplicationLifetime.StopApplication();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
