namespace CampusMarket.Security.Auth;

// Хранилище токена доступа (в памяти на время работы приложения)
public class TokenStore : ITokenProvider
{
    private string? _token;

    public string? GetToken() => _token;
    public void Save(string token) => _token = token;
    public void Clear() => _token = null;
}
