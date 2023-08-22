namespace KDataFinder.ConsoleApp.Abstraction;

internal interface ITableDataObtainer
{
    Task Obtain(Action<TableRow> onDataObtained);
}
