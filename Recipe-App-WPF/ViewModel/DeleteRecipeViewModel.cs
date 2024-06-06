using Recipe_App_WPF.Extensions;
using Recipe_App_WPF.Helpers;
using Recipe_App_WPF.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Recipe_App_WPF.ViewModel
{
    public class DeleteRecipeViewModel : ViewModelBase
    {
        private LoginModel _loginModel;
        private bool _isViewVisible;
        private int recipeUniqueID;


        public int RecipeUniqueID
        {
            get
            {
                return recipeUniqueID;
            }

            set
            {
                recipeUniqueID = value;
                OnPropertyChanged(nameof(RecipeUniqueID));
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

        public ICommand DeleteRecipeCommand { get; }
        public DeleteRecipeViewModel()
        {
            _loginModel = LoginModel.GetInstance();
            IsViewVisible = true;
            DeleteRecipeCommand = new ViewModelCommand(ExecuteDeleteRecipeCommand);
        }
        private async void ExecuteDeleteRecipeCommand(object obj)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", SecureStringExtensions.ToUnsecuredString(_loginModel.Token));

                    // Construct the URL with the RecipeUniqueID
                    string url = $"http://localhost:8000/api/recipe/recipes/{RecipeUniqueID}/";

                    // Use DELETE method
                    var response = await client.DeleteAsync(url);

                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        // Recipe deletion was successful
                        RecipesEventAggregator.Instance.PublishRecipeDeleted();
                        Debug.WriteLine("Recipe has been deleted successfully");
                    }
                    else
                    {
                        // Handle unsuccessful response
                        Debug.WriteLine($"Failed to delete recipe. Status code: {response.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException httpRequestException)
            {
                // Handle specific HTTP request exceptions
                Debug.WriteLine($"Request error: {httpRequestException.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                Debug.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
