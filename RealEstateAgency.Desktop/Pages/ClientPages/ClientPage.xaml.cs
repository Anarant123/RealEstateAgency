using RealEstateAgency.DBClient.Contracts.Requests;
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
        public ClientPage()
        {
            InitializeComponent();
            panelBtnToCreate.Visibility = Visibility.Visible;
        }

        public ClientPage(Client client)
        {
            InitializeComponent();
            panelBtnToEdit.Visibility = Visibility.Visible;

            tiName.Text = client.Name!;
            tiLastName.Text = client.LastName!;
            tiMiddleName.Text = client.MiddleName!;
            tiPhoneNumber.Text = client.PhoneNumber!;
            tiEmail.Text = client.Email!;

            UCTextInput.ToCollapsed(panelForm);

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

            NavigationService.Navigate(new ClientPage(client));
        }

        private void btnCancelCreate_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }


        //  Конструктор для проссмотра пользователя или редактирования
        //public ClientPage()
        //{
        //    InitializeComponent();
        //}
    }
}
