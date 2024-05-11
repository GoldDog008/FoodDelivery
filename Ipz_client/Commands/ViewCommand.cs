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
        private LoginViewModel _loginViewModel;
        public event EventHandler CanExecuteChanged;

        public ViewCommand(LoginViewModel loginViewModel)
        {
            _loginViewModel = loginViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter.ToString() == "Registration")
            {
                _loginViewModel.SelectedViewModel = new RegistrationViewModel();
            }
            else if (parameter.ToString() == "Login")
            {
                _loginViewModel.SelectedViewModel = new LoginViewModel();
            }
        }
    }
}
