using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_App_WPF.Helpers
{
    public class RecipesEventAggregator
    {
        private static RecipesEventAggregator _instance;
        public static RecipesEventAggregator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RecipesEventAggregator();
                }
                return _instance;
            }
        }

        public event EventHandler RecipeDeleted;
        public event EventHandler RecipeEdited;
        public event EventHandler RecipeCreated;

        public void PublishRecipeCreated()
        {
            RecipeCreated?.Invoke(this, EventArgs.Empty);
        }

        public void PublishRecipeDeleted()
        {
            RecipeDeleted?.Invoke(this, EventArgs.Empty);
        }

        // Add other recipe-related events and methods as needed
    }
}
