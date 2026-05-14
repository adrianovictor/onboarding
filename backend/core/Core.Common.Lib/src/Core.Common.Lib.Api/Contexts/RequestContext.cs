using Core.Common.Lib.Application.Context;
using Microsoft.AspNetCore.Http;

namespace Core.Common.Lib.Api.Contexts;

public sealed class RequestContext(IHttpContextAccessor httpContextAccessor) : IRequestContext
{
    private HttpContext? Http => httpContextAccessor.HttpContext;

    public string CorrelationId =>
        Http?.Request.Headers["X-Correlation-Id"].FirstOrDefault()
        ?? Http?.TraceIdentifier
        ?? Guid.NewGuid().ToString();

    public string TraceIdentifier => Http?.TraceIdentifier ?? string.Empty;

    public string? UserId =>
        Http?.User?.FindFirst("sub")?.Value
        ?? Http?.User?.FindFirst("userId")?.Value;

    public bool IsAuthenticated =>
        Http?.User?.Identity?.IsAuthenticated ?? false;

    public IReadOnlyList<string> UserRoles =>
        Http?.User?.Claims
            .Where(c => c.Type is "role" or "roles")
            .Select(c => c.Value)
            .ToList() ?? [];
}