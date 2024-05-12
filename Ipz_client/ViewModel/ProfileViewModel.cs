using Ipz_client.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ipz_client.ViewModel
{
    class ProfileViewModel : BaseViewModel
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
        public ICommand UpdateViewCommand { get; set; }
        public ICommand ProfileCommand { get; set; }
        public ProfileViewModel()
        {
            ProfileCommand = new RelayCommand(ProfileExecuteAsync, ProfileCanExecute);
            UpdateViewCommand = new ViewCommand(this);
        }
        private bool ProfileCanExecute(object obj)
        {
            return true;
        }

        private async void ProfileExecuteAsync(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
