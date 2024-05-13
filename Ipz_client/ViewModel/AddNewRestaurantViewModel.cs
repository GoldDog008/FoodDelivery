using Ipz_client.Commands;
using Ipz_client.Models.Request.Restraunt;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;

namespace Ipz_client.ViewModel
{
    class AddNewRestaurantViewModel : BaseViewModel
    {
        public RestaurantCreateRequestDto RestaurantCreateRequest { get; set; } = new RestaurantCreateRequestDto();

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
        public ICommand AddNewRestaurantCommand { get; set; }
        public ICommand UpdateViewCommand { get; set; }

        public AddNewRestaurantViewModel()
        {
            AddNewRestaurantCommand = new RelayCommand(AddNewRestaurantExecuteAsync, AddNewRestaurantUpdateCanExecute);
            UpdateViewCommand = new ViewCommand(this);
        }
        public List<ValidationResult>? ValidateRestaurantCreateRequest()
        {
            var context = new ValidationContext(RestaurantCreateRequest, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(RestaurantCreateRequest, context, results, true);

            return results;
        }
        private bool AddNewRestaurantUpdateCanExecute(object obj)
        {
            return true;
        }

        private async void AddNewRestaurantExecuteAsync(object obj)
        {
            var validateResult = ValidateRestaurantCreateRequest();
            if (validateResult != null && validateResult.Count > 0)
            {
                MessageBox.Show(validateResult.FirstOrDefault().ErrorMessage);
                return;
            }

            var apiResponse = await ServerRequest.SendAsync(Paths.CreateRestaurant, RestaurantCreateRequest, RequestTypes.Post);

            if (apiResponse.Success)
            {
                MessageBox.Show("Restraunt created successfully");
            }
            else
            {
                MessageBox.Show(apiResponse.Errors.First());
            }
        }
    }
}
