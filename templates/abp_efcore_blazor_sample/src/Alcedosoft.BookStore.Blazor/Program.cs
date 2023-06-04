var builder = WebAssemblyHostBuilder.CreateDefault(args);

var application = await builder.AddApplicationAsync<BookStoreBlazorModule>(options =>
{
    options.UseAutofac();
});

var host = builder.Build();

await application.InitializeApplicationAsync(host.Services);

await host.RunAsync();
