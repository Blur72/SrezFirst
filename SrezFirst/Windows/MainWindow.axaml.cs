using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SrezFirst
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            
            mainControl.Content = new MaterialsPage();
        }

        private void btnMaterials_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            mainControl.Content = new MaterialsPage();
        }

        private void btnSuppliers_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            mainControl.Content = new SuppliersPage();
        }

        private void btnCalculation_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            mainControl.Content = new CalculationPage();
        }
    }
}