using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_App_WPF.Model
{
    public class RecipeModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int Time_Minutes { get; set; }
        public float Price { get; set; }
        public string Link { get; set; }
        public List<TagModel> Tags { get; set; }
        public List<IngredientModel> Ingredients { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        //public RecipeModel()
        //{
        //    Tags = new List<TagModel>();
        //    Ingredients = new List<IngredientModel>();
        //}
    }
}
