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
                    _profileViewModel.SelectedViewModel = new NewOrderViewModel();
                    break;
                case "Profile":
                    _newOrderViewModel.SelectedViewModel = new ProfileViewModel();
                    break;
            }
        }

    }
}
