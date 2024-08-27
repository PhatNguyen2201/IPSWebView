var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    var env = hostingContext.HostingEnvironment;

    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json",
                         optional: true, reloadOnChange: true);

    config.AddEnvironmentVariables();

    if (args != null)
    {
        config.AddCommandLine(args);
    }
})
    .ConfigureLogging((hostingContext, logging) =>
    {
        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
        logging.AddConsole();
        logging.AddDebug();
    });

builder.WebHost.UseKestrel();
builder.WebHost.UseIIS();
builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory());
var app = builder.Build();

app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ExocadHome}/{action=Index}/{id?}");
app.UseStaticFiles();

app.Run();