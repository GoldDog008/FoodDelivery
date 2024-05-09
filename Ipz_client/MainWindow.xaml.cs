using Ipz_client.Models.Request.Auth;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ipz_client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //InitializeComponent();
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            var loginRequest = new LoginRequestDto
            {
                Email = Email.Text,
                Password = Password.Password
            };
        }
    }
}