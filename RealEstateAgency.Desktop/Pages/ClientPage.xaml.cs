﻿using RealEstateAgency.DBClient.Contracts.Requests;
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

namespace RealEstateAgency.Desktop.Pages.ClientPages
{
    /// <summary>
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        private Client _client;
        public ClientPage()
        {
            InitializeComponent();
            panelBtnToCreate.Visibility = Visibility.Visible;
            panelNF.Visibility = Visibility.Collapsed;
        }

        public ClientPage(Client client)
        {
            _client = client;

            InitializeComponent();
            panelBtnToEdit.Visibility = Visibility.Visible;

            var needs = Context.dBClient.GetNeeds().Where(x => x.ClientId == _client.Id);
            var offers = Context.dBClient.GetOffers().Where(x => x.ClientId == _client.Id);
            if (needs.Any() || offers.Any()) 
            {
                lvNeed.ItemsSource = needs;
                lvOffer.ItemsSource = offers;
            }
            else
            {
                panelNF.Visibility = Visibility.Collapsed;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_client == null) return;
            OnAppearing();
            UCTextInput.ToCollapsed(panelForm);
            RemoveMessage();
        }
        private void OnAppearing()
        {
            tiName.Text = _client.Name!;
            tiLastName.Text = _client.LastName!;
            tiMiddleName.Text = _client.MiddleName!;
            tiPhoneNumber.Text = _client.PhoneNumber!;
            tiEmail.Text = _client.Email!;
        }

        private void RemoveMessage()
        {
            tiName.RemoveMessage();
            tiEmail.RemoveMessage();
            tiLastName.RemoveMessage();
            tiMiddleName.RemoveMessage();
            tiPhoneNumber.RemoveMessage();
        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            var result = true;
            if (!UCTextInput.CheckPanelForm(panelForm))
                result = false;

            if (tiPhoneNumber.IsNullOrEmpty(tiPhoneNumber.Text) && tiEmail.IsNullOrEmpty(tiEmail.Text))
            {
                tiPhoneNumber.SetMessage("Одно из этих полей должно быть заполненно", false);
                tiEmail.SetMessage("Одно из этих полей должно быть заполненно", false);
                result = false;
            }
            else
            {
                if (tiPhoneNumber.IsNullOrEmpty(tiPhoneNumber.Text))
                    tiPhoneNumber.SuccessMessage();
                else if (tiEmail.IsNullOrEmpty(tiEmail.Text))
                    tiEmail.SuccessMessage();
                result = true;
            }

            if (!result)
                return;

            CreateClientRequest createClientRequest = new CreateClientRequest()
            {
                Name = tiName.Text,
                LastName = tiLastName.Text,
                MiddleName = tiMiddleName.Text,
                PhoneNumber = tiPhoneNumber.Text,
                Email = tiEmail.Text,
            };
            var client = await Context.dBClient.CreateClient(createClientRequest);

            NavigationService.Navigate(new NullPage());
            NavigationService.Navigate(new ClientPage(client));
        }

        private void btnCancelCreate_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private async void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            var result = true;
            if (!UCTextInput.CheckPanelForm(panelForm))
                result = false;

            if (tiPhoneNumber.IsNullOrEmpty(tiPhoneNumber.Text) && tiEmail.IsNullOrEmpty(tiEmail.Text))
            {
                tiPhoneNumber.SetMessage("Одно из этих полей должно быть заполненно", false);
                tiEmail.SetMessage("Одно из этих полей должно быть заполненно", false);
                result = false;
            }
            else
            {
                if (tiPhoneNumber.IsNullOrEmpty(tiPhoneNumber.Text))
                    tiPhoneNumber.SuccessMessage();
                else if (tiEmail.IsNullOrEmpty(tiEmail.Text))
                    tiEmail.SuccessMessage();
                result = true;
            }

            if (!result)
                return;

            UpdateClientRequest updateClientRequest = new UpdateClientRequest()
            {
                Id = _client.Id,
                Name = tiName.Text,
                LastName = tiLastName.Text,
                MiddleName = tiMiddleName.Text,
                PhoneNumber = tiPhoneNumber.Text,
                Email = tiEmail.Text,
            };

            var client = await Context.dBClient.UpdateClient(updateClientRequest);

            NavigationService.Navigate(new NullPage());
            NavigationService.Navigate(new ClientPage(client));
        }

        private void btnCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            OnAppearing();
            panelBtnToEdit.Visibility = Visibility.Visible;
            panelBtnsEditSettings.Visibility = Visibility.Collapsed;
            UCTextInput.ToCollapsed(panelForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            panelBtnsEditSettings.Visibility = Visibility.Visible;
            panelBtnToEdit.Visibility = Visibility.Collapsed;
            UCTextInput.ToEdit(panelForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Context.dBClient.DeleteClient(_client.Id);
            NavigationService.Navigate(new ListPersonPage(1));
        }
    }
}
