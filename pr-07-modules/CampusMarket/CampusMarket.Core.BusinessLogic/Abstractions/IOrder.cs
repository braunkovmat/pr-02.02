using CampusMarket.Data.ApiClient.Models;

using CampusMarket.Core.BusinessLogic.Services;

namespace CampusMarket.Core.BusinessLogic.Abstractions;

// Интерфейс для UI: оформить заказ из корзины
public interface IOrder
{
    Task<CheckoutResponse> CheckoutAsync(CartService cart, string customerId, string paymentMethod = "card");
}
