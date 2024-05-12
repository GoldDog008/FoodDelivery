using Ipz_client.Commands;
using System.Windows.Input;

namespace Ipz_client.ViewModel
{
    internal class NewOrderViewModel : BaseViewModel
    {
        public List<string> Restraunts { get; set; } = new List<string>();
        public List<string> Dishes { get; set; } = new List<string>();

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
        public ICommand NewOrderCommand { get; set; }
        public NewOrderViewModel()
        {
            NewOrderCommand = new RelayCommand(NewOrderExecuteAsync, NewOrderCanExecute);
            UpdateViewCommand = new ViewCommand(this);

            Restraunts.Add("Restraunt1");
            Restraunts.Add("Restraunt2");
            Restraunts.Add("Restraunt3");

            Dishes.Add("Dish1");
            Dishes.Add("Dish2");
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
