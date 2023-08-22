namespace KDataFinder.ConsoleApp.Abstraction;

internal interface ILoginService
{
    IOperationResult Login();
    IOperationResult LogOut();
}
