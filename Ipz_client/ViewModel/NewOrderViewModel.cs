using Ipz_client.Commands;
using Ipz_client.Models.Request.Order;
using Ipz_client.Models.Response;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Ipz_client.ViewModel
{
    public class NewOrderViewModel : BaseViewModel
    {
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

        private ObservableCollection<DishResponseDto> _dishes;
        public ObservableCollection<DishResponseDto> Dishes
        {
            get
            {
                return _dishes;
            }
            set
            {
                _dishes = value;
                OnPropertyChanged(nameof(Dishes));
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
        private RestaurantResponseDto _selectedRestraunt;
        public RestaurantResponseDto SelectedRestraunt
        {
            get { return _selectedRestraunt; }
            set
            {
                _selectedRestraunt = value;
                LoadDishes(value);
                OnPropertyChanged(nameof(SelectedRestraunt));
            }
        }
        private DishResponseDto _selectedDish;
        public DishResponseDto SelectedDish
        {
            get { return _selectedDish; }
            set
            {
                _selectedDish = value;
                OnPropertyChanged(nameof(SelectedDish));
                Quantity = 1;
            }
        }

        public int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
                TotalPrice = SelectedDish.Price * Quantity;
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        public decimal TotalPrice { get; set; }

        public ICommand UpdateViewCommand { get; set; }
        public ICommand NewOrderCommand { get; set; }
        public ICommand AddOrderCommand { get; set; }

        public NewOrderViewModel()
        {
            NewOrderCommand = new RelayCommand(NewOrderExecuteAsync, NewOrderCanExecute);
            AddOrderCommand = new RelayCommand(AddOrderExecuteAsync, AddOrderCanExecute);
            UpdateViewCommand = new ViewCommand(this);

            InitializeAsync();
        }

        private bool AddOrderCanExecute(object obj)
        {
            return true;
        }

        private void AddOrderExecuteAsync(object obj)
        {
            throw new NotImplementedException();
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
            LoadDishes(SelectedRestraunt);
        }
        private void LoadDishes(RestaurantResponseDto restaurant)
        {
            if (Dishes == null)
            {
                Dishes = new ObservableCollection<DishResponseDto>();
            }

            Dishes.Clear();
            Dishes = new ObservableCollection<DishResponseDto>(restaurant.Dishes.ToList());
            SelectedDish = Dishes.FirstOrDefault();
        }

        private bool NewOrderCanExecute(object obj)
        {
            return true;
        }

        private async void NewOrderExecuteAsync(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
