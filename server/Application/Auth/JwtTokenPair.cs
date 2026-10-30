namespace Application.Auth;

public record JwtTokenPair(string AccessToken, string RefreshToken);