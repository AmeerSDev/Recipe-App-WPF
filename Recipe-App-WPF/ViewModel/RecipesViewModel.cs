using Newtonsoft.Json;
using Recipe_App_WPF.Extensions;
using Recipe_App_WPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Recipe_App_WPF.ViewModel
{
    public class RecipesViewModel : ViewModelBase
    {
        private LoginModel _loginModel;
        private ObservableCollection<RecipeModel> _recipes;

        public ObservableCollection<RecipeModel> Recipes
        {
            get { return _recipes; }
            set
            {
                _recipes = value;
                OnPropertyChanged(nameof(Recipes));
            }
        }

        public ICommand RecipeSelectedCommand { get; }
        public RecipesViewModel()
        {
            _loginModel = LoginModel.GetInstance();
            Recipes = new ObservableCollection<RecipeModel>();
            _ = LoadRecipesUserData();
            //RecipeSelectedCommand = new ViewModelCommand(ExecuteRecipeSelectedCommand);
        }

        private async void ExecuteRecipeSelectedCommand(object obj)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", SecureStringExtensions.ToUnsecuredString(_loginModel.Token));
                var response = await client.GetAsync($"http://localhost:8000/api/recipe/recipes/{obj}/");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var recipesItems = JsonConvert.DeserializeObject<RecipeModel>(responseContent);
                    //InitializeData(recipesItems);
                }
                else
                {
                    //MessageBox.Show("Couldn't Retrieve User Recipes!");
                }
            }
        }

        private async Task LoadRecipesUserData()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", SecureStringExtensions.ToUnsecuredString(_loginModel.Token));
                var response = await client.GetAsync("http://localhost:8000/api/recipe/recipes/");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var recipesItems = JsonConvert.DeserializeObject<List<RecipeModel>>(responseContent);
                    InitializeData(recipesItems);
                }
                else
                {
                    //MessageBox.Show("Couldn't Retrieve User Recipes!");
                }
            }
        }

        private void InitializeData(List<RecipeModel> responseData)
        {
            foreach (var dataEntry in responseData)
            {
              
                Recipes.Add(dataEntry);
            }
        }
    }
}

