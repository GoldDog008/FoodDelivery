using Ipz_client.Commands;
using Ipz_client.Models;
using Ipz_client.Models.Request.Order;
using Ipz_client.Models.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

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

                TotalPrice = 0;
                OnPropertyChanged(nameof(TotalPrice));

                if (OrderInformations != null)
                {
                    OrderInformations.Clear();
                    OnPropertyChanged(nameof(OrderInformations));

                    SelectedOrderInformation = null;
                    OnPropertyChanged(nameof(SelectedOrderInformation));
                }

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

        private ObservableCollection<OrderInformation> _orderInformations;
        public ObservableCollection<OrderInformation> OrderInformations
        {
            get { return _orderInformations; }
            set
            {
                _orderInformations = value;
                OnPropertyChanged(nameof(OrderInformations));
            }
        }

        private OrderInformation _selectedOrderInformation;
        public OrderInformation SelectedOrderInformation
        {
            get { return _selectedOrderInformation; }
            set
            {
                _selectedOrderInformation = value;
                OnPropertyChanged(nameof(SelectedOrderInformation));
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

                if (SelectedDish != null)
                {
                    SelectedDishPrice = SelectedDish.Price * Quantity;
                    OnPropertyChanged(nameof(SelectedDishPrice));
                }
            }
        }

        public decimal SelectedDishPrice { get; set; }
        public decimal TotalPrice { get; set; } = 0;

        public ICommand UpdateViewCommand { get; set; }
        public ICommand CreateOrderCommand { get; set; }
        public ICommand AddOrderCommand { get; set; }
        public ICommand RemoveSelectedCommand { get; set; }

        public NewOrderViewModel()
        {
            CreateOrderCommand = new RelayCommand(NewOrderExecuteAsync, NewOrderCanExecute);
            AddOrderCommand = new RelayCommand(AddOrderExecuteAsync, AddOrderCanExecute);
            RemoveSelectedCommand = new RelayCommand(RemoveSelectedExecuteAsync, RemoveSelectedCanExecute);
            UpdateViewCommand = new ViewCommand(this);

            OrderInformations = new ObservableCollection<OrderInformation>();

            InitializeAsync();
        }

        private void RemoveSelectedExecuteAsync(object obj)
        {
            if (SelectedOrderInformation != null)
            {
                TotalPrice -= SelectedOrderInformation.Price;
                OnPropertyChanged(nameof(TotalPrice));

                OrderInformations.Remove(SelectedOrderInformation);
            }
        }

        private void AddOrderExecuteAsync(object obj)
        {
            if (Quantity < 1 || Quantity > 99)
            {
                MessageBox.Show("The quantity must be between 0..99");
                return;
            }

            var orderInformation = OrderInformations.FirstOrDefault(x => x.DishId == SelectedDish.DishId);

            if (orderInformation != null)
            {
                OrderInformations.Remove(orderInformation);
                orderInformation.Quantity += Quantity;
                orderInformation.Price = orderInformation.Quantity * SelectedDish.Price;
                OrderInformations.Add(orderInformation);
            }
            else
            {
                orderInformation = new OrderInformation
                {
                    Quantity = Quantity,
                    DishId = SelectedDish.DishId,
                    Name = SelectedDish.Name,
                    Price = SelectedDish.Price * Quantity
                };

                OrderInformations.Add(orderInformation);
            }

            SelectedOrderInformation = orderInformation;

            TotalPrice += SelectedDish.Price * Quantity;
            OnPropertyChanged(nameof(TotalPrice));
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

        private async void NewOrderExecuteAsync(object obj)
        {
            var OrderInformationsRequest = new List<OrderInformationCreateRequestDto>();

            foreach (var orderInformation in OrderInformations)
            {
                OrderInformationsRequest.Add(new OrderInformationCreateRequestDto
                {
                    Quantity = orderInformation.Quantity,
                    DishId = orderInformation.DishId
                });
            }

            OrderCreateRequestDto requestDto = new OrderCreateRequestDto
            {
                RestaurantId = SelectedRestraunt.RestaurantId,
                OrderInformations = OrderInformationsRequest
            };

            var apiResponse = await ServerRequest.SendAsync(Paths.CreateOrder, requestDto, RequestTypes.Post);

            if (apiResponse.Success)
            {
                MessageBox.Show("Order successfuly complete");
                OrderInformations.Clear();

                TotalPrice = 0;
                OnPropertyChanged(nameof(TotalPrice));
            }
            else
            {
                MessageBox.Show(apiResponse.Errors.First());
            }
        }

        private bool RemoveSelectedCanExecute(object obj)
        {
            return true;
        }

        private bool NewOrderCanExecute(object obj)
        {
            return true;
        }

        private bool AddOrderCanExecute(object obj)
        {
            return true;
        }
    }
}
