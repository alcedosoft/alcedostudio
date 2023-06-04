Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Volo.Abp", LogEventLevel.Warning)
#if DEBUG
    .MinimumLevel.Override("[PROJECTNAME]", LogEventLevel.Debug)
#else
    .MinimumLevel.Override("[PROJECTNAME]", LogEventLevel.Information)
#endif
    .Enrich.FromLogContext()
    .WriteTo.Async(c => c.File("Logs/logs.txt"))
    .WriteTo.Async(c => c.Console())
    .CreateLogger();

await Host.CreateDefaultBuilder(args)
    .AddAppSettingsSecretsJson()
    .ConfigureLogging((context, logging) => logging.ClearProviders())
    .ConfigureServices((hostContext, services) =>
    {
        _ = services.AddHostedService<DbMigratorHostedService>();
    })
    .RunConsoleAsync();
