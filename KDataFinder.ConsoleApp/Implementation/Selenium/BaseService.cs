using Microsoft.Extensions.Logging;
using OpenQA.Selenium;

namespace KDataFinder.ConsoleApp.Implementation.Selenium;

internal class BaseService<TChild>
    where TChild : class
{
    protected readonly IWebDriver _webDriver;
    protected readonly ILogger<TChild> _logger;

    public BaseService(IWebDriver webDriver, ILogger<TChild> logger)
    {
        _webDriver = webDriver;
        _logger = logger;
    }
}

internal class BaseService<TChild,TOptions> : BaseService<TChild>
    where TOptions : class
    where TChild : class
{
    protected readonly TOptions _options;

    public BaseService(IWebDriver webDriver, ILogger<TChild> logger, Microsoft.Extensions.Options.IOptions<TOptions> option) : base(webDriver, logger)
    {
        _options = option.Value;
    }
}
