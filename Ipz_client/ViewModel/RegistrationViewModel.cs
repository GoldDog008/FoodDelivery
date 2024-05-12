using Ipz_client.Commands;
using Ipz_client.Models;
using Ipz_client.Models.Request.Auth;
using Ipz_client.Models.Response;
using Ipz_client.Views;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ipz_client.ViewModel
{
    internal class RegistrationViewModel : BaseViewModel
    {
        private BaseViewModel _selectedViewModel;
        public BaseViewModel SelectedViewModel
        {
            get
            {
                return _selectedViewModel;
            }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }
        RegistrationRequestDto RegistrationRequestDto { get; set; } = new RegistrationRequestDto();

        public ICommand RegistrationCommand { get; set; }
        public ICommand UpdateViewCommand { get; set; }
        public RegistrationViewModel() 
        { 
            RegistrationCommand = new RelayCommand(RegistrationExecuteAsync, RegistrationCanExecute);
            UpdateViewCommand = new ViewCommand(this);
        }

        private bool RegistrationCanExecute(object obj)
        {
            return true; 
        }

        private async void RegistrationExecuteAsync(object obj)
        {
            var data = JsonConvert.SerializeObject(RegistrationRequestDto);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(Paths.Registration, content);

            var jsonApiResponse = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(jsonApiResponse);

            if (apiResponse.Success)
            {
                MessageBox.Show("Registration success");

                var userJson = apiResponse.Data.ToString();
                var user = JsonConvert.DeserializeObject<UserAuthResponseDto>(userJson);

                CurrentUser.SetCurrentUser(user);
            }
            else
            {
                MessageBox.Show(apiResponse.Errors.First());
            }
        }

    }
}
