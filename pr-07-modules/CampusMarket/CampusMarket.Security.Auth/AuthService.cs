namespace CampusMarket.Security.Auth;

// Вход и роли пользователя. Токен выдаёт SSO кампуса, здесь он только принимается и сохраняется.
public class AuthService
{
    private readonly TokenStore _store;

    public AuthService(TokenStore store) => _store = store;

    public string Role { get; private set; } = "guest";
    public bool IsSignedIn => !string.IsNullOrEmpty(_store.GetToken());

    public void SignIn(string ssoToken, string role = "student")
    {
        if (string.IsNullOrWhiteSpace(ssoToken))
            throw new ArgumentException("Токен не может быть пустым", nameof(ssoToken));
        _store.Save(ssoToken);
        Role = role;
    }

    public void SignOut()
    {
        _store.Clear();
        Role = "guest";
    }
}
