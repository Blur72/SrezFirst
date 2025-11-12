using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SrezFirst.Data;
using System;
using System.Linq;

namespace SrezFirst
{
    public partial class AddEditSupplierWindow : Window
    {
        Supplier currentSupplier = new Supplier();

        public AddEditSupplierWindow()
        {
            InitializeComponent();
            LoadTypes();
        }

        public AddEditSupplierWindow(Supplier supplier)
        {
            InitializeComponent();
            this.currentSupplier = supplier;
            DataContext = this.currentSupplier;
            LoadTypes();

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

        private void btnClose_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            currentSupplier.Name = nameSupplier.Text!;
            currentSupplier.Inn = innSupplier.Text!;
            currentSupplier.Rating = int.Parse(ratingSupplier.Text);
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
            Close();
        }
    }
}