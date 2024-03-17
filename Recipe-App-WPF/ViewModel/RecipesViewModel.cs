using Newtonsoft.Json;
using Recipe_App_WPF.Extensions;
using Recipe_App_WPF.Helpers;
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
        private bool _IsDeleteRecipePopUpOpen;

        public ObservableCollection<RecipeModel> Recipes
        {
            get { return _recipes; }
            set
            {
                _recipes = value;
                OnPropertyChanged(nameof(Recipes));
            }
        }
        public bool IsDeleteRecipePopUpOpen
        {
            get { return _IsDeleteRecipePopUpOpen; }
            set
            {
                if (_IsDeleteRecipePopUpOpen == value) return;
                _IsDeleteRecipePopUpOpen = value;
                OnPropertyChanged(nameof(IsDeleteRecipePopUpOpen));
            }
        }

        public ICommand RecipeSelectedCommand { get; }
        public ICommand OpenRecipeDeleteViewCommand { get; }
        public RecipesViewModel()
        {
            _loginModel = LoginModel.GetInstance();
            Recipes = new ObservableCollection<RecipeModel>();
            _ = LoadRecipesUserData();
            OpenRecipeDeleteViewCommand = new ViewModelCommand(ExecuteOpenRecipeDeleteViewCommand);
            //RecipeSelectedCommand = new ViewModelCommand(ExecuteRecipeSelectedCommand);
            RecipesEventAggregator.Instance.RecipeDeleted += OnRecipeDeleted;
        }

        private async void OnRecipeDeleted(object sender, EventArgs e)
        {
            DeinitializeAllData();
            await LoadRecipesUserData();
        }

        private void ExecuteOpenRecipeDeleteViewCommand(object obj)
        {
            IsDeleteRecipePopUpOpen = true;
            var deleteRecipeViewModel = new DeleteRecipeViewModel();
            deleteRecipeViewModel.IsViewVisible = true;
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
        private void DeinitializeAllData()
        {
            for (int i = Recipes.Count - 1; i >= 0; i--)
            {
                Recipes.RemoveAt(i);
            }
        }
    }
}

