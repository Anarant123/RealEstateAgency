using RealEstateAgency.DBClient.Data.Models.db;
using RealEstateAgency.DBClient.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RealEstateAgency.Desktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListNeedPage.xaml
    /// </summary>
    public partial class ListNeedPage : Page
    {
        public ListNeedPage()
        {
            InitializeComponent();
            lvNeed.ItemsSource = Context.dBClient.GetNeeds();
        }

        private void lvNeed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvNeed.SelectedItem is Need selectedNeed)
            {
                NavigationService.Navigate(new NeedPage(selectedNeed));
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NeedPage());
        }
    }
}
