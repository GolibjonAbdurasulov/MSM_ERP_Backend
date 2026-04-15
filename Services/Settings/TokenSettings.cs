namespace Services.Settings;

public class TokenSettings
{
    public string JwtKey { get; init; }
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string EncryptionKey { get; init; }
    public int ExpireSeconds { get; init; }
}