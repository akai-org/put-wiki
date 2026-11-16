namespace Application.Auth;

public sealed record JwtTokenPair(string AccessToken, string RefreshToken);