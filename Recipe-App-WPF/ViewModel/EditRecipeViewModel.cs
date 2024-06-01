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
        private int recipeUniqueID;
        private bool _isViewVisible;
        private bool _IsEditRecipeDetailsPopUpOpen;


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
        public ICommand OpenRecipeEditDetailsViewCommand { get; }

        public EditRecipeViewModel()
        {
            IsViewVisible = true;
            OpenRecipeEditDetailsViewCommand = new ViewModelCommand(ExecuteOpenRecipeEditDetailsViewCommand);
        }

        private void ExecuteOpenRecipeEditDetailsViewCommand(object obj)
        {
            IsEditRecipeDetailsPopUpOpen = true;
            var editRecipeDetailsViewModel = new EditRecipeDetailsViewModel();
            editRecipeDetailsViewModel.IsViewVisible = true;
            editRecipeDetailsViewModel.SetUpCurrentRecipeToEdit(obj);
        }
    }
}
