using FontAwesome.Sharp;
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
using System.Windows.Input;

namespace Recipe_App_WPF.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private LoginModel _loginModel;
        private UserAccountModel _currentUserAccount;
        private ViewModelBase _currentChildView;
        private string _caption;
        private IconChar _icon;

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

        public ViewModelBase CurrentChildView {
            get
            {
                return _currentChildView;
            }
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }
        public string Caption {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }
        public IconChar Icon {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }

        public MainViewModel()
        {
            _loginModel = LoginModel.GetInstance();
            _loginModel.UserLoggedIn += LoginModel_UserLoggedIn;
            //Initialze commands
            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
            ShowCustomerViewCommand = new ViewModelCommand(ExecuteShowCustomerViewCommand);

            //Default view
            ExecuteShowHomeViewCommand(null);

            LoadCurrentUserData();
        }

        private void ExecuteShowCustomerViewCommand(object obj)
        {
            CurrentChildView = new CustomersViewModel();
            Caption = "Customers";
            Icon = IconChar.UserGroup;
        }

        private void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentChildView = new HomeViewModel();
            Caption = "Dashboard";
            Icon = IconChar.Home;
        }

        //--> Commands
        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowCustomerViewCommand { get; }
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
                    //MessageBox.Show("Invalid user, not logged in");
                    //Application.Current.Shutdown();
                }
            }
        }

        private void LoginModel_UserLoggedIn(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
