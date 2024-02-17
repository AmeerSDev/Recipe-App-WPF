using Recipe_App_WPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_App_WPF.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private LoginModel _loginModel;

        public MainViewModel()
        {
            _loginModel = LoginModel.GetInstance();
            _loginModel.UserLoggedIn += LoginModel_UserLoggedIn;
        }

        private void LoginModel_UserLoggedIn(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
