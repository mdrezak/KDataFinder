﻿namespace KDataFinder.ConsoleApp.Abstraction;

internal interface ILoginService
{
    string SuccessCookieName { get; }
    IOperationResult Login();
    IOperationResult LogOut();
}
