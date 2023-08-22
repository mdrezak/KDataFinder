using KDataFinder.ConsoleApp.Abstraction;
using KDataFinder.ConsoleApp.Implementation;
using KDataFinder.ConsoleApp.Implementation.Selenium;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            IOperationResult loginResult = loginService.Login();
            if (!loginResult.IsSucceeded)
                throw new InvalidDataException(loginResult.AdditionalData?.ToString());
            var tableDataObtainer = ServiceProvider.GetRequiredService<ITableDataObtainer>();
            tableDataObtainer.Obtain(x =>
            {
                Console.Write(x.rowNumber + "-" + x.Columns.Length + "- \t");
                foreach (var item in x.Columns)
                    Console.Write(item.ToString() + "- \t");
            });
            //var tableDataObtainer = ServiceProvider.GetRequiredService<ITableDataObtainer<string, string>>();
            //var mainTask = tableDataObtainer.Obtain();
            //var pageDataObtainer = ServiceProvider.GetRequiredService<IPageDataObtainer<string, string>>();
            //TaskManager taskManager = new(10);
            //tableDataObtainer.DataCaster = async (y) =>
            //{
            //    var x = y;
            //    await taskManager.AddTask(pageDataObtainer.Obtain(""));
            //    return null;
            //};

            //هعی حالم خیلی خرابه
            // مسغرس که توی ریپوزیتوری گیت بنویسی نه ؟
            //تا حدودی نشانه از خلا های شخصیتی یک فرد میتونه باشه
            //ولی خلا ها به همین جا ختم نمیشن
            //دنیا پر از جای خالیه
            //و دردت زیاد تره وقتی با پر کننده همشم سر لج باشی.
            //یسری کد شعر.

            //ما در هر صورت بازتاب رفتارهای دیگرانیم مهم نیست عکسش یا خودش!

            //goto table page
            //fill select box 
            //proccess table data(paging,tasks manage,saving,....)
        }
    }
}