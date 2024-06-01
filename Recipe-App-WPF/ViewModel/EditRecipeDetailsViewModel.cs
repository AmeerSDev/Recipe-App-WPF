using Newtonsoft.Json;
using Recipe_App_WPF.Extensions;
using Recipe_App_WPF.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Recipe_App_WPF.ViewModel
{
    public class EditRecipeDetailsViewModel : ViewModelBase
    {
        private LoginModel _loginModel;
        private RecipeModel _currentRecipeDetails;
        private int _currentRecipeID;
        private bool _isViewVisible;
        private bool _IsEditRecipeDetailsPopUpOpen;

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
        public int CurrentRecipeID
        {
            get
            {
                return _currentRecipeID;
            }
            set
            {
                _currentRecipeID = value;
                OnPropertyChanged(nameof(CurrentRecipeID));
            }
        }
        public RecipeModel CurrentRecipeDetails
        {
            get
            {
                return _currentRecipeDetails;
            }
            set
            {
                _currentRecipeDetails = value;
                OnPropertyChanged(nameof(CurrentRecipeDetails));
            }
        }

        public bool IsEditRecipeDetailsPopUpOpen
        {
            get { return _IsEditRecipeDetailsPopUpOpen; }
            set
            {
                if (_IsEditRecipeDetailsPopUpOpen == value) return;
                _IsEditRecipeDetailsPopUpOpen = value;
                OnPropertyChanged(nameof(IsEditRecipeDetailsPopUpOpen));
            }
        }

        public ICommand EditRecipeDetailsCommand { get; }

        public EditRecipeDetailsViewModel()
        {
            _loginModel = LoginModel.GetInstance();
            _currentRecipeDetails = new RecipeModel();
            IsViewVisible = true;
            EditRecipeDetailsCommand = new ViewModelCommand(ExecuteEditRecipeDetailsCommand);
        }

        private async void ExecuteEditRecipeDetailsCommand(object obj)
        {

        }

        public async void SetUpCurrentRecipeToEdit(object selectedRecipeIDToEdit)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", SecureStringExtensions.ToUnsecuredString(_loginModel.Token));
                var response = await client.GetAsync($"http://localhost:8000/api/recipe/recipes/{selectedRecipeIDToEdit}/");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var retrievedRecipetoEdit = JsonConvert.DeserializeObject<RecipeModel>(responseContent);
                    _currentRecipeDetails = retrievedRecipetoEdit;
                    //Debug.WriteLine(_currentRecipeDetails.Title);
                    //Debug.WriteLine(_currentRecipeDetails.Time_Minutes);
                    //Debug.WriteLine(_currentRecipeDetails.TagsNames);
                    //Debug.WriteLine(_currentRecipeDetails.Tags);
                    //Debug.WriteLine(_currentRecipeDetails.Price);
                    //Debug.WriteLine(_currentRecipeDetails.Link);
                    //Debug.WriteLine(_currentRecipeDetails.IngredientsNames);
                    //Debug.WriteLine(_currentRecipeDetails.Ingredients);
                    //Debug.WriteLine(_currentRecipeDetails.Image);
                    //Debug.WriteLine(_currentRecipeDetails.ID);
                }
                else
                {
                    //MessageBox.Show("Couldn't Retrieve User Recipes!");
                }
            }
        }
    }
}
