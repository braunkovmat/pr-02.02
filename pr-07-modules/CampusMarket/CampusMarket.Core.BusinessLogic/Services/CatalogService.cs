using CampusMarket.Core.BusinessLogic.Abstractions;
using CampusMarket.Data.ApiClient;
using CampusMarket.Data.ApiClient.Models;

namespace CampusMarket.Core.BusinessLogic.Services;

public class CatalogService : ICatalog
{
    private readonly IApiClient _api;

    public CatalogService(IApiClient api) => _api = api;

    public async Task<IReadOnlyList<CatalogItem>> GetItemsAsync(string? category = null)
        => (await _api.GetCatalogItemsAsync(category)).Items;
}
