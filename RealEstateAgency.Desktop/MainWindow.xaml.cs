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
            frameMain.NavigationService.Navigate(new ClientPage());
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
            frameMain.NavigationService.Navigate(new ListPersonPage());
        }
        private void btnMenuRealtor_Click(object sender, RoutedEventArgs e)
        {
            frameMain.NavigationService.Navigate(new ListPersonPage());
        }

        private void btnMenuRealEstate_Click(object sender, RoutedEventArgs e)
        {
            frameMain.NavigationService.Navigate(new ListRealEstatePage());
        }

        private void btnMenuNeed_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnMenuOffer_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
