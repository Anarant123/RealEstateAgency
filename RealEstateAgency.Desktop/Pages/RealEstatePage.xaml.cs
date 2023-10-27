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
    /// Логика взаимодействия для RealEstatePage.xaml
    /// </summary>
    public partial class RealEstatePage : Page
    {
        public RealEstatePage()
        {
            InitializeComponent();
        }

        private void btnAddAddress_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            btn.Visibility = Visibility.Collapsed;
            panelAddress.Visibility = Visibility.Visible;
        }

        private void btnAddCoordinates_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            btn.Visibility = Visibility.Collapsed;
            panelCoordinate.Visibility = Visibility.Visible;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
