﻿using Newtonsoft.Json;
using Recipe_App_WPF.Extensions;
using Recipe_App_WPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Recipe_App_WPF.ViewModel
{
    public class SignUpViewModel : ViewModelBase
    {
        private SignUpModel _signUpModel;

        public string Name
        {
            get
            {
                return _signUpModel.Name;
            }

            set
            {
                _signUpModel.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Email
        {
            get
            {
                return _signUpModel.Email;
            }
            set
            {
                _signUpModel.Email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public SecureString Password
        {
            get
            {
                return _signUpModel.Password;
            }
            set
            {
                _signUpModel.Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string RegisterMessage
        {
            get
            {
                return _signUpModel.RegisterMessage;
            }
            set
            {
                _signUpModel.RegisterMessage = value;
                OnPropertyChanged(nameof(RegisterMessage));
            }
        }

        public ICommand RegisterCommand { get; }
        public SignUpViewModel()
        {
            _signUpModel = new SignUpModel();
            RegisterCommand = new ViewModelCommand(ExecuteRegisterCommand, CanExecuteRegisterCommand);
        }

        private bool CanExecuteRegisterCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(Email)
                || Email.Length < 3
                || Password == null
                || Password.Length < 5
                || !Email.Contains("@"))
                validData = false;
            else
                validData = true;

            return validData;
        }

        private async void ExecuteRegisterCommand(object obj)
        {
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "email", Email },
                    { "password", SecureStringExtensions.ToUnsecuredString(Password) },
                    { "name", Name}
                };

                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync("http://localhost:8000/api/user/create/", content);

                if (response.IsSuccessStatusCode)
                {
                    // Login was successful, read the token
                    RegisterMessage = "You have successfully registered!";
                }
                else
                {
                    RegisterMessage = " * Something went wrong upon registeration";
                }
            }
        }
    }
}