using IronOcr;
using KDataFinder.ConsoleApp.Abstraction;
using KDataFinder.ConsoleApp.Implementation;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Text.Json;

namespace KDataFinder.ConsoleApp
{
    internal class KApplication
    {
        class TempData
        {
            public DateTime CreationDateTime { get; set; }
            public string Cookie { get; set; }
        }
        private const string TempKey = "TEMP-MrKDataFinder.tmp";
        private readonly ILoginService _loginService;
        private readonly ITableDataObtainer _tableDataObtainer;
        private readonly ITableRowDetialObtainer _tableRowDetialObtainer;
        private readonly IWebDriver _webDriver;
        private readonly IDownloadService _downloadService;
        private readonly IDataStore _dataStore;

        public KApplication(ILoginService loginService, ITableDataObtainer tableDataObtainer, IWebDriver webDriver, ITableRowDetialObtainer tableRowDetialObtainer, IDownloadService downloadService, IDataStore dataStore)
        {
            _loginService = loginService;
            _tableDataObtainer = tableDataObtainer;
            _webDriver = webDriver;
            _tableRowDetialObtainer = tableRowDetialObtainer;
            _downloadService = downloadService;
            _dataStore = dataStore;
        }

        public void Run()
        {
            if (!File.Exists(TempKey)) File.Create(TempKey).Close();
            var tempDataStr = File.ReadAllText(TempKey);
            var tempData = !string.IsNullOrEmpty(tempDataStr) ? JsonSerializer.Deserialize<TempData>(tempDataStr) : null;
            if (tempData == null || DateTime.Now.Subtract(tempData.CreationDateTime).TotalDays > 1)
            {
                IOperationResult loginResult = _loginService.Login();
                if (!loginResult.IsSucceeded)
                    throw new InvalidDataException(loginResult.AdditionalData?.ToString());
                tempData = new TempData() { CreationDateTime = DateTime.Now, Cookie = loginResult.AdditionalData?.ToString() ?? "" };
                File.WriteAllText(TempKey, JsonSerializer.Serialize(tempData));
            }
            _downloadService.SetAuthenticationCookie(_loginService.SuccessCookieName, tempData.Cookie);

            var manager = new TaskManager<List<object>>(10, x => SaveData(x.res));
            _tableDataObtainer.Obtain(x =>
            {
                if (string.IsNullOrWhiteSpace(_tableRowDetialObtainer.GetTableDetialAddress(x)))
                {
                    string contents = string.Join(", ", x.Columns).Replace("\r","").Replace("\n","");
                    File.AppendAllText("./Store/loss-data.csv", contents);
                }
                else
                    manager.AddTask(_tableRowDetialObtainer.Obtain, x).Wait();
            });
            
            _webDriver.Quit();

            //test 
            //var row = new TableRow(new[] { "https://senf.ir/Company/5677648/%D8%B3%D8%A7%D9%84%D9%86-%D8%B2%DB%8C%D8%A8%D8%A7%DB%8C%DB%8C-%D9%86%D9%88%DB%8C" }, 1);
            //SaveData(_tableRowDetialObtainer.Obtain(row).Result);
        }
        public void SaveData(List<object> list)
        {
            _dataStore.SaveObjects(list);
        }
    }
}
