using KDataFinder.ConsoleApp.Abstraction;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;

namespace KDataFinder.ConsoleApp.Implementation.Selenium;

internal class LoginService : BaseService<LoginService, LoginOptions>, ILoginService
{
    public LoginService(IWebDriver webDriver, Microsoft.Extensions.Options.IOptions<LoginOptions> options, ILogger<LoginService> logger)
    : base(webDriver, logger, options)
    { }


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
        IWebElement? errorElement = null;
        try
        {
            _webDriver.FindElement(By.CssSelector(_options.BeforeLoginButton)).Click();
            errorElement = _webDriver.FindElement(By.CssSelector(_options.ErrorElement));
        }
        catch { }
        if (errorElement?.Displayed == true && !string.IsNullOrWhiteSpace(errorElement?.Text))
            return OperationResult.NotFound(errorElement.Text);
        if (_options.SuccessCookie != string.Empty && string.IsNullOrWhiteSpace(_webDriver.Manage().Cookies.GetCookieNamed(_options.SuccessCookie)?.Value))
            return OperationResult.Failure();
        return OperationResult.Succeeded();
    }

    public IOperationResult LogOut()
    {
        throw new NotImplementedException();
    }
}