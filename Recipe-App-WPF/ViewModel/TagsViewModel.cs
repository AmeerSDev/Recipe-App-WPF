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
    public class TagsViewModel : ViewModelBase
    {
        private LoginModel _loginModel;
        private ObservableCollection<TagModel> _tags;
        private bool _IsDeleteTagPopUpOpen;
        private bool _IsEditTagPopUpOpen;

        public ObservableCollection<TagModel> Tags
        {
            get { return _tags; }
            set
            {
                _tags = value;
                OnPropertyChanged(nameof(Tags));
            }
        }
        public bool IsDeleteTagPopUpOpen
        {
            get { return _IsDeleteTagPopUpOpen; }
            set
            {
                if (_IsDeleteTagPopUpOpen == value) return;
                _IsDeleteTagPopUpOpen = value;
                OnPropertyChanged(nameof(IsDeleteTagPopUpOpen));
            }
        }

        public bool IsEditTagPopUpOpen
        {
            get { return _IsEditTagPopUpOpen; }
            set
            {
                if (_IsEditTagPopUpOpen == value) return;
                _IsEditTagPopUpOpen = value;
                OnPropertyChanged(nameof(IsEditTagPopUpOpen));
            }
        }

        public ICommand OpenTagDeleteViewCommand { get; }
        public ICommand OpenTagEditViewCommand { get; }

        public TagsViewModel()
        {
            _loginModel = LoginModel.GetInstance();
            Tags = new ObservableCollection<TagModel>();
            OpenTagDeleteViewCommand = new ViewModelCommand(ExecuteOpenTagDeleteViewCommand);
            OpenTagEditViewCommand = new ViewModelCommand(ExecuteOpenTagEditViewCommand);
            _ = LoadTagsUserData();
            TagsEventAggregator.Instance.TagDeleted += OnTagDeleted;
            TagsEventAggregator.Instance.TagEdited += OnTagEdited;
        }

        private void ExecuteOpenTagEditViewCommand(object obj)
        {
            throw new NotImplementedException();
        }

        private void ExecuteOpenTagDeleteViewCommand(object obj)
        {
            IsDeleteTagPopUpOpen = true;
            var deleteRecipeViewModel = new DeleteTagViewModel();
            deleteRecipeViewModel.IsViewVisible = true;
        }

        private async void OnTagEdited(object sender, EventArgs e)
        {
            await RefreshTagsList();
        }

        private async void OnTagDeleted(object sender, EventArgs e)
        {
            await RefreshTagsList();
        }

        private async Task LoadTagsUserData()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", SecureStringExtensions.ToUnsecuredString(_loginModel.Token));
                var response = await client.GetAsync("http://localhost:8000/api/recipe/tags/");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var recipesItems = JsonConvert.DeserializeObject<List<TagModel>>(responseContent);
                    InitializeTagsData(recipesItems);
                }
                else
                {
                    Debug.WriteLine("Couldn't Retrieve User Tags!");
                }
            }
        }

        private void InitializeTagsData(List<TagModel> responseData)
        {
            foreach (var dataEntry in responseData)
            {
                Tags.Add(dataEntry);
            }
        }
        private async Task RefreshTagsList()
        {
            DeinitializeAllTagsData();
            await LoadTagsUserData();

        }
        private void DeinitializeAllTagsData()
        {
            for (int i = Tags.Count - 1; i >= 0; i--)
            {
                Tags.RemoveAt(i);
            }
        }
    }
}
