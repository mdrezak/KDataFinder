using KDataFinder.ConsoleApp.Abstraction;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;

namespace KDataFinder.ConsoleApp.Implementation.Selenium;

internal class TableRowDetialObtainer : BaseService<TableRowDetialObtainer, TableObtainerOptions>, ITableRowDetialObtainer
{
    private readonly DetailObtainerOptions options;
    private readonly IImageToTextService _imageToTextService;
    public TableRowDetialObtainer(IWebDriver webDriver, ILogger<TableRowDetialObtainer> logger, IOptions<TableObtainerOptions> option, IImageToTextService imageToTextService) : base(webDriver, logger, option)
    {
        this.options = option.Value.DetailObtainerOptions;
        _imageToTextService = imageToTextService;
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
                    var img = ((ITakesScreenshot)_webDriver.FindElement(By.CssSelector(objective.TargetElement))).GetScreenshot();
                    string fileName = $"{Random.Shared.Next(0, 100)}.png";
                    img.SaveAsFile(fileName);
                    Result.Add(await _imageToTextService.ImageToText(fileName));
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
