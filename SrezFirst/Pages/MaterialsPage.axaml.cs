using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using SrezFirst.Data;
using System.Linq;

namespace SrezFirst
{
    public partial class MaterialsPage : UserControl
    {
        public MaterialsPage()
        {
            InitializeComponent();
            Refresh();
        }

        private void Refresh()
        {
            var materials = App.dbContext.Materials
                .Include(m => m.MaterialType)
                .ToList()
                .Select(m => new
                {
                    m.Id, // Важно: сохраняем ID
                    m.Name,
                    MaterialTypeName = m.MaterialType?.Name ?? "None",
                    m.UnitPrice,
                    m.QuantityInStock,
                    m.MinimumQuantity,
                    m.QuantityInPackage,
                    m.UnitOfMeasure,
                    PurchaseCost = CalculatePurchaseCost(m)
                })
                .ToList();

            dgMaterials.ItemsSource = materials;
        }

        private string CalculatePurchaseCost(Material material)
        {
            if (material.QuantityInStock >= material.MinimumQuantity)
                return "Not needed";

            var deficit = material.MinimumQuantity - material.QuantityInStock;
            var packagesNeeded = (int)System.Math.Ceiling(deficit / material.QuantityInPackage);
            var quantityToBuy = packagesNeeded * material.QuantityInPackage;
            var cost = quantityToBuy * material.UnitPrice;

            return $"{cost:F2} руб.";
        }

        private async void addMaterial_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var addWindow = new AddEditMaterialWindow();
            var parent = this.VisualRoot as Window;
            await addWindow.ShowDialog(parent);

            Refresh();
        }

        private async void btnEdit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var anonymousObject = (sender as Button).Tag;

            var idProperty = anonymousObject.GetType().GetProperty("Id");
            var id = (int)idProperty.GetValue(anonymousObject);

            var material = App.dbContext.Materials
                .Include(m => m.MaterialType)
                .FirstOrDefault(m => m.Id == id);

            if (material != null)
            {
                var editWindow = new AddEditMaterialWindow(material);
                var parent = this.VisualRoot as Window;
                await editWindow.ShowDialog(parent);

                Refresh();
            }
        }

        private void btnDelete_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var anonymousObject = (sender as Button).Tag;
            var idProperty = anonymousObject.GetType().GetProperty("Id");
            var id = (int)idProperty.GetValue(anonymousObject);

            var material = App.dbContext.Materials.FirstOrDefault(m => m.Id == id);

            if (material != null)
            {
                App.dbContext.Remove(material);
                App.dbContext.SaveChanges();
                Refresh();
            }
        }
    }
}