using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CampusMarket.Core.BusinessLogic.Abstractions;
using CampusMarket.Core.BusinessLogic.Services;
using CampusMarket.Data.ApiClient.Models;
using CampusMarket.Security.Auth;

namespace CampusMarket.UI.Desktop.ViewModels;

// Модель представления: связывает окна с бизнес-логикой, сама не знает про HTTP
public class CatalogViewModel : INotifyPropertyChanged
{
    private readonly ICatalog _catalog;
    private readonly IOrder _orders;
    private readonly CartService _cart;
    private readonly AuthService _auth;
    private CatalogItem? _selectedItem;
    private string _status = "Войдите и обновите каталог";

    public CatalogViewModel(ICatalog catalog, IOrder orders, CartService cart, AuthService auth)
    {
        _catalog = catalog;
        _orders = orders;
        _cart = cart;
        _auth = auth;

        SignInCommand = new RelayCommand(_ => { _auth.SignIn("campus-sso-token"); Status = "Вход выполнен"; });
        RefreshCommand = new RelayCommand(async _ => await LoadAsync());
        AddToCartCommand = new RelayCommand(_ => AddSelected(), _ => SelectedItem != null);
        CheckoutCommand = new RelayCommand(async _ => await CheckoutAsync(), _ => !_cart.IsEmpty);
    }

    public ObservableCollection<CatalogItem> Items { get; } = new();
    public ObservableCollection<string> CartLines { get; } = new();

    public CatalogItem? SelectedItem
    {
        get => _selectedItem;
        set { _selectedItem = value; OnPropertyChanged(); }
    }

    public string Status
    {
        get => _status;
        private set { _status = value; OnPropertyChanged(); }
    }

    public double CartTotal => _cart.Total;

    public ICommand SignInCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand AddToCartCommand { get; }
    public ICommand CheckoutCommand { get; }

    private async Task LoadAsync()
    {
        try
        {
            Items.Clear();
            foreach (var item in await _catalog.GetItemsAsync()) Items.Add(item);
            Status = $"Товаров в каталоге: {Items.Count}";
        }
        catch (Exception ex) { Status = $"Ошибка: {ex.Message}"; }
    }

    private void AddSelected()
    {
        try
        {
            _cart.Add(SelectedItem!);
            RefreshCart();
            Status = $"В корзину добавлено: {SelectedItem!.Name}";
        }
        catch (InvalidOperationException ex) { Status = ex.Message; }
    }

    private async Task CheckoutAsync()
    {
        try
        {
            var order = await _orders.CheckoutAsync(_cart, "student-14239", "campus_card");
            RefreshCart();
            Status = $"Заказ {order.OrderId}: {order.Status}";
        }
        catch (Exception ex) { Status = $"Ошибка: {ex.Message}"; }
    }

    private void RefreshCart()
    {
        CartLines.Clear();
        foreach (var (item, quantity) in _cart.Lines)
            CartLines.Add($"{item.Name} × {quantity} = {item.Price * quantity:F2} руб.");
        OnPropertyChanged(nameof(CartTotal));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
