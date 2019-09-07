using System;
using System.Linq;
using System.Windows;

namespace DataGridSQLExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AdventureWorksEntities _adventureWorksEntities = new AdventureWorksEntities();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var query =
                from product in _adventureWorksEntities.Products
                where product.Color == "Red"
                orderby product.ListPrice
                select new
                {
                    product.Name,
                    product.Color,
                    Category = product.ProductSubcategory.ProductCategory.Name,
                    Price = product.ListPrice,
                };

            DataGrid1.ItemsSource = query.ToList();
        }
    }
}
