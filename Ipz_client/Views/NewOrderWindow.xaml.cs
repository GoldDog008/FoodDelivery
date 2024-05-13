using Ipz_client.Models;
using Ipz_client.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ipz_client.Views
{
    /// <summary>
    /// Логика взаимодействия для NewOrder.xaml
    /// </summary>
    public partial class NewOrderWindow : UserControl
    {
        public NewOrderWindow()
        {
            InitializeComponent();
            if (CurrentUser.IsAdmin)
            {
                AdminMenu.Visibility = Visibility.Visible;
            }
            else
            {
                AdminMenu.Visibility = Visibility.Collapsed;
            }
            DataContext = new NewOrderViewModel();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.Close();
            }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.WindowState = WindowState.Minimized;
            }
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.DragMove();
            }
        }
    }
}
