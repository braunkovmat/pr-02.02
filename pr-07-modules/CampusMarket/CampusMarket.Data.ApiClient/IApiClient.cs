using CampusMarket.Data.ApiClient.Models;

namespace CampusMarket.Data.ApiClient;

// Интерфейс сетевого слоя: через него бизнес-логика обращается к шлюзу (см. UML-диаграмму)
public interface IApiClient
{
    Task<CatalogItemList> GetCatalogItemsAsync(string? category = null);
    Task<CheckoutResponse> CheckoutAsync(CheckoutRequest request);
}
