using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using SrezFirst.Data;
using System;
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
            ClearErrors();
        }

        public AddEditMaterialWindow(Material material)
        {
            InitializeComponent();
            this.currentMaterial = material;
            DataContext = this.currentMaterial;
            LoadTypes();
            ClearErrors();

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

        private void ClearErrors()
        {
            nameError.Content = "";
            typeError.Content = "";
            priceError.Content = "";
            stockError.Content = "";
            minQuantityError.Content = "";
            packageError.Content = "";
            unitError.Content = "";
        }

        private void btnClose_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Close();
        }

        private async void btnSave_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            try
            {
                ClearErrors();
                bool isValid = true;

                if (string.IsNullOrWhiteSpace(nameMaterial.Text))
                {
                    nameError.Content = "Название материала не может быть пустым";
                    isValid = false;
                }

                if (!decimal.TryParse(priceMaterial.Text, out decimal price) || price <= 0)
                {
                    priceError.Content = "Цена должна быть положительным числом";
                    isValid = false;
                }

                if (!decimal.TryParse(stockMaterial.Text, out decimal stock) || stock < 0)
                {
                    stockError.Content = "Количество должно быть неотрицательным числом";
                    isValid = false;
                }

                if (!decimal.TryParse(minQuantityMaterial.Text, out decimal minQuantity) || minQuantity <= 0)
                {
                    minQuantityError.Content = "Минимальное количество должно быть положительным числом";
                    isValid = false;
                }

                if (!int.TryParse(packageMaterial.Text, out int package) || package <= 0)
                {
                    packageError.Content = "Количество в упаковке должно быть целым положительным числом";
                    isValid = false;
                }

                if (string.IsNullOrWhiteSpace(unitMaterial.Text))
                {
                    unitError.Content = "Единица измерения не может быть пустой";
                    isValid = false;
                }

                if (typeMaterial.SelectedItem == null)
                {
                    typeError.Content = "Необходимо выбрать тип материала";
                    isValid = false;
                }

                if (!isValid) return;

                currentMaterial.Name = nameMaterial.Text!;
                currentMaterial.UnitPrice = price;
                currentMaterial.QuantityInStock = stock;
                currentMaterial.MinimumQuantity = minQuantity;
                currentMaterial.QuantityInPackage = package;
                currentMaterial.UnitOfMeasure = unitMaterial.Text!;

                if (typeMaterial.SelectedItem is MaterialType selectedType)
                {
                    currentMaterial.MaterialTypeId = selectedType.Id;
                }

                if (currentMaterial.Id == 0)
                {
                    App.dbContext.Materials.Add(currentMaterial);
                }

                App.dbContext.SaveChanges();

                var successBox = MessageBoxManager.GetMessageBoxStandard(
                    title: "Успешно",
                    text: currentMaterial.Id == 0 ? "Материал успешно добавлен" : "Материал успешно изменен",
                    ButtonEnum.Ok);
                await successBox.ShowAsync();

                Close();
            }
            catch (Exception ex)
            {
                var errorBox = MessageBoxManager.GetMessageBoxStandard(
                    title: "Ошибка",
                    text: $"Не удалось сохранить материал: {ex.Message}",
                    ButtonEnum.Ok);
                await errorBox.ShowAsync();
            }
        }
    }
}