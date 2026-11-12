namespace CampusMarket.Security.Auth;

// Интерфейс, через который сетевой слой получает токен (см. UML-диаграмму)
public interface ITokenProvider
{
    string? GetToken();
}
