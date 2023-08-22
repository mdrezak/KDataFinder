namespace KDataFinder.ConsoleApp.Abstraction;

internal interface ITableRowDetialObtainer
{
    Task<Dictionary<string, object>> Obtain(TableRow row);
}
