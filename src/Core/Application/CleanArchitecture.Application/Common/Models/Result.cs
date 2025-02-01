namespace CleanArchitecture.Application.Common.Models;

public class Result
{
    public bool IsSuccess { get; }
    public string[] Errors { get; }
    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, string[] errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public static Result Success() => new(true, Array.Empty<string>());

    public static Result Failure(params string[] errors) => new(false, errors);
}

public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T Value { get; }
    public string[] Errors { get; }

    private Result(bool isSuccess, T value, string[] errors)
    {
        IsSuccess = isSuccess;
        Value = value;
        Errors = errors;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, Array.Empty<string>());
    }

    public static Result<T> Failure(params string[] errors)
    {
        return new Result<T>(false, default!, errors);
    }
} 