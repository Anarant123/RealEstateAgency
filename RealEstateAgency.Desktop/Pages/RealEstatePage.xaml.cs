using RealEstateAgency.DBClient;
using RealEstateAgency.DBClient.Contracts.Requests;
using RealEstateAgency.DBClient.Data.Models.db;
using RealEstateAgency.DBClient.Extensions;
using RealEstateAgency.Desktop.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Xml;
using System.Xml.Linq;

namespace RealEstateAgency.Desktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для RealEstatePage.xaml
    /// </summary>
    public partial class RealEstatePage : Page
    {
        private RealEstate _realEstate;
        private bool IsNeedCoordinate;
        private bool IsNeedPropertyAddress;
        public RealEstatePage()
        {
            InitializeComponent();
            panelBtnToCreate.Visibility = Visibility.Visible;
            panelAddPanel.Visibility = Visibility.Visible;
            cbType.Visibility = Visibility.Visible;
        }

        public RealEstatePage(RealEstate realEstate)
        {
            _realEstate = realEstate;

            InitializeComponent();
            panelBtnToEdit.Visibility = Visibility.Visible;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cbType.ItemsSource = Context.dBClient.GetTypeOfRealEstates();
            if (_realEstate == null) return;
                OnAppearing();
            RemoveMessage();
        }

        private void OnAppearing()
        {
            if (_realEstate.PropertyAddress != null)
            {
                panelAddress.Visibility = Visibility.Visible;
                var address = _realEstate.PropertyAddress;
                tiStreet.Text = address.Street!;
                tiHouseNumber.Text = address.HouseNumber!;
                tiApartmentNumber.Text = address.ApartmentNumber!;
                UCTextInput.ToCollapsed(panelAddress);
            }

            if (_realEstate.Coordinates != null)
            {
                panelCoordinate.Visibility = Visibility.Visible;
                tiLatitude.Text = _realEstate.Coordinates.Latitude.ToString();
                tiLongitude.Text = _realEstate.Coordinates.Longitude.ToString();
                UCTextInput.ToCollapsed(panelCoordinate);
            }

            switch (_realEstate.TypeId)
            {
                case 2:
                    panelTypeApartment.Visibility = Visibility.Visible;
                    tiFloorApartment.Text = _realEstate.Apartment!.Floor;
                    tiCountOfRoomApartment.Text = _realEstate.Apartment.CountRooms;
                    tiAreaApartment.Text = _realEstate.Apartment.Area.ToString()!;
                    break;
                case 3:
                    panelTypeHouse.Visibility = Visibility.Visible;
                    tiCountOfFloorHouse.Text = _realEstate.House!.CountFloors;
                    tiCountOfRoomHouse.Text = _realEstate.House.CountRooms;
                    tiAreaHouse.Text = _realEstate.House.Area.ToString()!;
                    break;
                case 4:
                    panelTypeLandPlot.Visibility = Visibility.Visible;
                    tiAreaLandPlot.Text = _realEstate.LandPlot!.Area.ToString()!;
                    break;
            }

        }
        private void RemoveMessage()
        {

        }

        private void btnAddAddress_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            btn.Visibility = Visibility.Collapsed;
            panelAddress.Visibility = Visibility.Visible;
            IsNeedPropertyAddress = true;
        }

        private void btnAddCoordinates_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            btn.Visibility = Visibility.Collapsed;
            panelCoordinate.Visibility = Visibility.Visible;
            IsNeedCoordinate = true;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            IsNeedPropertyAddress = false;
            IsNeedCoordinate = false;
            panelCoordinate.Visibility = Visibility.Collapsed;
            panelAddress.Visibility = Visibility.Collapsed;
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

            if (!result)
                return;

            CreateRealEstateRequest createRealEstateRequest = new CreateRealEstateRequest()
            {
                Type = (TypeOfRealEstate)cbType.SelectedItem,
            };

            if (IsNeedCoordinate)
                createRealEstateRequest.Coordinates = new Сoordinate()
                {
                    Latitude = double.Parse(tiLatitude.Text),
                    Longitude = double.Parse(tiLongitude.Text)
                };

            if (IsNeedPropertyAddress)
                createRealEstateRequest.PropertyAddress = new PropertyAddress()
                {
                    City = tiCity.Text,
                    Street = tiStreet.Text,
                    HouseNumber = tiHouseNumber.Text,
                    ApartmentNumber = tiApartmentNumber.Text,
                };

            var realEstate = new RealEstate();
            switch (cbType.SelectedIndex)
            {
                case 0:
                    var apartment = new Apartment()
                    {
                        Floor = tiFloorApartment.Text,
                        CountRooms = tiCountOfRoomApartment.Text,
                        Area = double.Parse(tiAreaApartment.Text)
                    };
                    realEstate = await Context.dBClient.CreateRealEstate<Apartment>(apartment, createRealEstateRequest);
                    break;
                case 1:
                    var house = new House()
                    {
                        CountFloors = tiCountOfFloorHouse.Text,
                        CountRooms = tiCountOfRoomHouse.Text,
                        Area = double.Parse(tiAreaHouse.Text)
                    };
                    realEstate = await Context.dBClient.CreateRealEstate<House>(house, createRealEstateRequest);
                    break;
                case 2:
                    var landPlot = new LandPlot()
                    {
                        Area = double.Parse(tiAreaLandPlot.Text)
                    };
                    realEstate = await Context.dBClient.CreateRealEstate<LandPlot>(landPlot, createRealEstateRequest);
                    break;
            }

            NavigationService.Navigate(new NullPage());
            NavigationService.Navigate(new RealEstatePage(realEstate));
        }

        private void btnCancelCreate_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            //panelBtnsEditSettings.Visibility = Visibility.Visible;
            //panelBtnToEdit.Visibility = Visibility.Collapsed;
            //UCTextInput.ToEdit(panelForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Context.dBClient.DeleteRealEstate(_realEstate.Id);
            NavigationService.Navigate(new ListPersonPage(1));
        }

        private async void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            //var result = true;
            //if (!UCTextInput.CheckPanelForm(panelForm))
            //    result = false;

            //if (int.Parse(tiShare.Text) < 0 || int.Parse(tiShare.Text) > 100)
            //{
            //    tiShare.SetMessage("Значение должно быть от 0 до 100", false);
            //    result = false;
            //}

            //if (!result)
            //    return;

            //UpdateRealtorRequest updateRealtorRequest = new UpdateRealtorRequest()
            //{
            //    Id = _realtor.Id,
            //    Name = tiName.Text,
            //    LastName = tiLastName.Text,
            //    MiddleName = tiMiddleName.Text,
            //    ShareCommission = int.Parse(tiShare.Text),
            //};

            //var realtor = await Context.dBClient.UpdateRealtor(updateRealtorRequest);

            //NavigationService.Navigate(new NullPage());
            //NavigationService.Navigate(new RealtorPage(realtor));
        }

        private void btnCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            //OnAppearing();
            //panelBtnToEdit.Visibility = Visibility.Visible;
            //panelBtnsEditSettings.Visibility = Visibility.Collapsed;
            //UCTextInput.ToCollapsed(panelForm);
        }
    }
}
