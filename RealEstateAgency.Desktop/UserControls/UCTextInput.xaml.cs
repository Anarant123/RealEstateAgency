using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
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
    public partial class UCTextInput : UserControl , INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string message;
        private string placeholder;
        private string text;
        private bool isDigit;
        private bool isNullable;


        public SolidColorBrush Color { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B0BEB5"));
        public SolidColorBrush TextColor { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78909C"));
        public SolidColorBrush ColorOnFocus { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#04A0FF"));
        public Visibility VisibilitySettings { get; set; } = Visibility.Collapsed;

        private bool? IsSuccess
        {
            set 
            { 
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
                OnPropertyChanged(nameof(Color));
            }
        }

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                if (string.IsNullOrEmpty(message))
                    VisibilitySettings = Visibility.Collapsed;
                else
                    VisibilitySettings = Visibility.Visible;
                OnPropertyChanged(nameof(VisibilitySettings));
                OnPropertyChanged(nameof(Message));
            }
        }
        public string Text 
        {
            get { return text; }
            set 
            {
                text = value;
                OnPropertyChanged(nameof(Text)); 
            }
        }

        public string Placeholder 
        {
            get { return placeholder; }
            set 
            { 
                placeholder = value;
                OnPropertyChanged(nameof(Placeholder)); 
            } 
        }

        public bool IsDigit
        {
            get { return isDigit; }
            set 
            { 
                isDigit = value;
                OnPropertyChanged(nameof(IsDigit)); 
            }
        }

        public bool IsNullable
        {
            get { return isNullable; }
            set
            {
                isNullable = value;
                OnPropertyChanged(nameof(IsNullable));
            }
        }


        public UCTextInput()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public void SetMessage(string message, bool isSuccess)
        {
            Message = message;
            IsSuccess = isSuccess;
        }

        public void SuccessMessage()
        {
            Message = "Все прекрасно";
            IsSuccess = true;
        }

        public void RemoveMessage()
        {
            Message = "";
            IsSuccess = null;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsDigit) return;
            string inputText = (sender as TextBox).Text + e.Text;
            if (!Regex.IsMatch(inputText, @"^[0-9]*(\.[0-9]*)?$")) e.Handled = true;
        }

        public bool IsNullOrEmpty(string text)
        {
            return string.IsNullOrEmpty(text);
        }

        public static bool CheckPanelForm (Panel panel)
        {
            var result = true;
            foreach (var child in panel.Children)
            {
                if (child is UCTextInput textInput)
                {
                    if (textInput.IsNullable != true && textInput.IsNullOrEmpty(textInput.Text))
                    {
                        textInput.SetMessage("Это поле не может быть пустым", false);
                        result = false;
                    }
                }
            }
            return result;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsNullable != true && IsNullOrEmpty((sender as TextBox).Text))
            {
                SetMessage("Это поле не может быть пустым", false);
                return;
            }

            SuccessMessage();
        }

        public static void ToCollapsed(Panel panel)
        {
            foreach (var child in panel.Children)
            {
                if (child is UCTextInput textInput)
                {
                    textInput.RemoveMessage();
                    textInput.IsEnabled = false;
                    if (textInput.IsNullOrEmpty(textInput.Text))
                    {
                        textInput.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        public static void ToEdit(Panel panel)
        {
            foreach (var child in panel.Children)
            {
                if (child is UCTextInput textInput)
                {
                    textInput.IsEnabled = false;
                    textInput.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
