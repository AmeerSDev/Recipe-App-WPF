using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_App_WPF.Helpers
{
    public class IngredientsEventAggregator
    {
        private static IngredientsEventAggregator _instance;
        public static IngredientsEventAggregator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new IngredientsEventAggregator();
                }
                return _instance;
            }
        }

        public event EventHandler IngredientDeleted;
        public event EventHandler IngredientEdited;

        public void PublishIngredientDeleted()
        {
            IngredientDeleted?.Invoke(this, EventArgs.Empty);
        }

        public void PublishIngredientEdited()
        {
            IngredientEdited?.Invoke(this, EventArgs.Empty);
        }
    }
}
