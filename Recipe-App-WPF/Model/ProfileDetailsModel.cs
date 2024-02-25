using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_App_WPF.Model
{
    public class ProfileDetailsModel
    {
        public UserAccountModel UserAccountPofileDetails { get; set; }

        public string ProfileDetailsMessage { get; set; }

        public ProfileDetailsModel()
        {
            UserAccountPofileDetails = new UserAccountModel();
        }
    }
}
