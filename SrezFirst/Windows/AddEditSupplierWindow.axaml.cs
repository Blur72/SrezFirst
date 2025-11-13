using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SrezFirst.Data;
using System;
using System.Linq;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace SrezFirst
{
    public partial class AddEditSupplierWindow : Window
    {
        Supplier currentSupplier = new Supplier();

        public AddEditSupplierWindow()
        {
            InitializeComponent();
            LoadTypes();
            ClearErrors();
        }

        public AddEditSupplierWindow(Supplier supplier)
        {
            InitializeComponent();
            this.currentSupplier = supplier;
            DataContext = this.currentSupplier;
            LoadTypes();
            ClearErrors();

            if (supplier.SupplierTypeId.HasValue)
            {
                var type = App.dbContext.SupplierTypes.FirstOrDefault(t => t.Id == supplier.SupplierTypeId.Value);
                typeSupplier.SelectedItem = type;
            }
        }

        private void LoadTypes()
        {
            var types = App.dbContext.SupplierTypes.ToList();
            typeSupplier.ItemsSource = types;
        }

        private void ClearErrors()
        {
            nameError.Content = "";
            typeError.Content = "";
            innError.Content = "";
            ratingError.Content = "";
            dateError.Content = "";
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

                if (string.IsNullOrWhiteSpace(nameSupplier.Text))
                {
                    nameError.Content = "Название поставщика не может быть пустым";
                    isValid = false;
                }

                if (string.IsNullOrWhiteSpace(innSupplier.Text))
                {
                    innError.Content = "ИНН не может быть пустым";
                    isValid = false;
                }

                if (!int.TryParse(ratingSupplier.Text, out int rating) || rating < 0 || rating > 10)
                {
                    ratingError.Content = "Рейтинг должен быть целым числом от 0 до 10";
                    isValid = false;
                }

                if (string.IsNullOrWhiteSpace(dateSupplier.Text))
                {
                    dateError.Content = "Дата начала не может быть пустой";
                    isValid = false;
                }
                else if (!DateOnly.TryParse(dateSupplier.Text, out DateOnly startDate))
                {
                    dateError.Content = "Неверный формат даты. Используйте: ГГГГ-ММ-ДД";
                    isValid = false;
                }

                if (typeSupplier.SelectedItem == null)
                {
                    typeError.Content = "Необходимо выбрать тип поставщика";
                    isValid = false;
                }

                if (!isValid) return;

                currentSupplier.Name = nameSupplier.Text!;
                currentSupplier.Inn = innSupplier.Text!;
                currentSupplier.Rating = rating;
                currentSupplier.StartDate = DateOnly.Parse(dateSupplier.Text);

                if (typeSupplier.SelectedItem is SupplierType selectedType)
                {
                    currentSupplier.SupplierTypeId = selectedType.Id;
                }

                if (currentSupplier.Id == 0)
                {
                    App.dbContext.Suppliers.Add(currentSupplier);
                }

                App.dbContext.SaveChanges();

                var successBox = MessageBoxManager.GetMessageBoxStandard(
                    title: "Успешно",
                    text: currentSupplier.Id == 0 ? "Поставщик успешно добавлен" : "Поставщик успешно изменен",
                    ButtonEnum.Ok);
                await successBox.ShowAsync();

                Close();
            }
            catch (Exception ex)
            {
                var errorBox = MessageBoxManager.GetMessageBoxStandard(
                    title: "Ошибка",
                    text: $"Не удалось сохранить поставщика: {ex.Message}",
                    ButtonEnum.Ok);
                await errorBox.ShowAsync();
            }
        }
    }
}