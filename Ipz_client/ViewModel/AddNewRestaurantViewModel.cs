using Ipz_client.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ipz_client.ViewModel
{
    class AddNewRestaurantViewModel : BaseViewModel
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
        public ICommand AddNewRestaurantCommand { get; set; }
        public ICommand UpdateViewCommand { get; set; }

        public AddNewRestaurantViewModel()
        {
            AddNewRestaurantCommand = new RelayCommand(AddNewRestaurantExecuteAsync, AddNewRestaurantUpdateCanExecute);
            UpdateViewCommand = new ViewCommand(this);
        }
        private bool AddNewRestaurantUpdateCanExecute(object obj)
        {
            return true;
        }

        private async void AddNewRestaurantExecuteAsync(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
