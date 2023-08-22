using KDataFinder.ConsoleApp.Abstraction;
using KDataFinder.ConsoleApp.Implementation.Selenium.Tools;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;

namespace KDataFinder.ConsoleApp.Implementation.Selenium
{
    internal class TableDataObtainer : BaseService<TableDataObtainer, TableObtainerOptions>, ITableDataObtainer
    {
        public TableDataObtainer(IWebDriver webDriver, ILogger<TableDataObtainer> logger, IOptions<TableObtainerOptions> option) : base(webDriver, logger, option)
        {
        }

        public Task Obtain(Action<TableRow> onDataObtained)
        {
            foreach (var url in _options.TablePagesUrl)
            {
                _webDriver.Navigate().GoToUrl(url);
                ObtainRows(onDataObtained);
            }
            return Task.CompletedTask;
        }
        private void ObtainRows(Action<TableRow> onDataObtained)
        {
            var tbody = _webDriver.FindElement(By.CssSelector(_options.TableRowsContainer));
            var currentPageRows = tbody.FindElements(By.Name("tr"));
            for (int i = 0; i < currentPageRows.Count; i++)
            {
                if (_options.HasHeader && i == 0) continue;
                var row = currentPageRows[i];
                var allRowTd = row.FindElements(By.Name("td"));
                var rowData = new TableRow(new object[allRowTd.Count],i + 1);
                for (int y = 0; y < allRowTd.Count; y++)
                {
                    var td = allRowTd[y];
                    rowData.Columns[y] = td.GetRelevantInfo(new[]
                    {
                        ("a","href"),
                        ("img","src"),
                    });
                    onDataObtained.Invoke(rowData);
                }
                if (_options.HasPagination)
                {
                    try
                    {
                        _webDriver.FindElement(By.CssSelector(_options.NextPageButton)).Click();
                        ObtainRows(onDataObtained);
                    }
                    catch { }
                }
            }
        }
    }
}
