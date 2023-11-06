using RealEstateAgency.DBClient.Data.Models.db;
using RealEstateAgency.DBClient.Extensions;
using RealEstateAgency.Desktop.Pages.ClientPages;
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
    /// Логика взаимодействия для ListRealEstatePage.xaml
    /// </summary>
    public partial class ListRealEstatePage : Page
    {
        public ListRealEstatePage()
        {
            InitializeComponent();
            lvRealestate.ItemsSource = Context.dBClient.GetRealEstates();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RealEstatePage());
        }

        private void lvRealestate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvRealestate.SelectedItem is RealEstate selectedRealEstate)
            {
                NavigationService.Navigate(new RealEstatePage(selectedRealEstate));
            }
        }
    }
}
