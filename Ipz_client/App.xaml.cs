using System.Windows;
using Ipz_client.Views;

namespace Ipz_client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow mainWindow = new MainWindow();

            mainWindow.Content = new AddNewRestaurantWindow();  

            mainWindow.Title = "Главное окно";
            mainWindow.Show();
        }
    }

}
