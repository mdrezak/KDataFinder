#region OperationResult
public interface IOperationResult<TAdditionalData>
{
    public bool IsSucceeded { get; }
}
public record OperationResult(bool IsSucceeded, object? AdditionalData) : IOperationResult<object?>
{
    public OperationResult(bool isSucceeded) : this(isSucceeded,null)
    {
        
    }
}
public record OperationResult<TAdditionalData> : OperationResult, IOperationResult<TAdditionalData>
{
    public OperationResult(bool isSucceeded, TAdditionalData additionalData) : base(isSucceeded, additionalData)
    {
        this.AdditionalData = additionalData;
    }

    public new TAdditionalData AdditionalData { get; }
}
#endregion


#region Login
public record LoginResult : OperationResult
{
    public LoginResult(bool IsSucceeded) : base(IsSucceeded, null)
    {
    }
}
public record LogOutResult : OperationResult
{
    public LogOutResult(bool IsSucceeded) : base(IsSucceeded, null)
    {
    }
}
#endregion