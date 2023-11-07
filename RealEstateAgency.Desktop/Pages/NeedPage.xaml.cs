using RealEstateAgency.DBClient.Contracts.Requests;
using RealEstateAgency.DBClient.Data.Models;
using RealEstateAgency.DBClient.Data.Models.db;
using RealEstateAgency.DBClient.Extensions;
using RealEstateAgency.Desktop.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для NeedPage.xaml
    /// </summary>
    public partial class NeedPage : Page
    {
        private Need _need;
        private Offer _offer;
        private bool IsNeedPropertyAddress;
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

        public NeedPage()
        {
            InitializeComponent();
            panelBtnToCreate.Visibility = Visibility.Visible;
            panelAddPanel.Visibility = Visibility.Visible;
            cbType.Visibility = Visibility.Visible;
        }

        public NeedPage(Need need)
        {
            _need = Context.dBClient!.GetNeed(need.Id);

            InitializeComponent();
            panelBtnToEdit.Visibility = Visibility.Visible;

            var offers = Context.dBClient.GetOffers().Where(x => x.RealEstate.TypeId == need.TypeId ).ToList();
            if (need.PropertyAddress != null)
            {
                if (need.PropertyAddress.City != null)
                    offers = offers.Where(x => x.RealEstate.PropertyAddress.City == need.PropertyAddress.City).ToList();
                if (need.PropertyAddress.Street != null)
                    offers = offers.Where(x => x.RealEstate.PropertyAddress.Street == need.PropertyAddress.Street).ToList();
            }
            offers = offers.Where(x => x.Price >= need.MinPrice && x.Price <= need.MaxPrice).ToList();

            lvOffer.ItemsSource = offers;
        }

        public NeedPage(Need need, Offer offer)
        {
            _need = Context.dBClient!.GetNeed(need.Id);
            _offer = Context.dBClient!.GetOffer(offer.Id);

            InitializeComponent();
            panelDeal.Visibility = Visibility.Visible;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cbType.ItemsSource = Context.dBClient!.GetTypeOfRealEstates();

            var listClients = new List<CBPerson<Client>>();
            foreach (var client in Context.dBClient.GetClients())
                listClients.Add(new CBPerson<Client>(client));
            cbClient.ItemsSource = listClients;

            var listRealtors = new List<CBPerson<Realtor>>();
            foreach (var realtor in Context.dBClient.GetRealtors())
                listRealtors.Add(new CBPerson<Realtor>(realtor));
            cbRealtor.ItemsSource = listRealtors;

            if (_need == null) return;
            OnAppearing();
            RemoveMessage(mainPanel);
        }

        private void OnAppearing()
        {
            tiMaxPrice.Text = _need.MaxPrice.ToString();
            tiMinPrice.Text = _need.MinPrice.ToString();
            cbType.SelectedIndex = _need.TypeId - 2;

            var listClients = new List<CBPerson<Client>>();
            foreach (var client in Context.dBClient.GetClients())
                listClients.Add(new CBPerson<Client>(client));
            cbClient.ItemsSource = listClients;
            cbClient.SelectedItem = listClients.First(x => x.Item == _need.Client);
            cbClient.IsEnabled = false;

            var listRealtors = new List<CBPerson<Realtor>>();
            foreach (var realtor in Context.dBClient.GetRealtors())
                listRealtors.Add(new CBPerson<Realtor>(realtor));
            cbRealtor.ItemsSource = listRealtors;
            cbRealtor.SelectedItem = listRealtors.First(x => x.Item == _need.Realtor);
            cbRealtor.IsEnabled = false;
            UCTextInput.ToCollapsed(panelInfo);
            

            if (_need.PropertyAddress != null)
            {
                panelAddress.Visibility = Visibility.Visible;
                var address = _need.PropertyAddress;
                tiCity.Text = address.City!;
                tiStreet.Text = address.Street!;
                tiHouseNumber.Text = address.HouseNumber!;
                tiApartmentNumber.Text = address.ApartmentNumber!;
                UCTextInput.ToCollapsed(panelAddress);
            }

            switch (_need.TypeId)
            {
                case 2:
                    panelTypeApartment.Visibility = Visibility.Visible;
                    var apartment = _need.NeedForApartments.First();
                    tiMinCountOfRoomApartment.Text = apartment.MinCountRooms;
                    tiMaxCountOfRoomApartment.Text = apartment.MaxCountRooms;
                    tiMinFloorApartment.Text = apartment.MinFloor;
                    tiMaxFloorApartment.Text = apartment.MaxFloor;
                    tiMinAreaApartment.Text = apartment.MinArea.ToString();
                    tiMaxAreaApartment.Text = apartment.MaxArea.ToString();
                    UCTextInput.ToCollapsed(panelTypeApartment);
                    break;
                case 3:
                    panelTypeHouse.Visibility = Visibility.Visible;
                    var house = _need.NeedForHomes.First();
                    tiMinCountOfRoomHouse.Text = house.MinCountRooms;
                    tiMaxCountOfRoomHouse.Text = house.MaxCountRooms;
                    tiMinCountOfFloorHouse.Text = house.MinCountFloors;
                    tiMaxCountOfFloorHouse.Text = house.MaxCountFloors;
                    tiMinAreaHouse.Text = house.MinArea.ToString();
                    tiMaxAreaHouse.Text = house.MaxArea.ToString();
                    UCTextInput.ToCollapsed(panelTypeHouse);
                    break;
                case 4:
                    panelTypeLandPlot.Visibility = Visibility.Visible;
                    var landPlot = _need.NeedForLandPlots.First();
                    tiMinAreaLandPlot.Text = landPlot.MinArea.ToString();
                    tiMaxAreaLandPlot.Text = landPlot.MaxArea.ToString();
                    UCTextInput.ToCollapsed(panelTypeLandPlot);
                    break;
            }
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
        private void cbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cbType.SelectedIndex)
            {
                case 0:
                    panelTypeApartment.Visibility = Visibility.Visible;
                    panelTypeHouse.Visibility = Visibility.Collapsed;
                    panelTypeLandPlot.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    panelTypeApartment.Visibility = Visibility.Collapsed;
                    panelTypeHouse.Visibility = Visibility.Visible;
                    panelTypeLandPlot.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    panelTypeApartment.Visibility = Visibility.Collapsed;
                    panelTypeHouse.Visibility = Visibility.Collapsed;
                    panelTypeLandPlot.Visibility = Visibility.Visible;
                    break;
            }
        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            var result = true;
            switch (cbType.SelectedIndex)
            {
                case 0:
                    if (!UCTextInput.CheckPanelForm(panelTypeApartment))
                        result = false;
                    break;
                case 1:
                    if (!UCTextInput.CheckPanelForm(panelTypeHouse))
                        result = false;
                    break;
                case 2:
                    if (!UCTextInput.CheckPanelForm(panelTypeLandPlot))
                        result = false;
                    break;
            }

            if (!result) return;

            var client = (CBPerson<Client>)cbClient.SelectedItem;
            var realtor = (CBPerson<Realtor>)cbRealtor.SelectedItem;

            CreateNeedRequest createNeed = new CreateNeedRequest()
            {
                Type = (TypeOfRealEstate)cbType.SelectedItem,
                MaxPrice = int.Parse(tiMaxPrice.Text),
                MinPrice = int.Parse(tiMinPrice.Text),
                Client = client.Item,
                Realtor = realtor.Item,
            };

            if (IsNeedPropertyAddress)
                createNeed.PropertyAddress = new PropertyAddress()
                {
                    City = tiCity.Text,
                    Street = tiStreet.Text,
                    HouseNumber = tiHouseNumber.Text,
                    ApartmentNumber = tiApartmentNumber.Text,
                };

            var need = new Need();
            switch (cbType.SelectedIndex)
            {
                case 0:
                    var apartment = new NeedForApartment()
                    {
                        MinCountRooms = tiMinCountOfRoomApartment.Text,
                        MaxCountRooms = tiMaxCountOfRoomApartment.Text,
                        MinFloor = tiMinFloorApartment.Text,
                        MaxFloor = tiMaxFloorApartment.Text,
                        MinArea = double.Parse(tiMinAreaApartment.Text),
                        MaxArea = double.Parse(tiMaxAreaApartment.Text)
                    };
                    need = await Context.dBClient.CreateNeed<NeedForApartment>(apartment, createNeed);
                    break;
                case 1:
                    var house = new NeedForHome()
                    {
                        MinCountRooms = tiMinCountOfRoomHouse.Text,
                        MaxCountRooms = tiMaxCountOfRoomHouse.Text,
                        MinCountFloors = tiMinCountOfFloorHouse.Text,
                        MaxCountFloors = tiMaxCountOfFloorHouse.Text,
                        MinArea = double.Parse(tiMinAreaHouse.Text),
                        MaxArea = double.Parse(tiMaxAreaHouse.Text)
                    };
                    need = await Context.dBClient.CreateNeed<NeedForHome>(house, createNeed);
                    break;
                case 2:
                    var landPlot = new NeedForLandPlot()
                    {
                        MinArea = double.Parse(tiMinAreaLandPlot.Text),
                        MaxArea = double.Parse(tiMaxAreaLandPlot.Text)
                    };
                    need = await Context.dBClient.CreateNeed<NeedForLandPlot>(landPlot, createNeed);
                    break;
            }

            NavigationService.Navigate(new NullPage());
            NavigationService.Navigate(new NeedPage(need));
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

            if (_need.PropertyAddress != null)
                UCTextInput.ToEdit(panelAddress);

            switch (_need.TypeId)
            {
                case 2:
                    UCTextInput.ToEdit(panelTypeApartment);
                    break;
                case 3:
                    UCTextInput.ToEdit(panelTypeHouse);
                    break;
                case 4:
                    UCTextInput.ToEdit(panelTypeLandPlot);
                    break;
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            await Context.dBClient.DeleteNeed(_need.Id);

            NavigationService.Navigate(new ListNeedPage());
        }

        private async void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            var result = true;
            switch (_need.TypeId)
            {
                case 2:
                    if (!UCTextInput.CheckPanelForm(panelTypeApartment))
                        result = false;
                    break;
                case 3:
                    if (!UCTextInput.CheckPanelForm(panelTypeHouse))
                        result = false;
                    break;
                case 4:
                    if (!UCTextInput.CheckPanelForm(panelTypeLandPlot))
                        result = false;
                    break;
            }

            if (!result) return;

            UpdateNeedRequest updateNeed = new UpdateNeedRequest()
            {
                Id = _need.Id,
                PropertyAddress = _need.PropertyAddress,
                NeedForApartments = _need.NeedForApartments,
                NeedForHomes = _need.NeedForHomes,
                NeedForLandPlots = _need.NeedForLandPlots,
                Type = _need.Type,
                MaxPrice = int.Parse(tiMaxPrice.Text),
                MinPrice = int.Parse(tiMinPrice.Text),
            };

            if (updateNeed.PropertyAddress != null)
            {
                updateNeed.PropertyAddress.City = tiCity.Text;
                updateNeed.PropertyAddress.Street = tiStreet.Text;
                updateNeed.PropertyAddress.HouseNumber = tiHouseNumber.Text;
                updateNeed.PropertyAddress.ApartmentNumber = tiApartmentNumber.Text;
            }

            switch (updateNeed.Type.Id)
            {
                case 2:
                    var apartment = updateNeed.NeedForApartments.FirstOrDefault();
                    apartment.MinCountRooms = tiMinCountOfRoomApartment.Text;
                    apartment.MaxCountRooms = tiMaxCountOfRoomApartment.Text;
                    apartment.MinFloor = tiMinFloorApartment.Text;
                    apartment.MaxFloor = tiMaxFloorApartment.Text;
                    apartment.MinArea = double.Parse(tiMinAreaApartment.Text);
                    apartment.MaxArea = double.Parse(tiMaxAreaApartment.Text);
                    break;
                case 3:
                    var house = updateNeed.NeedForHomes.FirstOrDefault();
                    house.MinCountRooms = tiMinCountOfRoomHouse.Text;
                    house.MaxCountRooms = tiMaxCountOfRoomHouse.Text;
                    house.MinCountFloors = tiMinCountOfFloorHouse.Text;
                    house.MaxCountFloors = tiMaxCountOfFloorHouse.Text;
                    house.MinArea = double.Parse(tiMinAreaHouse.Text);
                    house.MaxArea = double.Parse(tiMaxAreaHouse.Text);
                    break;
                case 4:
                    var landPlot = updateNeed.NeedForLandPlots.FirstOrDefault();
                    landPlot.MinArea = double.Parse(tiMinAreaLandPlot.Text);
                    landPlot.MaxArea = double.Parse(tiMaxAreaLandPlot.Text);
                    break;
            }

            var need = await Context.dBClient.UpdateNeed(updateNeed);
            NavigationService.Navigate(new NullPage());
            NavigationService.Navigate(new NeedPage(need));
        }

        private void btnCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            OnAppearing();
            panelBtnToEdit.Visibility = Visibility.Visible;
            panelBtnsEditSettings.Visibility = Visibility.Collapsed;
            if (_need.PropertyAddress != null)
                UCTextInput.ToCollapsed(panelAddress);

            switch (_need.TypeId)
            {
                case 2:
                    UCTextInput.ToCollapsed(panelTypeApartment);
                    break;
                case 3:
                    UCTextInput.ToCollapsed(panelTypeHouse);
                    break;
                case 4:
                    UCTextInput.ToCollapsed(panelTypeLandPlot);
                    break;
            }
        }

        private void btnAddAddress_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            btn.Visibility = Visibility.Collapsed;
            panelAddress.Visibility = Visibility.Visible;
            IsNeedPropertyAddress = true;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            IsNeedPropertyAddress = false;
            panelAddress.Visibility = Visibility.Collapsed;
        }

        private void lvOffer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvOffer.SelectedItem is Offer selectedOffer)
            {
                NavigationService.Navigate(new OfferPage(selectedOffer, _need));
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
