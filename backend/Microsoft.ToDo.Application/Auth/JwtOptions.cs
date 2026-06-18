namespace Microsoft.ToDo.Application.Auth;

public sealed class JwtOptions
{
    public required string Issuer { get; set; }
    
    public required string Audience { get; set; }
    
    public required string SecretKey { get; set; }
    
    public int ExpirationHours { get; set; }
}