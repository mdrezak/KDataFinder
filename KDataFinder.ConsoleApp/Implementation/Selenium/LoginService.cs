using KDataFinder.ConsoleApp.Abstraction;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;

namespace KDataFinder.ConsoleApp.Implementation.Selenium;

internal class LoginService : ILoginService
{
    private readonly IWebDriver _webDriver;
    private readonly ILogger<LoginService> _logger;
    private readonly LoginOptions _options;

    public LoginService(IWebDriver webDriver, Microsoft.Extensions.Options.IOptions<LoginOptions> options, ILogger<LoginService> logger)
    {
        _webDriver = webDriver;
        _options = options.Value;
        _logger = logger;
    }


    public IOperationResult Login()
    {
        _logger.LogTrace($"login proccess started with this option : \n {_options}");
        _webDriver.Navigate().GoToUrl(_options.LoginPath);
        _webDriver.FindElement(By.CssSelector(_options.BeforeLoginButton)).Click();
        var usernameInput = _webDriver.FindElement(By.Name(_options.UserNameElementName));
        var pwdInput = _webDriver.FindElement(By.Name(_options.PasswordElementName));
        usernameInput.SendKeys(_options.UserName);
        pwdInput.SendKeys(_options.Password);
        _webDriver.FindElement(By.CssSelector(_options.AfterLoginButton)).Click();
        _webDriver.FindElement(By.CssSelector(_options.BeforeLoginButton)).Click();
        IWebElement? errorElement = null;
        try
        {
            errorElement = _webDriver.FindElement(By.CssSelector(_options.ErrorElement));
        }
        catch { }
        if (errorElement?.Displayed == true && !string.IsNullOrWhiteSpace(errorElement?.Text))
            return OperationResult.NotFound(errorElement.Text);
        if (string.IsNullOrWhiteSpace(_webDriver.Manage().Cookies.GetCookieNamed(_options.SuccessCookie)?.Value))
            return OperationResult.Failure();
        return OperationResult.Succeeded();
    }

    public IOperationResult LogOut()
    {
        throw new NotImplementedException();
    }
}