namespace Infrastructure.Auth;

public class UsosOAuthSettings
{
    public string ConsumerKey { get; set; } = string.Empty;
    public string ConsumerSecret { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public string CallbackUrl { get; set; } = string.Empty;
    public string Scopes { get; set; } = string.Empty;
}