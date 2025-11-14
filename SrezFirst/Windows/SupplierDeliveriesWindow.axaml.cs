using Avalonia.Controls;
using Avalonia.Interactivity;
using SrezFirst.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SrezFirst;

public partial class SupplierDeliveriesWindow : Window
{
    public SupplierDeliveriesWindow(Supplier supplier)
    {
        InitializeComponent();

        titleSupplier.Text = $"Поставки поставщика: {supplier.Name}";

        var deliveries = App.dbContext.MaterialSuppliers
            .Include(ms => ms.Material)
            .Where(ms => ms.SupplierId == supplier.Id)
            .Select(ms => new
            {
                MaterialName = ms.Material.Name,
            })
            .ToList();

        dgDeliveries.ItemsSource = deliveries;
    }
    
    private void Close_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}