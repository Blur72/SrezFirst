using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SrezFirst.Data;
using System.Linq;

namespace SrezFirst
{
    public partial class AddEditMaterialWindow : Window
    {
        Material currentMaterial = new Material();

        public AddEditMaterialWindow()
        {
            InitializeComponent();
            LoadTypes();
        }

        public AddEditMaterialWindow(Material material)
        {
            InitializeComponent();
            this.currentMaterial = material;
            DataContext = this.currentMaterial;
            LoadTypes();

            if (material.MaterialTypeId.HasValue)
            {
                var type = App.dbContext.MaterialTypes.FirstOrDefault(t => t.Id == material.MaterialTypeId.Value);
                typeMaterial.SelectedItem = type;
            }
        }

        private void LoadTypes()
        {
            var types = App.dbContext.MaterialTypes.ToList();
            typeMaterial.ItemsSource = types;
        }

        private void btnClose_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            // Обновляем данные из полей
            currentMaterial.Name = nameMaterial.Text!;
            currentMaterial.UnitPrice = decimal.Parse(priceMaterial.Text);
            currentMaterial.QuantityInStock = decimal.Parse(stockMaterial.Text);
            currentMaterial.MinimumQuantity = decimal.Parse(minQuantityMaterial.Text);
            currentMaterial.QuantityInPackage = int.Parse(packageMaterial.Text);
            currentMaterial.UnitOfMeasure = unitMaterial.Text!;

            // Устанавливаем тип материала
            if (typeMaterial.SelectedItem is MaterialType selectedType)
            {
                currentMaterial.MaterialTypeId = selectedType.Id;
            }

            // Сохраняем
            if (currentMaterial.Id == 0) // Новый материал
            {
                App.dbContext.Materials.Add(currentMaterial);
            }

            App.dbContext.SaveChanges();
            Close();
        }
    }
}