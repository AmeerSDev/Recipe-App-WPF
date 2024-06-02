using Newtonsoft.Json;
using Recipe_App_WPF.Extensions;
using Recipe_App_WPF.Helpers;
using Recipe_App_WPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Recipe_App_WPF.ViewModel
{
    public class IngredientsViewModel : ViewModelBase
    {
        private LoginModel _loginModel;
        private ObservableCollection<IngredientModel> _ingredients;
        private bool _IsDeleteIngredientPopUpOpen;
        private bool _IsEditIngredientPopUpOpen;

        public ObservableCollection<IngredientModel> Ingredients
        {
            get { return _ingredients; }
            set
            {
                _ingredients = value;
                OnPropertyChanged(nameof(Ingredients));
            }
        }
        public bool IsDeleteIngredientPopUpOpen
        {
            get { return _IsDeleteIngredientPopUpOpen; }
            set
            {
                if (_IsDeleteIngredientPopUpOpen == value) return;
                _IsDeleteIngredientPopUpOpen = value;
                OnPropertyChanged(nameof(IsDeleteIngredientPopUpOpen));
            }
        }

        public bool IsEditIngredientPopUpOpen
        {
            get { return _IsEditIngredientPopUpOpen; }
            set
            {
                if (_IsEditIngredientPopUpOpen == value) return;
                _IsEditIngredientPopUpOpen = value;
                OnPropertyChanged(nameof(IsEditIngredientPopUpOpen));
            }
        }

        public ICommand OpenIngredientDeleteViewCommand { get; }
        public ICommand OpenIngredientEditViewCommand { get; }

        public IngredientsViewModel()
        {
            _loginModel = LoginModel.GetInstance();
            Ingredients = new ObservableCollection<IngredientModel>();
            OpenIngredientDeleteViewCommand = new ViewModelCommand(ExecuteOpenIngredientDeleteViewCommand);
            OpenIngredientEditViewCommand = new ViewModelCommand(ExecuteOpenIngredientEditViewCommand);
            _ = LoadIngredientsUserData();
            IngredientsEventAggregator.Instance.IngredientDeleted += OnIngredientDeleted;
            IngredientsEventAggregator.Instance.IngredientEdited += OnIngredientEdited;
        }

        private async void OnIngredientEdited(object sender, EventArgs e)
        {
            await RefreshIngredientsList();
        }

        private async void OnIngredientDeleted(object sender, EventArgs e)
        {
            await RefreshIngredientsList();
        }

        private async Task LoadIngredientsUserData()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", SecureStringExtensions.ToUnsecuredString(_loginModel.Token));
                var response = await client.GetAsync("http://localhost:8000/api/recipe/ingredients/");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var recipesItems = JsonConvert.DeserializeObject<List<IngredientModel>>(responseContent);
                    InitializeIngredientsData(recipesItems);
                }
                else
                {
                    Debug.WriteLine("Couldn't Retrieve User Ingredients!");
                }
            }
        }
        private void ExecuteOpenIngredientEditViewCommand(object obj)
        {
            throw new NotImplementedException();
        }

        private void ExecuteOpenIngredientDeleteViewCommand(object obj)
        {
            throw new NotImplementedException();
        }

        private void InitializeIngredientsData(List<IngredientModel> responseData)
        {
            foreach (var dataEntry in responseData)
            {
                Ingredients.Add(dataEntry);
            }
        }
        private async Task RefreshIngredientsList()
        {
            DeinitializeAllIngredientsData();
            await LoadIngredientsUserData();

        }
        private void DeinitializeAllIngredientsData()
        {
            for (int i = Ingredients.Count - 1; i >= 0; i--)
            {
                Ingredients.RemoveAt(i);
            }
        }
    }
}
