using System.Windows.Controls;

namespace ExpenseIt
{
    /// <summary>
    /// Interaction logic for ExpenseItHome.xaml
    /// </summary>
    public partial class ExpenseItHome : Page
    {
        public ExpenseItHome()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var expenseReportPage = new ExpenseReportPage(PeopleListBox.SelectedItem);
            NavigationService.Navigate(expenseReportPage);
        }
    }
}
