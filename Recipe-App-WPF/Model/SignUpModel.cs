using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_App_WPF.Model
{
    public class SignUpModel
    {
        public UserAccountModel SignUpUserAccount { get; set; }

        public string RegisterMessage { get; set; }

        public SignUpModel()
        {
            SignUpUserAccount = new UserAccountModel();
        }
    }
}
