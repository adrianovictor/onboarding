namespace Core.Common.Lib.Api.Options.Jwt;

public class JwtOptions
{
    public string Issuer { get; set; } = string.Empty;
    public List<string> IgnoreRoutes { get; set; } = [];

    public bool IsValid() =>
        !string.IsNullOrWhiteSpace(Issuer) && IgnoreRoutes.Count > 0;
}
