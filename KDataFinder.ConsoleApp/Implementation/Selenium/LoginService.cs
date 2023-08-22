using KDataFinder.ConsoleApp.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;

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
        var usernameInput = _webDriver.FindElement(By.Name(_options.UserNameElementName));
        var pwdInput = _webDriver.FindElement(By.Name(_options.PasswordElementName));
        usernameInput.SendKeys(_options.UserName);
        pwdInput.SendKeys(_options.Password);
        var sumbitBtn = _webDriver.FindElement(By.CssSelector(_options.LoginButton));
        sumbitBtn.Click();
        return OperationResult.Succeeded();
    }

    public IOperationResult LogOut()
    {
        throw new NotImplementedException();
    }
}