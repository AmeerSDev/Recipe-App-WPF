using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_App_WPF.Model
{
    public class LoginModel
    {
        private static LoginModel _instance;
        private static UserAccountModel _currentLoggedInAccount;
        public static LoginModel GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LoginModel();
                _currentLoggedInAccount = new UserAccountModel();
            }
            return _instance;
        }

        public bool LoggedIn { get; set; }

        public SecureString Token { get; set; }

        public UserAccountModel CurrentLoggedInAccount
        {
            get
            {
                return _currentLoggedInAccount;
            }
            set
            {
                _currentLoggedInAccount = value;
            }
        }

    }

}
