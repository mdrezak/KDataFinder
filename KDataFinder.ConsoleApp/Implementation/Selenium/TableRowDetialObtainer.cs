using IronOcr;
using KDataFinder.ConsoleApp.Abstraction;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;

namespace KDataFinder.ConsoleApp.Implementation.Selenium;

internal class TableRowDetialObtainer : BaseService<TableRowDetialObtainer, TableObtainerOptions>, ITableRowDetialObtainer
{
    private readonly DetailObtainerOptions options;
    public TableRowDetialObtainer(IWebDriver webDriver, ILogger<TableRowDetialObtainer> logger, IOptions<TableObtainerOptions> option) : base(webDriver, logger, option)
    {
        this.options = option.Value.DetailObtainerOptions;
    }

    public async Task<List<object>> Obtain(TableRow row)
    {
        List<object> Result = new();
        _webDriver.SwitchTo().NewWindow(WindowType.Tab);
        _webDriver.Navigate().GoToUrl(row.Columns[options.OriginColumnIndex].ToString()!.Split("|||")[options.OriginColumnDataIndex]);
        for (int i = 0; i < options.Objectives.Length; i++)
        {
            try
            {
                var objective = options.Objectives[i];
                if (objective.IsContinuedIndexedList)
                {
                    for (int y = 0; y < objective.Count; y++)
                    {
                        if (objective.ImageToText)
                            throw new NotImplementedException("Continued indexed list does not support for ImageToText objectives.");
                        Result.Add(
                            string.IsNullOrEmpty(objective.TargetAttribute) ?
                            _webDriver.FindElement(By.CssSelector(objective.TargetElement.Replace("{0}", y.ToString()))).Text
                            :
                            _webDriver.FindElement(By.CssSelector(objective.TargetElement.Replace("{0}", y.ToString()))).GetAttribute(objective.TargetAttribute)
                        );
                    }
                }
                else if (objective.ImageToText)
                {
                    var Ocr = new IronTesseract();
                    Ocr.Language = OcrLanguage.Arabic;
                    var img = ((ITakesScreenshot)_webDriver.FindElement(By.CssSelector(objective.TargetElement))).GetScreenshot();
                    img.SaveAsFile($"{Random.Shared.Next(0, 100)}.png");
                    using (var Input = new OcrInput(img.AsByteArray))
                        Result.Add((await Ocr.ReadAsync(Input)).Text);
                }
                else
                {
                    Result.Add(
                        string.IsNullOrEmpty(objective.TargetAttribute) ?
                        _webDriver.FindElement(By.CssSelector(objective.TargetElement)).Text
                        :
                        _webDriver.FindElement(By.CssSelector(objective.TargetElement)).GetAttribute(objective.TargetAttribute)
                    );
                }
            }
            catch { }
        }
        _webDriver.Close();
        return (Result);
    }
}
