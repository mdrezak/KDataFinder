using OpenQA.Selenium;

namespace KDataFinder.ConsoleApp.Implementation.Selenium.Tools;
public static class WebElementExtensions
{
    public static string GetRelevantInfo(this IWebElement element, (string TagName, string TagAttr)[] relevant,string seperator = "|||")
    {
        foreach (var r in relevant)
        {
            try
            {
                var Tags = element.FindElements(By.TagName(r.TagName));
                if (Tags.Count > 0)
                {
                    return string.Join(seperator,Tags.Select(x => x.GetAttribute(r.TagAttr)));
                }
            }
            catch { }
        }
        return element.Text;
    }
}

