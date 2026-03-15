namespace Autoservice.Application.DTOs.Common;

public class Result
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public string? Warning { get; }

    protected Result(bool isSuccess, string? error, string? warning = null)
    {
        IsSuccess = isSuccess;
        Error = error;
        Warning = warning;
    }

    public static Result Success() => new(true, null);
    public static Result Failure(string error) => new(false, error);
    public static Result SuccessWithWarning(string warning) => new(true, null, warning);
}

public class Result<T> : Result
{
    public T? Value { get; }

    private Result(bool isSuccess, T? value, string? error, string? warning = null)
        : base(isSuccess, error, warning)
        => Value = value;

    public static Result<T> Success(T value) => new(true, value, null);
    public static new Result<T> Failure(string error) => new(false, default, error);
    public static Result<T> SuccessWithWarning(T value, string warning) => new(true, value, null, warning);
}
