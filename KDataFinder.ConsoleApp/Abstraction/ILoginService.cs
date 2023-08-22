namespace KDataFinder.ConsoleApp.Abstraction;

internal interface ILoginService
{
    IOperationResult Login();
    IOperationResult LogOut();
}

internal interface IPageDataObtainer<TData,TNotCastedData>
{
    public Func<TNotCastedData, TData> DataCaster { get; } 
    Task Obtain(string Uri);
}
