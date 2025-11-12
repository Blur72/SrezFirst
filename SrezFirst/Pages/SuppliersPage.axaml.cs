using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using SrezFirst.Data;
using System.Linq;

namespace SrezFirst
{
    public partial class SuppliersPage : UserControl
    {
        public SuppliersPage()
        {
            InitializeComponent();
            Refresh();
        }

        private void Refresh()
        {
            var suppliers = App.dbContext.Suppliers
                .Include(s => s.SupplierType)
                .ToList();

            dgSuppliers.ItemsSource = suppliers;
        }

        private async void addSupplier_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var addWindow = new AddEditSupplierWindow();
            var parent = this.VisualRoot as Window;
            await addWindow.ShowDialog(parent);

            Refresh();
        }

        private async void btnEdit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var supplier = (sender as Button).Tag as Supplier;

            if (supplier != null)
            {
                var editWindow = new AddEditSupplierWindow(supplier);
                var parent = this.VisualRoot as Window;
                await editWindow.ShowDialog(parent);

                Refresh();
            }
        }

        private void btnDelete_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var supplier = (sender as Button).Tag as Supplier;

            if (supplier != null)
            {
                App.dbContext.Remove(supplier);
                App.dbContext.SaveChanges();
                Refresh();
            }
        }
    }
}