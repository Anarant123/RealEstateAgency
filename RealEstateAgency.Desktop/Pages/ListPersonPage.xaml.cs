using RealEstateAgency.DBClient.Data.Models.db;
using RealEstateAgency.DBClient.Extensions;
using RealEstateAgency.Desktop.Pages.ClientPages;
using RealEstateAgency.Desktop.Pages.RealtorPages;
using RealEstateAgency.Desktop.Utilities;
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
    /// Логика взаимодействия для ListPersonPage.xaml
    /// </summary>
    public partial class ListPersonPage : Page
    {
        int mode = 0;
        public ListPersonPage(int mode)
        {
            InitializeComponent();
            this.mode = mode;
            if (mode == 1)
                lvPeople.ItemsSource = Context.dBClient.GetClients();
            if (mode == 2)
                lvPeople.ItemsSource = Context.dBClient.GetRealtors();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (mode == 1)
                NavigationService.Navigate(new ClientPage());
            if (mode == 2)
                NavigationService.Navigate(new RealtorPage());
        }

        private void btnResetSearch_Click(object sender, RoutedEventArgs e)
        {
            tiFullName.Text = string.Empty;
            if (mode == 1)
                lvPeople.ItemsSource = Context.dBClient.GetClients();
            if (mode == 2)
                lvPeople.ItemsSource = Context.dBClient.GetRealtors();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (mode == 1)
            {
                lvPeople.ItemsSource = LevenshteinDistance.SearchItemsByNameFuzzy("John", Context.dBClient.GetClients(), client => client.Name);
            }
            if (mode == 2)
            {
                lvPeople.ItemsSource = LevenshteinDistance.SearchItemsByNameFuzzy("Alice", Context.dBClient.GetRealtors(), realtor => realtor.Name);
            }
        }

        private void lvPeople_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvPeople.SelectedItem is Client selectedClient)
            {
                NavigationService.Navigate(new ClientPage(selectedClient));
            }

            if (lvPeople.SelectedItem is Realtor selectedRealtor)
            {
                NavigationService.Navigate(new RealtorPage(selectedRealtor));
            }
        }
    }
}
