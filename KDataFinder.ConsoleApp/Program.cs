using KDataFinder.ConsoleApp.Abstraction;
using KDataFinder.ConsoleApp.Implementation.Selenium;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;

namespace KDataFinder.ConsoleApp
{
    internal class Program
    {

        public static IConfigurationRoot Configuration;
        public static IServiceProvider ServiceProvider;
        #region Initialization
        static Program()
        {
            //setup & build configuration
            Configuration = new ConfigurationBuilder()
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
            services.AddTransient(SeleniumDriverSetup.SetupDriver);
            #endregion
            #region Login
            services.Configure<LoginOptions>(Configuration.GetSection(nameof(LoginOptions)));
            services.AddTransient<ILoginService, LoginService>();
            #endregion
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
            var loginService = ServiceProvider.GetRequiredService<ILoginService>();
            loginService.Login();
        }
    }
}