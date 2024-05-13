using Ipz_client.Commands;
using Ipz_client.Models.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Ipz_client.ViewModel
{
    class AllOrdersViewModel : BaseViewModel
    {
        private ObservableCollection<OrderResponseDto> _orders;
        public ObservableCollection<OrderResponseDto> Orders
        {
            get
            {
                return _orders;
            }
            set
            {
                _orders = value;
                OnPropertyChanged(nameof(Orders));
            }
        }

        private OrderResponseDto _selectedOrder;
        public OrderResponseDto SelectedOrder
        {
            get
            {
                return _selectedOrder;
            }
            set
            {
                _selectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));
                OnPropertyChanged(nameof(OrderInfo));
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

        public string OrderInfo
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                foreach (var orderInfo in SelectedOrder.OrderInformations)
                {
                    sb.Append(orderInfo.ToString());
                    sb.Append("\n");
                }

                return sb.ToString();
            }
        }

        public ICommand AllOrdersCommand { get; set; }
        public ICommand UpdateViewCommand { get; set; }

        public AllOrdersViewModel()
        {
            AllOrdersCommand = new RelayCommand(AllOrdersExecuteAsync, AllOrdersUpdateCanExecute);
            UpdateViewCommand = new ViewCommand(this);

            InitializeAsync();
        }
        private async void InitializeAsync()
        {
            var apiResponse = await ServerRequest.SendAsync(Paths.GetOrders, null, RequestTypes.Get);

            if (!apiResponse.Success)
            {
                MessageBox.Show("Error while getting orders");
                return;
            }

            var ordersJson = apiResponse.Data.ToString();
            var orders = JsonConvert.DeserializeObject<List<OrderResponseDto>>(ordersJson);

            Orders = new ObservableCollection<OrderResponseDto>(orders);
            //SelectedRestraunt = Restraunts.FirstOrDefault();
        }

        private bool AllOrdersUpdateCanExecute(object obj)
        {
            return true;
        }

        private async void AllOrdersExecuteAsync(object obj) 
        { 
            throw new NotImplementedException();
        }
    }
}
