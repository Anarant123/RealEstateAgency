using RealEstateAgency.DBClient.Data.Models.db;
using RealEstateAgency.DBClient.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace RealEstateAgency.Desktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListOfferPage.xaml
    /// </summary>
    public partial class ListOfferPage : Page
    {
        public ListOfferPage()
        {
            InitializeComponent();
            lvOffer.ItemsSource = Context.dBClient.GetOffers();
        }

        private void lvOffer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvOffer.SelectedItem is Offer selectedOffer)
            {
                NavigationService.Navigate(new OfferPage(selectedOffer));
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OfferPage());
        }
    }
}
