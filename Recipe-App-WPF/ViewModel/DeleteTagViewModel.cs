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
    public class DeleteTagViewModel : ViewModelBase
    {
        private LoginModel _loginModel;
        private bool _isViewVisible;
        private int tagUniqueID;

        public int TagUniqueID
        {
            get
            {
                return tagUniqueID;
            }

            set
            {
                tagUniqueID = value;
                OnPropertyChanged(nameof(TagUniqueID));
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

        public ICommand DeleteTagCommand { get; }

        public DeleteTagViewModel()
        {
            _loginModel = LoginModel.GetInstance();
            IsViewVisible = true;
            DeleteTagCommand = new ViewModelCommand(ExecuteDeleteTagCommand);
        }

        private async void ExecuteDeleteTagCommand(object obj)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", SecureStringExtensions.ToUnsecuredString(_loginModel.Token));

                    // Construct the URL with the TagUniqueID
                    string url = $"http://localhost:8000/api/recipe/tags/{TagUniqueID}/";

                    // Use DELETE method
                    var response = await client.DeleteAsync(url);

                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        // Tag deletion was successful
                        TagsEventAggregator.Instance.PublishTagDeleted();
                        Debug.WriteLine("Tag has been deleted successfully");
                    }
                    else
                    {
                        // Handle unsuccessful response
                        Debug.WriteLine($"Failed to delete tag. Status code: {response.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException httpRequestException)
            {
                Debug.WriteLine($"Request error: {httpRequestException.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

    }
}
