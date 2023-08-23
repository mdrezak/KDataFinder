using KDataFinder.ConsoleApp.Abstraction;
using KDataFinder.ConsoleApp.Implementation;
using OpenQA.Selenium;

namespace KDataFinder.ConsoleApp
{
    internal class KApplication
    {
        private readonly ILoginService _loginService;
        private readonly ITableDataObtainer _tableDataObtainer;
        private readonly ITableRowDetialObtainer _tableRowDetialObtainer;
        private readonly IWebDriver _webDriver;

        public KApplication(ILoginService loginService, ITableDataObtainer tableDataObtainer, IWebDriver webDriver, ITableRowDetialObtainer tableRowDetialObtainer)
        {
            _loginService = loginService;
            _tableDataObtainer = tableDataObtainer;
            _webDriver = webDriver;
            _tableRowDetialObtainer = tableRowDetialObtainer;
        }

        public void Run()
        {
            IOperationResult loginResult = _loginService.Login();
            if (!loginResult.IsSucceeded)
                throw new InvalidDataException(loginResult.AdditionalData?.ToString());
            var manager = new TaskManager<List<object>>(10,x => SaveData(x.res));
            _tableDataObtainer.Obtain(x =>
            {
                manager.AddTask(_tableRowDetialObtainer.Obtain,x).Wait();
            });
            _webDriver.Quit();
        }
        public void SaveData(List<object> list)
        {
            File.AppendAllText("data.csv", string.Join(" , ", list));
        }
    }
}
