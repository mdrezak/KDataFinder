using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KDataFinder.ConsoleApp
{
    public class Program
    {
        public static IConfigurationRoot Configuration;
        public static IServiceProvider ServiceProvider;
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
        static void ConfigureServices(IServiceCollection descriptors)
        {
        }
        static void ConfigureLogging(ILoggingBuilder builder)
        {
        }
        static void Main(string[] args)
        {
        }
    }
}