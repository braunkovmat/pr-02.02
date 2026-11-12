using System.Windows;

namespace CampusMarket.UI.Desktop.Views;

public partial class CatalogView : Window
{
    public CatalogView() => InitializeComponent();

    private void OpenCart_Click(object sender, RoutedEventArgs e)
        => new CartView { DataContext = DataContext, Owner = this }.Show();
}
