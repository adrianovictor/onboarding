namespace Core.Common.Lib.Application.Exceptions;

public class CustomException : Exception
{
    public string ErrorCode { get; }
    public string Description { get; }

    public CustomException(string errorCode, string description, string message) : base(message)
    {
        ErrorCode = errorCode;
        Description = description;
    }

    public CustomException(string errorCode, string description, string message, Exception innerException) 
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        Description = description;
    }
}
