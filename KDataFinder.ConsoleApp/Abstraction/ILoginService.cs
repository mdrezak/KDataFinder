namespace KDataFinder.ConsoleApp.Abstraction;

internal interface ILoginService
{
    LoginResult Login();
    LoginResult LogOut();
}
