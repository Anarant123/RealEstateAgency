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
    /// Логика взаимодействия для ListDealPage.xaml
    /// </summary>
    public partial class ListDealPage : Page
    {
        public ListDealPage()
        {
            InitializeComponent();
            lvDeal.ItemsSource = Context.dBClient.GetDeals();
        }

        private void lvDeal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvDeal.SelectedItem is Deal selectedDeal)
            {
                NavigationService.Navigate(new DealPage(selectedDeal));
            }
        }
    }
}
