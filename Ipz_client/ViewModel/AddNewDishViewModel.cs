using Ipz_client.Commands;
using Ipz_client.Models.Request.Dish;
using Ipz_client.Models.Response;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;

namespace Ipz_client.ViewModel
{
    class AddNewDishViewModel : BaseViewModel
    {
        public DishToRestaurantRequestDto DishToRestaurantRequestDto { get; set; } = new DishToRestaurantRequestDto();

        private ObservableCollection<RestaurantResponseDto> _restraunts;
        public ObservableCollection<RestaurantResponseDto> Restraunts
        {
            get
            {
                return _restraunts;
            }
            set
            {
                _restraunts = value;
                OnPropertyChanged(nameof(Restraunts));
            }
        }

        private RestaurantResponseDto _selectedRestraunt;
        public RestaurantResponseDto SelectedRestraunt
        {
            get { return _selectedRestraunt; }
            set
            {
                _selectedRestraunt = value;
                OnPropertyChanged(nameof(SelectedRestraunt));
            }
        }

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

        public ICommand AddNewDishCommand { get; set; }
        public ICommand UpdateViewCommand { get; set; }

        public AddNewDishViewModel()
        {
            AddNewDishCommand = new RelayCommand(AddNewDishExecuteAsync, AddNewDishUpdateCanExecute);
            UpdateViewCommand = new ViewCommand(this);

            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            var apiResponse = await ServerRequest.SendAsync(Paths.GetRestaurants, null, RequestTypes.Get);

            if (!apiResponse.Success)
            {
                MessageBox.Show("Error while getting restraunts");
                return;
            }

            var restrauntsJson = apiResponse.Data.ToString();
            var restraunts = JsonConvert.DeserializeObject<List<RestaurantResponseDto>>(restrauntsJson);

            Restraunts = new ObservableCollection<RestaurantResponseDto>(restraunts);
            SelectedRestraunt = Restraunts.FirstOrDefault();
        }

        private bool AddNewDishUpdateCanExecute(object obj)
        {
            return true;
        }
        public List<ValidationResult>? ValidateRegistrationRequest()
        {
            var context = new ValidationContext(DishToRestaurantRequestDto, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(DishToRestaurantRequestDto, context, results, true);

            return results;
        }

        private async void AddNewDishExecuteAsync(object obj)
        {
            DishToRestaurantRequestDto.RestaurantId = SelectedRestraunt.RestaurantId;

            var validateResult = ValidateRegistrationRequest();
            if (validateResult != null && validateResult.Count > 0)
            {
                MessageBox.Show(validateResult.FirstOrDefault().ErrorMessage);
                return;
            }

            var apiResponse = await ServerRequest.SendAsync(Paths.CreateDish, DishToRestaurantRequestDto, RequestTypes.Post);

            if (apiResponse.Success)
            {
                MessageBox.Show("Dish created successfully");
            }
            else
            {
                MessageBox.Show(apiResponse.Errors.First());
            }
        }
    }
}
