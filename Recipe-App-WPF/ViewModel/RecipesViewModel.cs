using Recipe_App_WPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_App_WPF.ViewModel
{
    public class RecipesViewModel : ViewModelBase
    {
        private ObservableCollection<RecipeModel> _recipes;

        public ObservableCollection<RecipeModel> Recipes
        {
            get { return _recipes; }
            set
            {
                _recipes = value;
                OnPropertyChanged(nameof(Recipes));
            }
        }

        public RecipesViewModel()
        {
            InitializeData();
        }

        private void InitializeData()
        {
            // Initialize and populate the ObservableCollection with recipe data
            Recipes = new ObservableCollection<RecipeModel>
            {
                new RecipeModel { Title = "Pasta Carbonara",
                                  Description = "Loreipsum Loreipsum Loreipsum",
                                  Link="www.berlier.com",
                                  /* Add more details */ },
                new RecipeModel { Title = "Chicken Stir-Fry", Description = "Loreipsum Loreipsum Loreipsum", Link="www.berlier.com" /* Add more details */ },
                new RecipeModel { Title = "Chocolate Cake", Description = "Loreipsum Loreipsum Loreipsum", Link="www.berlier.com"/* Add more details */ },
                new RecipeModel { Title = "Chocolate Cake", Description = "Loreipsum Loreipsum Loreipsum", Link="www.berlier.com"/* Add more details */ },
                new RecipeModel { Title = "Chocolate Cake", Description = "Loreipsum Loreipsum Loreipsum", Link="www.berlier.com"/* Add more details */ },
                new RecipeModel { Title = "Chocolate Cake", Description = "Loreipsum Loreipsum Loreipsum", Link="www.berlier.com"/* Add more details */ },
                new RecipeModel { Title = "Chocolate Cake", /* Add more details */ },
                // Add more recipes as needed
            };
        }
    }
}
