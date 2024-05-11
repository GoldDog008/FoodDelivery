using Ipz_client.Commands;
using Ipz_client.Models.Request.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ipz_client.ViewModel
{
    internal class RegistrationViewModel : BaseViewModel
    {
        RegistrationRequestDto RegistrationRequestDto { get; set; } = new RegistrationRequestDto();

        public ICommand RegistrationCommand { get; set; }
        public RegistrationViewModel() 
        { 
            RegistrationCommand = new RelayCommand(RegistrationExecuteAsync, RegistrationCanExecute);

        }

        private bool RegistrationCanExecute(object obj)
        {
            return true; 
        }

        private void RegistrationExecuteAsync(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
