using Ipz_client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ipz_client.Commands
{
    internal class ViewCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private LoginViewModel _loginViewModel;
        private RegistrationViewModel _registrationViewModel;
        private NewOrderViewModel _newOrderViewModel;
        private ProfileViewModel _profileViewModel;
        private AllOrdersViewModel _allOrdersViewModel;
        private AddNewRestaurantViewModel _addNewRestaurantViewModel;
        private AddNewDishViewModel _addNewDishViewModel;

        public ViewCommand(LoginViewModel loginViewModel)
        {
            _loginViewModel = loginViewModel;
        }
        public ViewCommand(RegistrationViewModel registrationViewModel)
        {
            _registrationViewModel = registrationViewModel;
        }
        public ViewCommand(NewOrderViewModel newOrderViewModel)
        {
            _newOrderViewModel = newOrderViewModel;
        }
        public ViewCommand(ProfileViewModel profileViewModel)
        {
            _profileViewModel = profileViewModel;
        }
        public ViewCommand(AllOrdersViewModel allOrdersViewModel)
        {
            _allOrdersViewModel = allOrdersViewModel;
        }
        public ViewCommand(AddNewRestaurantViewModel addNewRestaurantViewModel)
        {
            _addNewRestaurantViewModel = addNewRestaurantViewModel;
        }
        public ViewCommand(AddNewDishViewModel addNewDishViewModel)
        {
            _addNewDishViewModel = addNewDishViewModel;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var param = parameter.ToString();
            switch (param)
            {
                case "Registration":
                    _loginViewModel.SelectedViewModel = new RegistrationViewModel();
                    break;

                case "Login":
                    _registrationViewModel.SelectedViewModel = new LoginViewModel();
                    break;

                case "NewOrder":
                    if (_profileViewModel != null)
                    {
                        _profileViewModel.SelectedViewModel = new NewOrderViewModel();
                        break;
                    }
                    else if (_allOrdersViewModel != null)
                    {
                        _allOrdersViewModel.SelectedViewModel = new NewOrderViewModel();
                    }
                    break;

                case "Profile":
                    if (_loginViewModel != null)
                    {
                        _loginViewModel.SelectedViewModel = new ProfileViewModel();
                        break;
                    }
                    else if (_registrationViewModel != null)
                    {
                        _registrationViewModel.SelectedViewModel = new ProfileViewModel();
                        break;
                    }
                    else if (_newOrderViewModel != null)
                    {
                        _newOrderViewModel.SelectedViewModel = new ProfileViewModel();
                        break;
                    }
                    else if (_allOrdersViewModel != null)
                    {
                        _allOrdersViewModel.SelectedViewModel = new ProfileViewModel();
                    }
                    break;
                case "AllOrders":
                    if (_profileViewModel != null)
                    {
                        _profileViewModel.SelectedViewModel = new AllOrdersViewModel();
                        break;
                    }
                    else if (_newOrderViewModel != null)
                    {
                        _newOrderViewModel.SelectedViewModel = new AllOrdersViewModel();
                        break;
                    }
                    break;
                case "NewRestaraunt":
                    _addNewDishViewModel.SelectedViewModel = new AddNewRestaurantViewModel();
                    break;
                case "NewDish":
                    _addNewRestaurantViewModel.SelectedViewModel = new AddNewDishViewModel();
                    break;
            }
        }

    }
}
