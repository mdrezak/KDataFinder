#region OperationResult
using Microsoft.Extensions.Options;
using OpenQA.Selenium;

public interface IOperationResult : IOperationResult<object?> { }
public interface IOperationResult<TAdditionalData>
{
    public bool IsSucceeded { get; }
    public FailureType FailureType { get; }
    public TAdditionalData AdditionalData { get; }
}

public enum FailureType
{
    None,
    Conflict,
    NotFound,
    Failure,
}

public record OperationResult(bool IsSucceeded, object? AdditionalData, FailureType FailureType) : IOperationResult
{
    public static IOperationResult Succeeded(object? AdditionalData = null) => new OperationResult(true, AdditionalData, FailureType.None);
    public static IOperationResult Conflict(object? AdditionalData = null) => new OperationResult(false, AdditionalData, FailureType.Conflict);
    public static IOperationResult NotFound(object? AdditionalData = null) => new OperationResult(false, AdditionalData, FailureType.NotFound);
    public static IOperationResult Failure(object? AdditionalData = null) => new OperationResult(false, AdditionalData, FailureType.Failure);
}
public record OperationResult<TAdditionalData> : OperationResult, IOperationResult<TAdditionalData>
{
    public OperationResult(bool isSucceeded, TAdditionalData additionalData, FailureType failureType) : base(isSucceeded, additionalData, failureType)
    {
        this.AdditionalData = additionalData;
    }

    public new TAdditionalData AdditionalData { get; }
}
#endregion


#region Login
#nullable disable
public record LoginOptions
{
    public string LoginPath { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string UserNameElementName { get; set; }
    public string PasswordElementName { get; set; }
    public string BeforeLoginButton { get; set; }
    public string AfterLoginButton { get; set; }
    public string ErrorElement { get; set; }
    public string SuccessCookie { get; set; }
}
#nullable restore
#endregion

//public enum NavigationMode
//{
//    Refresh = 0,
//    Back = 1,
//    Forward = 2,
//    Url = 3,
//}

//public record Navigation(NavigationMode Mode,string Url);
//class Action
//{
//    public Navigation? Navigation { get; set; }
//    public Navigation? Navigation { get; set; }
//}