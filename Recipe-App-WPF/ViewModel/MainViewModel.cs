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
            //Initialze commands
            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
            ShowRecipesViewCommand = new ViewModelCommand(ExecuteRecipesViewCommand);
            ShowTagsViewCommand = new ViewModelCommand(ExecuteTagsViewCommand);
            ShowIngredientsViewCommand = new ViewModelCommand(ExecuteIngredientsViewCommand);

            //Default view
            ExecuteShowHomeViewCommand(null);

            if(_loginModel.LoggedIn)
                LoadCurrentUserData();
        }


        private void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentChildView = new HomeViewModel();
            Caption = "Dashboard";
            Icon = IconChar.Home;
        }

        private void ExecuteRecipesViewCommand(object obj)
        {
            CurrentChildView = new RecipesViewModel();
            Caption = "Recipes";
            Icon = IconChar.BowlFood;
        }
        private void ExecuteTagsViewCommand(object obj)
        {
            CurrentChildView = new TagsViewModel();
            Caption = "Tags";
            Icon = IconChar.Tags;
        }
        private void ExecuteIngredientsViewCommand(object obj)
        {
            CurrentChildView = new IngredientsViewModel();
            Caption = "Ingredients";
            Icon = IconChar.Carrot;
        }

        //--> Commands
        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowRecipesViewCommand { get; }
        public ICommand ShowTagsViewCommand { get; }
        public ICommand ShowIngredientsViewCommand { get; }
        private async void LoadCurrentUserData()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", SecureStringExtensions.ToUnsecuredString(_loginModel.Token));
                var response = await client.GetAsync("http://localhost:8000/api/user/me/");

                 if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                    CurrentUserAccount = new UserAccountModel()
                    {
                        Email = tokenResponse["email"],
                        Name = $"Welcome {tokenResponse["name"]} "

                    };
                }
                else
                {
                    //MessageBox.Show("Invalid user, not logged in");
                    //Application.Current.Shutdown();
                }
            }
        }
    }
}
