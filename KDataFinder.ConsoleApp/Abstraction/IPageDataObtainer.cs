namespace KDataFinder.ConsoleApp.Abstraction;

internal interface ITableRowDetialObtainer
{
    Task<List<object>> Obtain(TableRow row);
}
