using Newtonsoft.Json;
using Recipe_App_WPF.Extensions;
using Recipe_App_WPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;

namespace Recipe_App_WPF.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private LoginModel _loginModel;
        private UserAccountModel _currentUserAccount;

        public UserAccountModel CurrentUserAccount
        {
            get
            {
                return _currentUserAccount;
            }

            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));
            }
        }

        public MainViewModel()
        {
            _currentUserAccount = new UserAccountModel();
            _loginModel = LoginModel.GetInstance();
            _loginModel.UserLoggedIn += LoginModel_UserLoggedIn;
            LoadCurrentUserData();
        }

        private async void LoadCurrentUserData()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _loginModel.Token);
                var response = await client.GetAsync("http://localhost:8000/api/user/me/");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                    CurrentUserAccount = new UserAccountModel()
                    {
                        Username = tokenResponse["email"],
                        DisplayName = $"Welcome {tokenResponse["name"]} ",
                        ProfilePictures = null

                    };
                }
                else
                {
                    MessageBox.Show("Invalid user, not logged in");
                    Application.Current.Shutdown();
                }
            }
        }

        private void LoginModel_UserLoggedIn(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
