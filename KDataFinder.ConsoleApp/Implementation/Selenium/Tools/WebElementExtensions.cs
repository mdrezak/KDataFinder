using OpenQA.Selenium;

namespace KDataFinder.ConsoleApp.Implementation.Selenium.Tools;
public static class WebElementExtensions
{
    public static string GetRelevantInfo(this IWebElement element, (string TagName, string TagAttr)[] relevant)
    {
        foreach (var r in relevant)
        {
            try
            {
                var Tag = element.FindElement(By.TagName(r.TagName));
                if (Tag != null)
                {
                    return Tag.GetAttribute(r.TagAttr);
                }
            }
            catch { }
        }
        return element.Text;
    }
}

