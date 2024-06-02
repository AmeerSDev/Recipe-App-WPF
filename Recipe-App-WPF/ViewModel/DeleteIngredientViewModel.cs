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
    public class DeleteIngredientViewModel : ViewModelBase
    {
        private LoginModel _loginModel;
        private bool _isViewVisible;
        private int ingredientUniqueID;

        public int IngredientUniqueID
        {
            get
            {
                return ingredientUniqueID;
            }

            set
            {
                ingredientUniqueID = value;
                OnPropertyChanged(nameof(IngredientUniqueID));
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

        public ICommand DeleteIngredientCommand { get; }

        public DeleteIngredientViewModel()
        {
            _loginModel = LoginModel.GetInstance();
            IsViewVisible = true;
            DeleteIngredientCommand = new ViewModelCommand(ExecuteDeleteIngredientCommand);
        }

        private async void ExecuteDeleteIngredientCommand(object obj)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", SecureStringExtensions.ToUnsecuredString(_loginModel.Token));

                // Construct the URL with the IngredientUniqueID
                string url = $"http://localhost:8000/api/recipe/ingredients/{IngredientUniqueID}/";

                // Use DELETE method
                var response = await client.DeleteAsync(url);

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // Ingredient deletion was successful
                    IngredientsEventAggregator.Instance.PublishIngredientDeleted();
                    Debug.WriteLine("Ingredient has been deleted successfully");
                }
                else
                {
                    // Handle unsuccessful response
                    Debug.WriteLine($"Failed to delete ingredient. Status code: {response.StatusCode}");
                }
            }
        }
    }
}
