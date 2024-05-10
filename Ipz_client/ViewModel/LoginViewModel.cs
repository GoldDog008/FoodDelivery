using Ipz_client.Commands;
using Ipz_client.Models.Request.Auth;
using Ipz_client.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ipz_client.ViewModel
{
    public class LoginViewModel
    {
        public LoginRequestDto LoginRequestDto { get; set; } = new LoginRequestDto();

        public ICommand LoginCommand { get; set; }
        public ICommand ShowRegistrationWindowCommand { get; set; }

        public LoginViewModel()
        {
            ShowRegistrationWindowCommand = new RelayCommand(ShowRegistrationWindowExecute, ShowRegistrationWindowCanExecute);
            LoginCommand = new RelayCommand(LoginExecute, LoginCanExecute);
        }

        public bool IsLoginRequestValid()
        {
            var context = new ValidationContext(LoginRequestDto, serviceProvider: null, items: null);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var isValid = Validator.TryValidateObject(LoginRequestDto, context, results, true);

            return isValid;
        }

        private bool LoginCanExecute(object obj)
        {
            return true;
        }

        private void LoginExecute(object obj)
        {
            PasswordBox box = (PasswordBox)obj;
            LoginRequestDto.Password = box.Password;

            if (!IsLoginRequestValid())
            {
                MessageBox.Show("Possible errors:\nInvalid email\nPasword must contains minimum 8 character");
                return;
            }

            //TODO: send request to server
            //https://localhost:7067/
            //POST /api/auth/login

        }

        private bool ShowRegistrationWindowCanExecute(object obj)
        {
            return true;
        }

        private void ShowRegistrationWindowExecute(object obj)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
        }
    }
}
