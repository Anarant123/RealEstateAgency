using Microsoft.IdentityModel.Tokens;
using RealEstateAgency.DBClient.Contracts.Requests;
using RealEstateAgency.DBClient.Data.Models;
using RealEstateAgency.DBClient.Data.Models.db;
using RealEstateAgency.DBClient.Extensions;
using RealEstateAgency.Desktop.UserControls;
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
    /// Логика взаимодействия для OfferPage.xaml
    /// </summary>
    public partial class OfferPage : Page
    {
        private Offer _offer;
        private Need _need;
        private class CBPerson<T> where T : Person
        {
            public string FIO { get; set; }
            public T Item { get; set; }

            public CBPerson(T item)
            {
                FIO = $"{item.Name} {item.LastName} {item.MiddleName}";
                Item = item;
            }
        }
        private class CBRealEstate
        {
            public string Info { get; set; }
            public RealEstate Item { get; set; }

            public CBRealEstate(RealEstate item)
            {
                Info = $"{item.Type.Name} ";
                if (item.PropertyAddress != null)
                {
                    if (item.PropertyAddress.City != null)
                        Info += $"{item.PropertyAddress.City} ";
                    if (item.PropertyAddress.Street != null)
                        Info += $"ул. {item.PropertyAddress.Street} ";
                    if (item.PropertyAddress.HouseNumber != null)
                        Info += $"{item.PropertyAddress.HouseNumber} ";
                }
                if (item.Apartment != null)
                    Info += Info += $"площадь {item.Apartment.Area} кв м";
                if (item.House != null)
                    Info += Info += $"площадь {item.House.Area} кв м";
                if (item.LandPlot != null)
                    Info += Info += $"площадь {item.LandPlot.Area} кв м";
                Item = item;
            }

        }

        public OfferPage()
        {
            InitializeComponent();
            panelBtnToCreate.Visibility = Visibility.Visible;
        }

        public OfferPage(Offer offer)
        {
            _offer = Context.dBClient!.GetOffer(offer.Id);

            InitializeComponent();
            panelBtnToEdit.Visibility = Visibility.Visible;

            var needs = Context.dBClient.GetNeeds().Where(x => offer.RealEstate.TypeId == x.TypeId).ToList();
            if (offer.RealEstate.PropertyAddress != null)
            {
                needs = needs.Where(x => x.PropertyAddress != null).ToList();
                if (!needs.Any())
                {
                    needs = Context.dBClient.GetNeeds().Where(x => offer.RealEstate.TypeId == x.TypeId).ToList();
                }
                else
                {
                    if (offer.RealEstate.PropertyAddress.City != null)
                        needs = needs.Where(x => offer.RealEstate.PropertyAddress.City == x.PropertyAddress.City).ToList();
                    if (offer.RealEstate.PropertyAddress.Street != null)
                        needs = needs.Where(x => offer.RealEstate.PropertyAddress.Street == x.PropertyAddress.Street).ToList();
                }
            }
            needs = needs.Where(x => offer.Price >= x.MinPrice && offer.Price <= x.MaxPrice).ToList();

            lvNeed.ItemsSource = needs;
        }

        public OfferPage(Offer offer, Need need)
        {
            _offer = Context.dBClient!.GetOffer(offer.Id);
            _need = Context.dBClient!.GetNeed(need.Id);
            InitializeComponent();
            panelDeal.Visibility = Visibility.Visible;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var listClients = new List<CBPerson<Client>>();
            foreach (var client in Context.dBClient.GetClients())
                listClients.Add(new CBPerson<Client>(client));
            cbClient.ItemsSource = listClients;

            var listRealtors = new List<CBPerson<Realtor>>();
            foreach (var realtor in Context.dBClient.GetRealtors())
                listRealtors.Add(new CBPerson<Realtor>(realtor));
            cbRealtor.ItemsSource = listRealtors;

            var listRealEstate = new List<CBRealEstate>();
            foreach (var realEstate in Context.dBClient.GetRealEstates())
                listRealEstate.Add(new CBRealEstate(realEstate));
            cbRealEstate.ItemsSource = listRealEstate;

            if (_offer == null) return;
            OnAppearing();
            RemoveMessage(mainPanel);
        }

        private void OnAppearing()
        {
            tiPrice.Text = _offer.Price.ToString();

            var listClients = new List<CBPerson<Client>>();
            foreach (var client in Context.dBClient.GetClients())
                listClients.Add(new CBPerson<Client>(client));
            cbClient.ItemsSource = listClients;
            cbClient.SelectedItem = listClients.First(x => x.Item == _offer.Client);
            cbClient.IsEnabled = false;

            var listRealtors = new List<CBPerson<Realtor>>();
            foreach (var realtor in Context.dBClient.GetRealtors())
                listRealtors.Add(new CBPerson<Realtor>(realtor));
            cbRealtor.ItemsSource = listRealtors;
            cbRealtor.SelectedItem = listRealtors.First(x => x.Item == _offer.Realtor);
            cbRealtor.IsEnabled = false;

            var listRealEstate = new List<CBRealEstate>();
            foreach (var realEstate in Context.dBClient.GetRealEstates())
                listRealEstate.Add(new CBRealEstate(realEstate));
            cbRealEstate.ItemsSource = listRealEstate;
            cbRealEstate.SelectedItem = listRealEstate.First(x => x.Item == _offer.RealEstate);
            cbRealEstate.IsEnabled = false;

            UCTextInput.ToCollapsed(panelInfo);
        }

        private void RemoveMessage(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is UCTextInput ucTextInput)
                    ucTextInput.RemoveMessage();

                RemoveMessage(child);
            }
        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (!UCTextInput.CheckPanelForm(panelInfo)) return;

            var client = (CBPerson<Client>)cbClient.SelectedItem;
            var realtor = (CBPerson<Realtor>)cbRealtor.SelectedItem;
            var realEstate = (CBRealEstate)cbRealEstate.SelectedItem;

            CreateOfferRequest createOffer = new CreateOfferRequest()
            {
                Client = client.Item,
                Realtor = realtor.Item,
                RealEstate = realEstate.Item,
                Price = int.Parse(tiPrice.Text),
            };

            var offer = await Context.dBClient.CreateOffer(createOffer);

            NavigationService.Navigate(new NullPage());
            NavigationService.Navigate(new OfferPage(offer));
        }

        private void btnCancelCreate_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            panelBtnsEditSettings.Visibility = Visibility.Visible;
            panelBtnToEdit.Visibility = Visibility.Collapsed;
            UCTextInput.ToEdit(panelInfo);
            cbClient.IsEnabled = false;
            cbRealtor.IsEnabled = false;
            cbRealEstate.IsEnabled = false;
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            await Context.dBClient.DeleteOffer(_offer.Id);
            NavigationService.Navigate(new ListOfferPage());
        }

        private async void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (!UCTextInput.CheckPanelForm(panelInfo)) return;

            var client = (CBPerson<Client>)cbClient.SelectedItem;
            var realtor = (CBPerson<Realtor>)cbRealtor.SelectedItem;
            var realEstate = (CBRealEstate)cbRealEstate.SelectedItem;

            UpdateOfferRequest updateOffer = new UpdateOfferRequest()
            {
                Id = _offer.Id,
                Client = client.Item,
                Realtor = realtor.Item,
                RealEstate = realEstate.Item,
                Price = int.Parse(tiPrice.Text),
            };

            var offer = await Context.dBClient.UpdateOffer(updateOffer);
            NavigationService.Navigate(new NullPage());
            NavigationService.Navigate(new OfferPage(offer));
        }

        private void btnCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            OnAppearing();
            panelBtnToEdit.Visibility = Visibility.Visible;
            panelBtnsEditSettings.Visibility = Visibility.Collapsed;
        }

        private void lvNeed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvNeed.SelectedItem is Need selectedNeed)
            {
                NavigationService.Navigate(new NeedPage(selectedNeed, _offer));
            }
        }

        private async void btnHaveDeal_Click(object sender, RoutedEventArgs e)
        {
            CreateDealRequest createDeal = new CreateDealRequest()
            {
                Offer = _offer,
                Need = _need,
            };

            var deal = await Context.dBClient.CreateDeal(createDeal);
            NavigationService.Navigate(new NullPage());
            NavigationService.Navigate(new DealPage(deal));
        }
    }
}
