using Newtonsoft.Json;
using Recipe_App_WPF.Extensions;
using Recipe_App_WPF.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Recipe_App_WPF.ViewModel
{
    public class SignUpViewModel : ViewModelBase
    {
        private SignUpModel _signUpModel;
        private bool _isViewVisible;

        public string Name
        {
            get
            {
                return _signUpModel.SignUpUserAccount.Name;
            }

            set
            {
                _signUpModel.SignUpUserAccount.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Email
        {
            get
            {
                return _signUpModel.SignUpUserAccount.Email;
            }
            set
            {
                _signUpModel.SignUpUserAccount.Email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public SecureString Password
        {
            get
            {
                return _signUpModel.SignUpUserAccount.Password;
            }
            set
            {
                _signUpModel.SignUpUserAccount.Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string RegisterMessage
        {
            get
            {
                return _signUpModel.RegisterMessage;
            }
            set
            {
                _signUpModel.RegisterMessage = value;
                OnPropertyChanged(nameof(RegisterMessage));
            }
        }

        public bool IsViewVisible
        {
            get
            {
                return _isViewVisible;
            }
            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }

        public ICommand RegisterCommand { get; }
        public ICommand CloseSignUpFormCommand { get; }
        public SignUpViewModel()
        {
            _signUpModel = new SignUpModel();
            IsViewVisible = true;
            RegisterCommand = new ViewModelCommand(ExecuteRegisterCommand, CanExecuteRegisterCommand);
            CloseSignUpFormCommand = new ViewModelCommand(ExecuteCloseSignUpFormCommand);
        }

        private void ExecuteCloseSignUpFormCommand(object obj)
        {
          IsViewVisible = false;
        }

        private bool CanExecuteRegisterCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(Email)
                || Email.Length < 3
                || Password == null
                || Password.Length < 5
                || !Email.Contains("@"))
                validData = false;
            else
                validData = true;

            return validData;
        }

        private async void ExecuteRegisterCommand(object obj)
        {
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "email", Email },
                    { "password", SecureStringExtensions.ToUnsecuredString(Password) },
                    { "name", Name}
                };

                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync("http://localhost:8000/api/user/create/", content);

                if (response.IsSuccessStatusCode)
                {
                    // Login was successful, read the token
                    RegisterMessage = "You have successfully registered!";
                }
                else
                {
                    RegisterMessage = " * Something went wrong upon registeration";
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Response Content: {responseContent}");
                }
            }
        }
    }
}
