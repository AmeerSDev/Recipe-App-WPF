using Recipe_App_WPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Recipe_App_WPF.ViewModel
{
    public class EditRecipeDetailsViewModel : ViewModelBase
    {
        private LoginModel _loginModel;
        private RecipeModel recipeModel;
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
            IsViewVisible = true;
            _loginModel = LoginModel.GetInstance();
            EditRecipeDetailsCommand = new ViewModelCommand(ExecuteEditRecipeDetailsCommand);
        }

        private void ExecuteEditRecipeDetailsCommand(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
