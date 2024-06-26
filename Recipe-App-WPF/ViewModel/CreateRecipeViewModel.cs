﻿using Newtonsoft.Json;
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
    public class CreateRecipeViewModel : ViewModelBase
    {
        private LoginModel _loginModel;
        private RecipeModel recipeModel;
        private bool _isViewVisible;

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

        public RecipeModel RecipeModel
        {
            get
            {
                return recipeModel;
            }
            set
            {
                recipeModel = value;
                OnPropertyChanged(nameof(RecipeModel));
            }
        }

        public ICommand CreateRecipeCommand { get; }
        public CreateRecipeViewModel()
        {
            _loginModel = LoginModel.GetInstance();
            recipeModel = new RecipeModel();
            IsViewVisible = true;
            CreateRecipeCommand = new ViewModelCommand(ExecuteCreateRecipeCommand, CanExecuteCreateRecipeCommand);

        }

        private bool CanExecuteCreateRecipeCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(recipeModel.Title)
                || recipeModel.Time_Minutes <= 0
                || recipeModel.Price <= 0)
                validData = false;
            else
                validData = true;

            return validData;
        }

        private async void ExecuteCreateRecipeCommand(object obj)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var values = new Dictionary<string, object>
                    {
                        { "title", recipeModel.Title },
                        { "time_minutes", recipeModel.Time_Minutes},
                        { "link", recipeModel.Link},
                        { "price", recipeModel.Price.ToString()},
                        { "tags", _NestedRecipeObjectToJsonObject(recipeModel.TagsNames) },
                        { "ingredients", _NestedRecipeObjectToJsonObject(recipeModel.IngredientsNames)},
                        { "description", recipeModel.Description},
                        {"image", null }
                    };

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token",
                                                                                               SecureStringExtensions.ToUnsecuredString(_loginModel.Token));

                    string jsonPayload = JsonConvert.SerializeObject(values);
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://localhost:8000/api/recipe/recipes/", content);

                    if (response.IsSuccessStatusCode)
                    {
                        RecipesEventAggregator.Instance.PublishRecipeCreated();
                        Debug.WriteLine("Recipe has been created successfully");
                    }
                    else
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine($"Failed to create recipe. Status code: {response.StatusCode}, Response Content: {responseContent}");
                    }
                }
            }
            catch (HttpRequestException httpRequestException)
            {
                // Handle specific HTTP request exceptions
                Debug.WriteLine($"Request error: {httpRequestException.Message}");
            }
            catch (TaskCanceledException taskCanceledException)
            {
                // Handle timeout or cancellation exceptions
                Debug.WriteLine($"Request timed out: {taskCanceledException.Message}");
            }
            catch (InvalidOperationException invalidOperationException)
            {
                // Handle invalid operations, such as invalid URL format
                Debug.WriteLine($"Invalid operation: {invalidOperationException.Message}");
            }
            catch (JsonSerializationException jsonSerializationException)
            {
                // Handle JSON serialization errors
                Debug.WriteLine($"Serialization error: {jsonSerializationException.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                Debug.WriteLine($"An unexpected error occurred: {ex.Message}");
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
