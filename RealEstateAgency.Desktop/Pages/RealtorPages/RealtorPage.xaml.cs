using RealEstateAgency.DBClient;
using RealEstateAgency.DBClient.Contracts.Requests;
using RealEstateAgency.DBClient.Data.Models.db;
using RealEstateAgency.DBClient.Extensions;
using RealEstateAgency.Desktop.Pages.ClientPages;
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

namespace RealEstateAgency.Desktop.Pages.RealtorPages
{
    /// <summary>
    /// Логика взаимодействия для RealtorPage.xaml
    /// </summary>
    public partial class RealtorPage : Page
    {
        private Realtor _realtor;
        public RealtorPage()
        {
            InitializeComponent();
            panelBtnToCreate.Visibility = Visibility.Visible;
        }

        public RealtorPage(Realtor realtor)
        {
            _realtor = realtor;

            InitializeComponent();
            panelBtnToEdit.Visibility = Visibility.Visible;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_realtor == null) return;
            OnAppearing();
            UCTextInput.ToCollapsed(panelForm);
            RemoveMessage();
        }
        private void OnAppearing()
        {
            tiName.Text = _realtor.Name!;
            tiLastName.Text = _realtor.LastName!;
            tiMiddleName.Text = _realtor.MiddleName!;
            tiShare.Text = _realtor.ShareCommission.ToString();
        }

        private void RemoveMessage()
        {
            tiName.RemoveMessage();
            tiLastName.RemoveMessage();
            tiMiddleName.RemoveMessage();
            tiShare.RemoveMessage();
        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            var result = true;
            if (!UCTextInput.CheckPanelForm(panelForm))
                result = false;

            if (int.Parse(tiShare.Text) < 0 || int.Parse(tiShare.Text) > 100)
            {
                tiShare.SetMessage("Значение должно быть от 0 до 100", false);
                result = false;
            }

            if (!result)
                return;

            CreateRealtorRequest createRealtorRequest = new CreateRealtorRequest()
            {
                Name = tiName.Text,
                LastName = tiLastName.Text,
                MiddleName = tiMiddleName.Text,
                ShareCommission = int.Parse(tiShare.Text),
            };
            var realtor = await Context.dBClient.CreateRealtor(createRealtorRequest);

            NavigationService.Navigate(new NullPage());
            NavigationService.Navigate(new RealtorPage(realtor));
        }

        private void btnCancelCreate_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            panelBtnsEditSettings.Visibility = Visibility.Visible;
            panelBtnToEdit.Visibility = Visibility.Collapsed;
            UCTextInput.ToEdit(panelForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Context.dBClient.DeleteRealtor(_realtor.Id);
            NavigationService.Navigate(new ListPersonPage(1));
        }

        private async void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            var result = true;
            if (!UCTextInput.CheckPanelForm(panelForm))
                result = false;

            if (int.Parse(tiShare.Text) < 0 || int.Parse(tiShare.Text) > 100)
            {
                tiShare.SetMessage("Значение должно быть от 0 до 100", false);
                result = false;
            }

            if (!result)
                return;

            UpdateRealtorRequest updateRealtorRequest = new UpdateRealtorRequest()
            {
                Id = _realtor.Id,
                Name = tiName.Text,
                LastName = tiLastName.Text,
                MiddleName = tiMiddleName.Text,
                ShareCommission = int.Parse(tiShare.Text),
            };

            var realtor = await Context.dBClient.UpdateRealtor(updateRealtorRequest);

            NavigationService.Navigate(new NullPage());
            NavigationService.Navigate(new RealtorPage(realtor));
        }

        private void btnCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            OnAppearing();
            panelBtnToEdit.Visibility = Visibility.Visible;
            panelBtnsEditSettings.Visibility = Visibility.Collapsed;
            UCTextInput.ToCollapsed(panelForm);
        }
    }
}
