using Ipz_client.Commands;
using Ipz_client.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Ipz_client.ViewModel
{
    class ProfileViewModel : BaseViewModel
    {
        private DateTime _currentTime;
        public DateTime CurrentTime
        {
            get { return _currentTime; }
            set
            {
                _currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }

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

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
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

        private void Timer_Tick(object? sender, EventArgs e)
        {
            GetServerTime();
        }

        private async void GetServerTime()
        {
            var apiResponse = await ServerRequest.SendAsync(Paths.GetCurrentTime, null, RequestTypes.Get);
            if (apiResponse.Success)
            {
                var dateTimeJson = apiResponse.Data.ToString();
                CurrentTime = DateTime.Parse(dateTimeJson);
            }
        }
    }
}
