using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System.Reflection;

namespace KDataFinder.ConsoleApp
{
    internal class SeleniumDriverSetup
    {
        #region Options
#nullable disable
        public string DriverName { get; set; }
        public string DriverPath { get; set; }
#nullable restore
        #endregion
        public static IWebDriver SetupDriver(IServiceProvider serviceProvider)
        {
            ILogger logger = serviceProvider.GetRequiredService<ILogger<SeleniumDriverSetup>>();
            SeleniumDriverSetup options = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<SeleniumDriverSetup>>().Value;
            #region InitializeDriver
         
            string driverClassFullName = $"OpenQA.Selenium.{options.DriverName}.{options.DriverName}Driver, WebDriver";
            Type type = Type.GetType(driverClassFullName)
                ?? throw new InvalidDataException($"Driver name {options.DriverName} is not exist in {driverClassFullName}");
            ConstructorInfo constructor = type.GetConstructor(new[] { typeof(string) })
                ?? throw new InvalidDataException($"Driver {options.DriverName} can not initialize with DriverPath parameter");
            object instance = constructor.Invoke(new object[] { options.DriverPath });
            
            #endregion
            return (IWebDriver)instance;
        }
    }
}
