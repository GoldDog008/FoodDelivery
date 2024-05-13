using Ipz_client.Models;
using Ipz_client.ViewModel;
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

namespace Ipz_client.Views
{
    /// <summary>
    /// Логика взаимодействия для AllOrders.xaml
    /// </summary>
    public partial class AllOrdersWindow : UserControl
    {
        public AllOrdersWindow()
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
            DataContext = new AllOrdersViewModel();
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
