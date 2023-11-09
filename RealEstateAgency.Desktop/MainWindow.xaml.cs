using RealEstateAgency.Desktop.Pages;
using RealEstateAgency.Desktop.Pages.ClientPages;
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

namespace RealEstateAgency.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Context.dBClient = new DBClient.DBClient(new DBClient.Data.Models.RealEstateAgencyContext());
            frameMain.NavigationService.Navigate(new NullPage());
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if(WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
        }

        private void btnMinimaze_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnMenuClient_Click(object sender, RoutedEventArgs e)
        {
            frameMain.NavigationService.Navigate(new ListPersonPage(1));
            lbNamePage.Text = "Клиенты";
        }
        private void btnMenuRealtor_Click(object sender, RoutedEventArgs e)
        {
            frameMain.NavigationService.Navigate(new ListPersonPage(2));
            lbNamePage.Text = "Риэлторы";
        }

        private void btnMenuRealEstate_Click(object sender, RoutedEventArgs e)
        {
            frameMain.NavigationService.Navigate(new ListRealEstatePage());
            lbNamePage.Text = "Недвижимость";
        }

        private void btnMenuNeed_Click(object sender, RoutedEventArgs e)
        {
            frameMain.NavigationService.Navigate(new ListNeedPage());
            lbNamePage.Text = "Потребности";
        }

        private void btnMenuOffer_Click(object sender, RoutedEventArgs e)
        {
            frameMain.NavigationService.Navigate(new ListOfferPage());
            lbNamePage.Text = "Предложения";
        }

        private void btnMenuDeal_Click(object sender, RoutedEventArgs e)
        {
            frameMain.NavigationService.Navigate(new ListDealPage());
            lbNamePage.Text = "Сделки";
        }
    }
}
