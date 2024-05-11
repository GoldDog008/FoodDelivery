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
    public class LoginViewModel : BaseViewModel
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

        public LoginRequestDto LoginRequestDto { get; set; } = new LoginRequestDto();

        public ICommand LoginCommand { get; set; }
        public ICommand UpdateViewCommand { get; set; }
        public ICommand ShowRegistrationWindowCommand { get; set; }

        public LoginViewModel()
        {
            ShowRegistrationWindowCommand = new RelayCommand(ShowRegistrationWindowExecute, ShowRegistrationWindowCanExecute);
            LoginCommand = new RelayCommand(LoginExecuteAsync, LoginCanExecute);
            UpdateViewCommand = new ViewCommand(this);
        }

        public bool IsValidLoginRequest()
        {
            var context = new ValidationContext(LoginRequestDto, serviceProvider: null, items: null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var isValid = Validator.TryValidateObject(LoginRequestDto, context, results, true);

            return isValid;

            //if (!isValid)
            //{
            //    List<string> errors = new List<string>();
            //    foreach (var error in results)
            //    {
            //        errors.Add(error.ErrorMessage);

            //    }

            //    return error.ErrorMessage;
            //}
        }

        private bool LoginCanExecute(object obj)
        {
            return true;
        }

        private async void LoginExecuteAsync(object obj)
        {
            PasswordBox box = (PasswordBox)obj;
            LoginRequestDto.Password = box.Password;

            if (!IsValidLoginRequest())
            {
                MessageBox.Show("Possible errors:\nInvalid email\nPasword must contains minimum 8 character");
                return;
            }

            var data = JsonConvert.SerializeObject(LoginRequestDto);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(Paths.Login, content);

            var jsonApiResponse = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(jsonApiResponse);

            if (apiResponse.Success)
            {
                MessageBox.Show("Login success");

                var userJson = apiResponse.Data.ToString();
                var user = JsonConvert.DeserializeObject<UserAuthResponseDto>(userJson);

                SetCurrentUser(user);
            }
            else
            {
                MessageBox.Show(apiResponse.Errors.First());
            }
        }

        private static void SetCurrentUser(UserAuthResponseDto user)
        {
            CurrentUser.UserId = user.UserId;
            CurrentUser.FirstName = user.FirstName;
            CurrentUser.LastName = user.LastName;
            CurrentUser.Email = user.Email;
            CurrentUser.Phone = user.Phone;
            CurrentUser.Country = user.Country;
            CurrentUser.City = user.City;
            CurrentUser.Street = user.Street;
            CurrentUser.AccessToken = user.AccessToken;
        }

        private bool ShowRegistrationWindowCanExecute(object obj)
        {
            return true;
        }

        private void ShowRegistrationWindowExecute(object obj)
        {
            //RegistrationWindow registrationWindow = new RegistrationWindow();
            //registrationWindow.Show();
        }
    }
}
