using Newtonsoft.Json;
using Recipe_App_WPF.Extensions;
using Recipe_App_WPF.Helpers;
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
            // Create a dictionary with non-null values
            var values = new Dictionary<string, object>
                {
                    { "id", CurrentRecipeDetails.ID },
                    { "title", CurrentRecipeDetails.Title },
                    { "time_minutes", CurrentRecipeDetails.Time_Minutes},
                    { "link", CurrentRecipeDetails.Link},
                    { "price", CurrentRecipeDetails.Price.ToString()},
                    { "tags", _NestedRecipeObjectToJsonObject(CurrentRecipeDetails.TagsNames) },
                    { "ingredients", _NestedRecipeObjectToJsonObject(CurrentRecipeDetails.IngredientsNames)},
                    { "description", CurrentRecipeDetails.Description},
                    {"image", null }
                };

            // Remove entries with null, empty, whitespace-only values, values less than or equal to 0, or empty lists
            values = values
                .Where(pair => pair.Value != null &&
                               (!(pair.Value is string) || !string.IsNullOrWhiteSpace((string)pair.Value)) &&
                               (!(pair.Value is int) || (int)pair.Value > 0) &&
                               (!(pair.Value is List<Dictionary<string, object>>) || ((List<Dictionary<string, object>>)pair.Value).Count > 0) &&
                               !(pair.Key == "price" && int.TryParse((string)pair.Value, out int price) && price <= 0))
                .ToDictionary(pair => pair.Key, pair => pair.Value);


            // Convert the dictionary to a JSON string
            string jsonPayload = JsonConvert.SerializeObject(values);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token",
                                                                                            SecureStringExtensions.ToUnsecuredString(_loginModel.Token));

                // Create StringContent with JSON payload
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // Send PATCH request
                var response = await client.PatchAsync($"http://localhost:8000/api/recipe/recipes/{CurrentRecipeDetails.ID}/", content);

                if (response.IsSuccessStatusCode)
                {
                    RecipesEventAggregator.Instance.PublishRecipeEdited();
                    Debug.WriteLine("Recipe has been patched successfully");
                }
                else
                {
                    // Optionally, log the response content for more details
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Response Content: {responseContent}");
                }
            }
        }

        private List<Dictionary<string, object>> _NestedRecipeObjectToJsonObject(string concatedObjectsNames)
        {
            if (string.IsNullOrEmpty(concatedObjectsNames))
            {
                return new List<Dictionary<string, object>>(); // Return an empty list
            }
            else
            {
                var nestedRecipeObjects = new List<Dictionary<string, object>>();

                // Split the comma-separated string into individual tags
                string[] objectsNamesArray = concatedObjectsNames.Split(',');

                // Create a dictionary for each tag and add it to the list
                foreach (var objectName in objectsNamesArray)
                {
                    var recipeObjectsDict = new Dictionary<string, object>
                {
                    { "name", objectName.Trim() }    // Trim any extra spaces around the tag name
                };
                    nestedRecipeObjects.Add(recipeObjectsDict);
                }


                return nestedRecipeObjects;
            }

        }
    }
}
