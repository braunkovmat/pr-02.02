using CampusMarket.Data.ApiClient.Models;

namespace CampusMarket.Core.BusinessLogic.Abstractions;

// Интерфейс для UI: получить товары каталога
public interface ICatalog
{
    Task<IReadOnlyList<CatalogItem>> GetItemsAsync(string? category = null);
}
