using Ipz_client.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ipz_client.ViewModel
{
    class AddNewDishViewModel : BaseViewModel
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
        public ICommand AddNewDishCommand { get; set; }
        public ICommand UpdateViewCommand { get; set; }

        public AddNewDishViewModel()
        {
            AddNewDishCommand = new RelayCommand(AddNewDishExecuteAsync, AddNewDishUpdateCanExecute);
            UpdateViewCommand = new ViewCommand(this);
        }
        private bool AddNewDishUpdateCanExecute(object obj)
        {
            return true;
        }

        private async void AddNewDishExecuteAsync(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
