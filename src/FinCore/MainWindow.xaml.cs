using System.Windows;

namespace FinCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var connection = IEXTrading.IEXTradingConnection.Instance;
            var result = await connection.GetQueryObject_REFERENCEDATA_SYMBOLS().QueryAsync();
            listBoxSymbols.ItemsSource = result.Data.Symbols;
        }
    }
}
