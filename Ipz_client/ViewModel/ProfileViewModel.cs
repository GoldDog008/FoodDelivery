using Ipz_client.Commands;
using Ipz_client.Models;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;

namespace Ipz_client.ViewModel
{
    class ProfileViewModel : BaseViewModel
    {
        public UserUpdateRequest User { get; set; }

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
        public ICommand UpdateViewCommand { get; set; }
        public ICommand UpdateProfileCommand { get; set; }
        public ProfileViewModel()
        {
            UpdateProfileCommand = new RelayCommand(ProfileUpdateExecuteAsync, ProfileUpdateCanExecute);
            UpdateViewCommand = new ViewCommand(this);

            User = new UserUpdateRequest();
            SetUser();

        }

        private void SetUser()
        {
            User.City = CurrentUser.City;
            User.Country = CurrentUser.Country;
            User.FirstName = CurrentUser.FirstName;
            User.LastName = CurrentUser.LastName;
            User.Phone = CurrentUser.Phone;
            User.Street = CurrentUser.Street;
        }

        private bool ProfileUpdateCanExecute(object obj)
        {
            return true;
        }

        public List<ValidationResult>? ValidateUserUpdateRequest()
        {
            var context = new ValidationContext(User, serviceProvider: null, items: null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            Validator.TryValidateObject(User, context, results, true);

            return results;
        }

        private async void ProfileUpdateExecuteAsync(object obj)
        {
            var validateResult = ValidateUserUpdateRequest();
            if (validateResult != null && validateResult.Count > 0)
            {
                MessageBox.Show(validateResult.FirstOrDefault().ErrorMessage);
                return;
            }

            var apiResponse = await ServerRequest.SendAsync(Paths.UpdateUser, User, RequestTypes.Put);

            if (apiResponse.Success)
            {
                MessageBox.Show("Successfuly update");
                CurrentUser.SetCurrentUser(User);
            }
            else
            {
                MessageBox.Show(apiResponse.Errors.First());
            }
        }
    }
}
