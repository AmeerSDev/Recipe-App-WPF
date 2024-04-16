using Recipe_App_WPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Recipe_App_WPF.ViewModel
{
    public class EditRecipeViewModel : ViewModelBase
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

        public ICommand EditRecipeCommand { get; }

        public EditRecipeViewModel()
        {
            _loginModel = LoginModel.GetInstance();
            IsViewVisible = true;
            EditRecipeCommand = new ViewModelCommand(ExecuteEditRecipeCommand);
        }

        private void ExecuteEditRecipeCommand(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
