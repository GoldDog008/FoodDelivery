using Ipz_client.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ipz_client.ViewModel
{
    class AllOrdersViewModel : BaseViewModel
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
        public ICommand AllOrdersCommand { get; set; }
        public ICommand UpdateViewCommand { get; set; }

        public AllOrdersViewModel()
        {
            AllOrdersCommand = new RelayCommand(AllOrdersExecuteAsync, AllOrdersUpdateCanExecute);
            UpdateViewCommand = new ViewCommand(this);
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
