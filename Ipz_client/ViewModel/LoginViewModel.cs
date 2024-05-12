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

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(LoginExecuteAsync, LoginCanExecute);
            UpdateViewCommand = new ViewCommand(this);
        }

        public List<System.ComponentModel.DataAnnotations.ValidationResult>? ValidateLoginRequest()
        {
            var context = new ValidationContext(LoginRequestDto, serviceProvider: null, items: null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            Validator.TryValidateObject(LoginRequestDto, context, results, true);

            return results;
        }

        private bool LoginCanExecute(object obj)
        {
            return true;
        }

        private async void LoginExecuteAsync(object obj)
        {
            PasswordBox box = (PasswordBox)obj;
            LoginRequestDto.Password = box.Password;

            var validateResult = ValidateLoginRequest();
            if (validateResult != null && validateResult.Count > 0)
            {
                MessageBox.Show(validateResult.FirstOrDefault().ErrorMessage);
                return;
            }

            var apiResponse = await ServerRequest.SendAsync(Paths.Login, LoginRequestDto, RequestTypes.Post);

            if (apiResponse.Success)
            {
                MessageBox.Show("Login success");

                var userJson = apiResponse.Data.ToString();
                var user = JsonConvert.DeserializeObject<UserAuthResponseDto>(userJson);

                CurrentUser.SetCurrentUser(user);
                UpdateViewCommand.Execute("Profile");
            }
            else
            {
                MessageBox.Show(apiResponse.Errors.First());
            }
        }
    }
}
