await Host.CreateDefaultBuilder(args)
    .AddAppSettingsSecretsJson()
    .ConfigureServices((hostContext, services) =>
    {
        _ = services.AddHostedService<ConsoleTestAppHostedService>();
    })
    .RunConsoleAsync();
