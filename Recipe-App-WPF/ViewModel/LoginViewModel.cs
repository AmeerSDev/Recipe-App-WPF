using Newtonsoft.Json;
using Recipe_App_WPF.Extensions;
using Recipe_App_WPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Recipe_App_WPF.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private string _email;
        private bool _isSignUpPopupOpen;
        private SecureString _password;
        private string _errorMessage;
        private bool _isViewVisible = true;

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public SecureString Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
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
        public bool IsSignUpPopupOpen
        {
            get { return _isSignUpPopupOpen; }
            set
            {
                if (_isSignUpPopupOpen == value) return;
                _isSignUpPopupOpen = value;
                OnPropertyChanged(nameof(IsSignUpPopupOpen));
            }
        }
        // -> Command
        public ICommand LoginCommand { get; }
        public ICommand OpenSignUpCommand { get; }

        public LoginViewModel()
        {

            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            OpenSignUpCommand = new ViewModelCommand(ExecuteOpenSignUpCommand);
        }

        private void ExecuteOpenSignUpCommand(object obj)
        {
            IsSignUpPopupOpen = true;
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(Email)
                || Email.Length < 3
                || Password == null
                || Password.Length < 5)
                validData = false;
            else
                validData = true;

            return validData;
        }

        private async void ExecuteLoginCommand(object obj)
        {
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "email", Email },
                    { "password", SecureStringExtensions.ToUnsecuredString(Password) }
                };

                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync("http://localhost:8000/api/user/token/", content);

                if (response.IsSuccessStatusCode)
                {
                    // Login was successful, read the token
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                    string token = tokenResponse["token"];

                    // Update LoginModel with the token and raise event
                    LoginModel.GetInstance().Token = token;
                    LoginModel.GetInstance().LoggedIn = true;
                    LoginModel.GetInstance().RaiseUserLoggedIn();
                    IsViewVisible = false;
                }
                else
                {
                    ErrorMessage = " * Invalid email or password";
                }
            }
        }

    }
}
