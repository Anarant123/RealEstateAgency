using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
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
using static System.Net.Mime.MediaTypeNames;

namespace RealEstateAgency.Desktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для DealPage.xaml
    /// </summary>
    public partial class DealPage : Page
    {
        private Deal _deal;


        public DealPage(Deal deal)
        {
            InitializeComponent();
            _deal = Context.dBClient.GetDeal(deal.Id);
            panelBtnToEdit.Visibility = Visibility.Visible;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var need = _deal.Need;
            var offer = _deal.Offer;
            tiNeed.Text = $"{need.Type.Name} ";
            if (need.PropertyAddress != null)
            {
                if (need.PropertyAddress.City != null)
                    tiNeed.Text += $"{need.PropertyAddress.City} ";
                if (need.PropertyAddress.Street != null)
                    tiNeed.Text += $"ул. {need.PropertyAddress.Street} ";
            }

            tiOffer.Text = $"{offer.RealEstate.Type.Name} {offer.Price} ";
            if (offer.RealEstate.PropertyAddress != null)
            {
                if (offer.RealEstate.PropertyAddress.City != null)
                    tiOffer.Text += $"{offer.RealEstate.PropertyAddress.City} ";
                if (offer.RealEstate.PropertyAddress.Street != null)
                    tiOffer.Text += $"ул. {offer.RealEstate.PropertyAddress.Street} ";
                if (offer.RealEstate.PropertyAddress.ApartmentNumber != null)
                    tiOffer.Text += $"кв. {offer.RealEstate.PropertyAddress.ApartmentNumber} ";
            }

            tiBuyerCommission.Text = _deal.BuyerCommission.ToString();
            tiSellerCommission.Text = _deal.SellerCommission.ToString();

            OnAppearing();
            RemoveMessage(panelInfo);
        }

        private void OnAppearing()
        {
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

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            panelBtnsEditSettings.Visibility = Visibility.Visible;
            panelBtnToEdit.Visibility = Visibility.Collapsed;
            UCTextInput.ToEdit(panelInfo);
            tiNeed.IsEnabled = false;
            tiOffer.IsEnabled = false;
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            await Context.dBClient.DeleteDeal(_deal.Id);
            NavigationService.Navigate(new ListDealPage());
        }

        private async void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (!UCTextInput.CheckPanelForm(panelInfo)) return;

            UpdateDealRequest updateDeal = new UpdateDealRequest()
            {
                Id = _deal.Id,
                SellerCommission = double.Parse(tiSellerCommission.Text),
                BuyerCommission = double.Parse(tiBuyerCommission.Text),
            };

            var dial = await Context.dBClient.UpdateDeal(updateDeal);
            NavigationService.Navigate(new NullPage());
            NavigationService.Navigate(new DealPage(dial));
        }

        private void btnCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            OnAppearing();
            panelBtnToEdit.Visibility = Visibility.Visible;
            panelBtnsEditSettings.Visibility = Visibility.Collapsed;
        }
    }
}
