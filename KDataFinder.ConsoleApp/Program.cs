using KDataFinder.ConsoleApp.Abstraction;
using KDataFinder.ConsoleApp.Implementation.Selenium;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KDataFinder.ConsoleApp;

internal class Program
{
    public static IConfigurationRoot Configuration;
    public static ServiceProvider ServiceProvider;
    #region Initialization
    static Program()
    {
        //setup & build configuration
        Configuration = new ConfigurationBuilder()
                   .AddUserSecrets<Program>()
                   .AddEnvironmentVariables()
                   .AddJsonFile("appsetting.json", false)
                   .AddJsonFile($"appsetting.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
                   .Build();

        //setup DI
        var serviceCollection = new ServiceCollection()
            .AddLogging(ConfigureLogging)
            .AddSingleton(Configuration);

        ConfigureServices(serviceCollection);

        //build DI
        ServiceProvider = serviceCollection.BuildServiceProvider();

    }
    #endregion
    static void ConfigureServices(IServiceCollection services)
    {
        #region Selenium
        services.Configure<SeleniumDriverSetup>(Configuration.GetSection(nameof(SeleniumDriverSetup)));
        services.AddSingleton(SeleniumDriverSetup.SetupDriver);
        #endregion
        #region Login
        services.Configure<LoginOptions>(Configuration.GetSection(nameof(LoginOptions)));
        services.AddSingleton<ILoginService, LoginService>();
        #endregion
        #region TableObtainer
        services.Configure<TableObtainerOptions>(Configuration.GetSection(nameof(TableObtainerOptions)));
        services.AddSingleton<ITableDataObtainer, TableDataObtainer>();
        services.AddSingleton<ITableRowDetialObtainer, TableRowDetialObtainer>();
        #endregion
        services.AddTransient<KApplication>();
    }
    static void ConfigureLogging(ILoggingBuilder builder)
    {
        builder.ClearProviders();
        builder.AddConfiguration(Configuration.GetSection("Logging"));
        builder.AddDebug();
        builder.AddConsole();
    }
    static void Main(string[] args)
    {
        var application = ServiceProvider.GetRequiredService<KApplication>();
        application.Run();
        ServiceProvider.Dispose();
        if (Configuration is ConfigurationRoot disposableConfiguration)
            disposableConfiguration.Dispose();
    }
}