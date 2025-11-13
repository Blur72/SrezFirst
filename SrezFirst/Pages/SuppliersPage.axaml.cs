using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using SrezFirst.Data;
using System.Linq;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

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

        private async void btnDelete_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var supplier = (sender as Button).Tag as Supplier;

            if (supplier != null)
            {
                var confirmBox = MessageBoxManager.GetMessageBoxStandard(
                    title: "Подтверждение удаления",
                    text: $"Вы уверены, что хотите удалить поставщика '{supplier.Name}'?",
                    ButtonEnum.YesNo,
                    Icon.Question);

                var result = await confirmBox.ShowAsync();

                if (result == ButtonResult.Yes)
                {
                    App.dbContext.Remove(supplier);
                    App.dbContext.SaveChanges();

                    var successBox = MessageBoxManager.GetMessageBoxStandard(
                        title: "Успешно",
                        text: "Поставщик успешно удален",
                        ButtonEnum.Ok,
                        Icon.Success);
                    await successBox.ShowAsync();

                    Refresh();
                }
            }
        }
    }
}