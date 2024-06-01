using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_App_WPF.Helpers
{
    public class TagsEventAggregator
    {
        private static TagsEventAggregator _instance;
        public static TagsEventAggregator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TagsEventAggregator();
                }
                return _instance;
            }
        }

        public event EventHandler TagDeleted;
        public event EventHandler TagEdited;

        public void PublishTagDeleted()
        {
            TagDeleted?.Invoke(this, EventArgs.Empty);
        }

        public void PublishTagEdited()
        {
            TagEdited?.Invoke(this, EventArgs.Empty);
        }
    }
}
