using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            // Create a dictionary with non-null values
            var values = new Dictionary<string, string>
            {
                { "email", Email ?? _loginModel.CurrentLoggedInAccount.Email },
                { "password", SecureStringExtensions.ToUnsecuredString(Password) },
                { "name", Name ?? _loginModel.CurrentLoggedInAccount.Name }
            };

            // Remove entries with null values
            values = values
                .Where(pair => pair.Value != null && pair.Value != string.Empty)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            // Convert the dictionary to a JSON string
            string jsonPayload = JsonConvert.SerializeObject(values);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loginModel.Token);

                // Create StringContent with JSON payload
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // Send PATCH request
                var response = await client.PatchAsync("http://localhost:8000/api/user/me/", content);

                if (response.IsSuccessStatusCode)
                {
                    _profileDetailsModel.ProfileDetailsMessage = "Your new profile details have been saved successfully!";
                }
                else
                {
                    _profileDetailsModel.ProfileDetailsMessage = $" * Something went wrong. Error: {response.StatusCode} - {response.ReasonPhrase}";
                    // Optionally, log the response content for more details
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Response Content: {responseContent}");
                }
            }
        }
    }
}
