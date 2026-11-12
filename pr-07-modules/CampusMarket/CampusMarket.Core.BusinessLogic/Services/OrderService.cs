using CampusMarket.Core.BusinessLogic.Abstractions;
using CampusMarket.Data.ApiClient;
using CampusMarket.Data.ApiClient.Models;
using CampusMarket.Security.Auth;

namespace CampusMarket.Core.BusinessLogic.Services;

// Оформление заказа: проверяет вход и корзину, затем обращается к шлюзу
public class OrderService : IOrder
{
    private readonly IApiClient _api;
    private readonly AuthService _auth;

    public OrderService(IApiClient api, AuthService auth)
    {
        _api = api;
        _auth = auth;
    }

    public async Task<CheckoutResponse> CheckoutAsync(CartService cart, string customerId, string paymentMethod = "card")
    {
        if (!_auth.IsSignedIn) throw new InvalidOperationException("Для оформления заказа нужно войти");
        if (cart.IsEmpty) throw new InvalidOperationException("Корзина пуста");

        var request = new CheckoutRequest { CustomerId = customerId, PaymentMethod = paymentMethod };
        foreach (var (item, quantity) in cart.Lines)
            request.Lines.Add(new OrderLine { ItemId = item.Id, Quantity = quantity });

        var response = await _api.CheckoutAsync(request);
        cart.Clear();
        return response;
    }
}
