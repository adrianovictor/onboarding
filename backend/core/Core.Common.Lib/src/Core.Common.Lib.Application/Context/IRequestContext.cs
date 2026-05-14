namespace Core.Common.Lib.Application.Context;

public interface IRequestContext
{
    string CorrelationId { get; }
    string TraceIdentifier { get; }
    string? UserId { get; }
    
    bool IsAuthenticated { get; }

    IReadOnlyList<string> UserRoles { get; }
}

