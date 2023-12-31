﻿#region OperationResult

using System.Net.NetworkInformation;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
#endregion


#region TableObtainer
public record TableObtainerOptions()
{
    public string[] TablePagesUrl { get; set; }
    public bool HasHeader { get; set; }
    public bool HasPagination { get; set; }
    public int StartPage { get; set; }
    public string TableRowsSelector { get; set; }
    /// <summary>
    /// en-GT : When this selector does not refer to any element, it means that the pagination is over
    /// fa-IR : زمانی که این انتخابگر به هیچ المانی ارجاع ندهد به این معنی است که صفحه بندی به پایان رسیده 
    /// </summary>
    public string NextPageButton { get; set; }
    public string WaitWhileShow { get; set; }
    public DetailObtainerOptions DetailObtainerOptions { get; set; }
}

public class DetailObtainerOptions
{
    public int OriginColumnIndex { get; set; }
    public int OriginColumnDataIndex { get; set; } = 0;
    public ObjectivesOfDataObtainer[] Objectives { get; set; }
    public class ObjectivesOfDataObtainer
    {
        public bool IsContinuedIndexedList { get; set; }
        public int Count { get; set; }
        public bool ImageToText { get; set; }
        public string TargetElement { get; set; }
        public string TargetAttribute { get; set; }
    }
}

public record TableRow(object[] Columns, int rowNumber)
{ }
#nullable restore
#endregion

#region DataStore
#nullable disable
public record DataStoreOptions()
{
    public enum OutputTypes
    {
        Csv,
    }
    public OutputTypes OutputType { get; set; }
    /// <summary>
    /// ایندکس بخشی از دیتا که بر اساس آن دسته بندی صورت میگیرد
    /// </summary>
    public int[] Grouping { get; set; }
    //public int[] DownloadableFiles { get; set; }
    //public int[] Files { get; set; }
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