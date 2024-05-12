using Ipz_client.Commands;
using Ipz_client.Models;
using Ipz_client.Models.Request.Auth;
using Ipz_client.Models.Response;
using Ipz_client.Views;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Reflection.Metadata;
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
        public RegistrationRequestDto RegistrationRequestDto { get; set; } = new RegistrationRequestDto();

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

        public List<System.ComponentModel.DataAnnotations.ValidationResult>? ValidateRegistrationRequest()
        {
            var context = new ValidationContext(RegistrationRequestDto, serviceProvider: null, items: null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            Validator.TryValidateObject(RegistrationRequestDto, context, results, true);

            return results;
        }

        private async void RegistrationExecuteAsync(object obj)
        {
            var objs = obj as object[];

            var password = objs[0] as PasswordBox;
            var confirmPassword = objs[1] as PasswordBox;

            RegistrationRequestDto.Password = password.Password;
            RegistrationRequestDto.ConfirmPassword = confirmPassword.Password;

            var validateResult = ValidateRegistrationRequest();
            if (validateResult != null && validateResult.Count > 0)
            {
                MessageBox.Show(validateResult.FirstOrDefault().ErrorMessage);
                return;
            }

            var apiResponse = await ServerRequest.SendAsync(Paths.Registration, RegistrationRequestDto, RequestTypes.Post);

            if (apiResponse.Success)
            {
                MessageBox.Show("Registration success");

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