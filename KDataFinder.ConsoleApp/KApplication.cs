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
        private readonly IDownloadService _downloadService;

        public KApplication(ILoginService loginService, ITableDataObtainer tableDataObtainer, IWebDriver webDriver, ITableRowDetialObtainer tableRowDetialObtainer, IDownloadService downloadService)
        {
            _loginService = loginService;
            _tableDataObtainer = tableDataObtainer;
            _webDriver = webDriver;
            _tableRowDetialObtainer = tableRowDetialObtainer;
            _downloadService = downloadService;
        }

        public void Run()
        {
            IOperationResult loginResult = _loginService.Login();
            if (!loginResult.IsSucceeded)
                throw new InvalidDataException(loginResult.AdditionalData?.ToString());
            _downloadService.SetAuthenticationCookie(_loginService.SuccessCookieName, loginResult.AdditionalData?.ToString() ?? "");
            //var manager = new TaskManager<List<object>>(10, x => SaveData(x.res));
            //_tableDataObtainer.Obtain(x =>
            //{
            //    manager.AddTask(_tableRowDetialObtainer.Obtain, x).Wait();
            //});
            //test 
            var row = new TableRow(new[] { "https://senf.ir/Company/5677648/%D8%B3%D8%A7%D9%84%D9%86-%D8%B2%DB%8C%D8%A8%D8%A7%DB%8C%DB%8C-%D9%86%D9%88%DB%8C" }, 1);
            _tableRowDetialObtainer.Obtain(row).Wait();
            _webDriver.Quit();
        }
        public void SaveData(List<object> list)
        {
            File.AppendAllText("data.csv", string.Join(" , ", list));
        }
    }
}
