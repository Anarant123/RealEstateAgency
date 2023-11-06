﻿using RealEstateAgency.DBClient.Contracts.Requests;
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
    /// Логика взаимодействия для NeedPage.xaml
    /// </summary>
    public partial class NeedPage : Page
    {
        private Need _need;
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
            cbClient.ItemsSource = listRealtors;

            if (_need == null) return;
            //OnAppearing();
            RemoveMessage(mainPanel);
        }

        //private void OnAppearing()
        //{
        //    if (_realEstate.PropertyAddress != null)
        //    {
        //        panelAddress.Visibility = Visibility.Visible;
        //        var address = _realEstate.PropertyAddress;
        //        tiCity.Text = address.City!;
        //        tiStreet.Text = address.Street!;
        //        tiHouseNumber.Text = address.HouseNumber!;
        //        tiApartmentNumber.Text = address.ApartmentNumber!;
        //        UCTextInput.ToCollapsed(panelAddress);
        //    }

        //    if (_realEstate.Coordinates != null)
        //    {
        //        panelCoordinate.Visibility = Visibility.Visible;
        //        tiLatitude.Text = _realEstate.Coordinates.Latitude.ToString();
        //        tiLongitude.Text = _realEstate.Coordinates.Longitude.ToString();
        //        UCTextInput.ToCollapsed(panelCoordinate);
        //    }

        //    switch (_realEstate.TypeId)
        //    {
        //        case 2:
        //            panelTypeApartment.Visibility = Visibility.Visible;
        //            tiFloorApartment.Text = _realEstate.Apartment!.Floor;
        //            tiCountOfRoomApartment.Text = _realEstate.Apartment.CountRooms;
        //            tiAreaApartment.Text = _realEstate.Apartment.Area.ToString()!;
        //            UCTextInput.ToCollapsed(panelTypeApartment);
        //            break;
        //        case 3:
        //            panelTypeHouse.Visibility = Visibility.Visible;
        //            tiCountOfFloorHouse.Text = _realEstate.House!.CountFloors;
        //            tiCountOfRoomHouse.Text = _realEstate.House.CountRooms;
        //            tiAreaHouse.Text = _realEstate.House.Area.ToString()!;
        //            UCTextInput.ToCollapsed(panelTypeHouse);
        //            break;
        //        case 4:
        //            panelTypeLandPlot.Visibility = Visibility.Visible;
        //            tiAreaLandPlot.Text = _realEstate.LandPlot!.Area.ToString()!;
        //            UCTextInput.ToCollapsed(panelTypeLandPlot);
        //            break;
        //    }
        //}

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
            //panelBtnsEditSettings.Visibility = Visibility.Visible;
            //panelBtnToEdit.Visibility = Visibility.Collapsed;

            //if (_realEstate.PropertyAddress != null)
            //    UCTextInput.ToEdit(panelAddress);

            //if (_realEstate.Coordinates != null)
            //    UCTextInput.ToEdit(panelCoordinate);

            //switch (_realEstate.TypeId)
            //{
            //    case 2:
            //        UCTextInput.ToEdit(panelTypeApartment);
            //        break;
            //    case 3:
            //        UCTextInput.ToEdit(panelTypeHouse);
            //        break;
            //    case 4:
            //        UCTextInput.ToEdit(panelTypeLandPlot);
            //        break;
            //}
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Context.dBClient.DeleteNeed(_need.Id);
            NavigationService.Navigate(new ListPersonPage(1));
        }

        private async void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            //var result = true;
            //switch (_realEstate.TypeId)
            //{
            //    case 2:
            //        if (!UCTextInput.CheckPanelForm(panelTypeApartment))
            //            result = false;
            //        break;
            //    case 3:
            //        if (!UCTextInput.CheckPanelForm(panelTypeHouse))
            //            result = false;
            //        break;
            //    case 4:
            //        if (!UCTextInput.CheckPanelForm(panelTypeLandPlot))
            //            result = false;
            //        break;
            //}

            //if (!result) return;

            //UpdateRealEstateRequest updateRealEstate = new UpdateRealEstateRequest()
            //{
            //    Id = _realEstate.Id,
            //    Coordinates = _realEstate.Coordinates,
            //    PropertyAddress = _realEstate.PropertyAddress,
            //    Apartment = _realEstate.Apartment,
            //    House = _realEstate.House,
            //    LandPlot = _realEstate.LandPlot,
            //};

            //if (updateRealEstate.Coordinates != null)
            //{
            //    updateRealEstate.Coordinates.Latitude = double.Parse(tiLatitude.Text);
            //    updateRealEstate.Coordinates.Longitude = double.Parse(tiLongitude.Text);
            //}

            //if (updateRealEstate.PropertyAddress != null)
            //{
            //    updateRealEstate.PropertyAddress.City = tiCity.Text;
            //    updateRealEstate.PropertyAddress.Street = tiStreet.Text;
            //    updateRealEstate.PropertyAddress.HouseNumber = tiHouseNumber.Text;
            //    updateRealEstate.PropertyAddress.ApartmentNumber = tiApartmentNumber.Text;
            //}

            //switch (_realEstate.Type.Id)
            //{
            //    case 2:
            //        updateRealEstate.Apartment.Floor = tiFloorApartment.Text;
            //        updateRealEstate.Apartment.CountRooms = tiCountOfRoomApartment.Text;
            //        updateRealEstate.Apartment.Area = double.Parse(tiAreaApartment.Text);
            //        break;
            //    case 3:
            //        updateRealEstate.House.CountFloors = tiCountOfFloorHouse.Text;
            //        updateRealEstate.House.CountRooms = tiCountOfRoomHouse.Text;
            //        updateRealEstate.House.Area = double.Parse(tiAreaHouse.Text);
            //        break;
            //    case 4:
            //        updateRealEstate.LandPlot.Area = double.Parse(tiAreaLandPlot.Text);
            //        break;
            //}

            //var realEstate = await Context.dBClient.UpdateRealEstate(updateRealEstate);
            //NavigationService.Navigate(new NullPage());
            //NavigationService.Navigate(new RealEstatePage(realEstate));
        }

        private void btnCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            //OnAppearing();
            //panelBtnToEdit.Visibility = Visibility.Visible;
            //panelBtnsEditSettings.Visibility = Visibility.Collapsed;
            //if (_realEstate.PropertyAddress != null)
            //    UCTextInput.ToCollapsed(panelAddress);

            //if (_realEstate.Coordinates != null)
            //    UCTextInput.ToCollapsed(panelCoordinate);

            //switch (_realEstate.TypeId)
            //{
            //    case 2:
            //        UCTextInput.ToCollapsed(panelTypeApartment);
            //        break;
            //    case 3:
            //        UCTextInput.ToCollapsed(panelTypeHouse);
            //        break;
            //    case 4:
            //        UCTextInput.ToCollapsed(panelTypeLandPlot);
            //        break;
            //}
        }
    }
}
