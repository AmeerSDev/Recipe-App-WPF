using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Recipe_App_WPF.Extensions;
using Recipe_App_WPF.Model;

namespace Recipe_App_WPF.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private LoginModel _loginModel;
        private ProfileDetailsModel _profileDetailsModel;

        public string Name
        {
            get
            {
                return _profileDetailsModel.Name;
            }

            set
            {
                _profileDetailsModel.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Email
        {
            get
            {
                return _profileDetailsModel.Email;
            }
            set
            {
                _profileDetailsModel.Email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public SecureString Password
        {
            get
            {
                return _profileDetailsModel.Password;
            }
            set
            {
                _profileDetailsModel.Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ProfileDetailsMessage
        {
            get
            {
                return _profileDetailsModel.ProfileDetailsMessage;
            }
            set
            {
                _profileDetailsModel.ProfileDetailsMessage = value;
                OnPropertyChanged(nameof(ProfileDetailsMessage));
            }
        }

        public ICommand SaveProfileDetailsCommand { get; }

        public HomeViewModel()
        {
            _loginModel = LoginModel.GetInstance();
            _profileDetailsModel = new ProfileDetailsModel();
            SaveProfileDetailsCommand = new ViewModelCommand(ExecuteSaveProfileDetailsCommand);
        }

        private bool CanExecuteSaveProfileDetailsCommand(object obj)
        {
            bool validData;
            if (Password.Length < 5 ||
                !string.IsNullOrWhiteSpace(Email) && !Email.Contains("@"))
            {
                validData = false;
            }
            else
                validData = true;

            return validData;
        }

        private async void ExecuteSaveProfileDetailsCommand(object obj)
        {
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "email", Email ?? _loginModel.CurrentLoggedInAccount.Email},
                    { "password", SecureStringExtensions.ToUnsecuredString(Password) },
                    { "name", Name ?? _loginModel.CurrentLoggedInAccount.Name }
                };
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loginModel.Token);
                var content = new FormUrlEncodedContent(values);
                var response = await client.PutAsync("http://localhost:8000/api/user/me/", content);

                if (response.IsSuccessStatusCode)
                {
                    _profileDetailsModel.ProfileDetailsMessage = "You're new profile details has been saved successfully!";
                }
                else
                {
                    _profileDetailsModel.ProfileDetailsMessage = " * Something went wrong while saving you're new profile details";
                }
            }
        }
    }
}
