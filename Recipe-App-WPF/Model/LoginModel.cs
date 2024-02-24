﻿using System;
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
        private static ProfileDetailsModel _currentLoggedInAccount;
        public static LoginModel GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LoginModel();
                _currentLoggedInAccount = new ProfileDetailsModel();
            }
            return _instance;
        }

        public bool LoggedIn { get; set; }

        public string Token { get; set; }

        public ProfileDetailsModel CurrentLoggedInAccount
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


        public event EventHandler UserLoggedIn;

        public void RaiseUserLoggedIn()
        {
            UserLoggedIn?.Invoke(this, EventArgs.Empty);
        }
    }

}
