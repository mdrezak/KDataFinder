using Microsoft.Extensions.Logging;

namespace KDataFinder.ConsoleApp.Implementation.HtmlAgilityPack;

internal class BaseService<TChild>
where TChild : class
{
    protected readonly ILogger<TChild> _logger;

    public BaseService(ILogger<TChild> logger)
    {
        _logger = logger;
    }
}

internal class BaseService<TChild, TOptions> : BaseService<TChild>
where TOptions : class
where TChild : class
{
protected readonly TOptions _options;

public BaseService(ILogger<TChild> logger, Microsoft.Extensions.Options.IOptions<TOptions> option) : base(logger)
{
    _options = option.Value;
}
}
