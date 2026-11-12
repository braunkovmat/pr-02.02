using System.Net.Http;
using System.Windows;
using CampusMarket.Core.BusinessLogic.Services;
using CampusMarket.Data.ApiClient;
using CampusMarket.Security.Auth;
using CampusMarket.UI.Desktop.ViewModels;
using CampusMarket.UI.Desktop.Views;

namespace CampusMarket.UI.Desktop;

public partial class App : Application
{
    // Здесь модули соединяются между собой (интерфейс сам ничего не создаёт)
    private void OnStartup(object sender, StartupEventArgs e)
    {
        // Адрес шлюза можно переопределить: CAMPUS_API_URL
        var baseUrl = Environment.GetEnvironmentVariable("CAMPUS_API_URL") ?? "http://localhost:5080/api/v1/";

        var tokens = new TokenStore();
        var auth = new AuthService(tokens);
        var api = new CampusApiClient(new HttpClient { BaseAddress = new Uri(baseUrl) }, tokens);
        var cart = new CartService();

        var viewModel = new CatalogViewModel(new CatalogService(api), new OrderService(api, auth), cart, auth);
        new CatalogView { DataContext = viewModel }.Show();
    }
}
