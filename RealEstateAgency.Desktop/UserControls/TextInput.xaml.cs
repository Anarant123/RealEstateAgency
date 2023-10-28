using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace RealEstateAgency.Desktop.UserControls
{
    /// <summary>
    /// Логика взаимодействия для TextInput.xaml
    /// </summary>
    public partial class TextInput : UserControl
    {
        private bool? isSuccess;
        private string message;
        public SolidColorBrush Color { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B0BEB5"));
        public SolidColorBrush TextColor { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78909C"));
        public SolidColorBrush ColorOnFocus { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#04A0FF"));
        public Visibility VisibilitySettings { get; set; } = Visibility.Collapsed;

        private bool? IsSuccess
        {
            set 
            { 
                isSuccess = value;
                switch (value)
                {
                    case true:
                        Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00D9BB"));
                        break;

                    case false:
                        Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF1744"));
                        break;

                    case null:
                        Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B0BEB5"));
                        break;
                }
            }
        }

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                if (string.IsNullOrEmpty(message))
                {
                    VisibilitySettings = Visibility.Collapsed;
                }
                else
                {
                    VisibilitySettings = Visibility.Visible;
                }
            }
        }
        public string Text { get; set; }

        public string Placeholder { get; set; }

        public bool IsDigit { get; set; }

        public TextInput()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public void SetMessage(string message, bool isSuccess)
        {
            Message = message;
            IsSuccess = isSuccess;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsDigit) return;
            string inputText = (sender as TextBox).Text + e.Text;
            if (!Regex.IsMatch(inputText, @"^[0-9]*(\.[0-9]*)?$")) e.Handled = true;
        }
    }
}
