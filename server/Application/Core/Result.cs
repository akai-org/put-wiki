using System;

namespace Application.Core;

public class Result
{
    public bool IsSuccess { get; set; }
    public string? Error { get; set; }
    public int Code { get; set; }

    public static Result<T> Success<T>(T value) => new() { IsSuccess = true, Value = value };

    public static Result<T> Failure<T>(string error, int code) =>
        new()
        {
            IsSuccess = false,
            Error = error,
            Code = code,
        };
}

public class Result<T> : Result
{
    public T? Value { get; set; }
}