using Recipe_App_WPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
                OnPropertyChanged(nameof(_tags));
            }
        }
        public bool IsDeleteTagPopUpOpen
        {
            get { return _IsDeleteTagPopUpOpen; }
            set
            {
                if (_IsDeleteTagPopUpOpen == value) return;
                _IsDeleteTagPopUpOpen = value;
                OnPropertyChanged(nameof(_IsDeleteTagPopUpOpen));
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
        }

        private void ExecuteOpenTagEditViewCommand(object obj)
        {
            throw new NotImplementedException();
        }

        private void ExecuteOpenTagDeleteViewCommand(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
