using CampusMarket.Data.ApiClient.Models;

namespace CampusMarket.Core.BusinessLogic.Services;

// Корзина: добавление товаров, расчёт суммы
public class CartService
{
    private readonly Dictionary<long, (CatalogItem Item, int Quantity)> _lines = new();

    public IReadOnlyCollection<(CatalogItem Item, int Quantity)> Lines => _lines.Values;
    public double Total => _lines.Values.Sum(l => l.Item.Price * l.Quantity);
    public bool IsEmpty => _lines.Count == 0;

    public void Add(CatalogItem item, int quantity = 1)
    {
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Количество должно быть больше нуля");
        var current = _lines.TryGetValue(item.Id, out var line) ? line.Quantity : 0;
        if (current + quantity > item.Stock)
            throw new InvalidOperationException($"Недостаточно товара «{item.Name}»: в наличии {item.Stock}");
        _lines[item.Id] = (item, current + quantity);
    }

    public void Remove(long itemId) => _lines.Remove(itemId);
    public void Clear() => _lines.Clear();
}
