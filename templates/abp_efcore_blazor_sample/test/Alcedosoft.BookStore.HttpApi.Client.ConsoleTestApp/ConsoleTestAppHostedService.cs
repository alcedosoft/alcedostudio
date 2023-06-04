using Volo.Abp;

namespace Alcedosoft.BookStore;

public class ConsoleTestAppHostedService : IHostedService
{
    private readonly IConfiguration _configuration;

    public ConsoleTestAppHostedService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var application = await AbpApplicationFactory
            .CreateAsync<BookStoreConsoleApiClientModule>(options =>
            {
                _ = options.Services.ReplaceConfiguration(_configuration);
                options.UseAutofac();
            });

        await application.InitializeAsync();

        await application.ServiceProvider
            .GetRequiredService<ClientDemoService>()
            .RunAsync();

        await application.ShutdownAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
