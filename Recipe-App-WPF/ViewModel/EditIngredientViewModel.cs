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
    public class EditIngredientViewModel : ViewModelBase
    {
        private LoginModel _loginModel;
        private bool _isViewVisible;
        private IngredientModel currentIngredientModel;

        public IngredientModel CurrentIngredientModel
        {
            get
            {
                return currentIngredientModel;
            }

            set
            {
                currentIngredientModel = value;
                OnPropertyChanged(nameof(CurrentIngredientModel));
            }
        }
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


        public ICommand EditIngredientCommand { get; }
        public EditIngredientViewModel()
        {
            _loginModel = LoginModel.GetInstance();
            currentIngredientModel = new IngredientModel();
            IsViewVisible = true;
            EditIngredientCommand = new ViewModelCommand(ExecuteEditIngredientCommand, CanExecuteDeleteIngredientCommand);
        }
        private bool CanExecuteDeleteIngredientCommand(object obj)
        {
            bool validData;
            if (obj == null)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(CurrentIngredientModel.Name)
                || CurrentIngredientModel.Id <= 0)
                validData = false;
            else
                validData = true;

            return validData;
        }
        private async void ExecuteEditIngredientCommand(object obj)
        {
            // Create a dictionary with non-null values
            var values = new Dictionary<string, string>
            {
                { "name", CurrentIngredientModel.Name}
            };

            // Remove entries with null values
            values = values
                .Where(pair => pair.Value != null && pair.Value != string.Empty)
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
                var response = await client.PatchAsync($"http://localhost:8000/api/recipe/ingredients/{CurrentIngredientModel.Id}/", content);

                if (response.IsSuccessStatusCode)
                {
                    IngredientsEventAggregator.Instance.PublishIngredientEdited();
                    Debug.WriteLine("Ingredient has been patched successfully");
                }
                else
                {
                    // Optionally, log the response content for more details
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Response Content: {responseContent}");
                }
            }
        }
    }
}
