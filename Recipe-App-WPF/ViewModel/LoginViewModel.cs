using Recipe_App_WPF.Model;
using Recipe_App_WPF.Repoistories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Recipe_App_WPF.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private string _email;
        private SecureString _password;
        private string _errorMessage;
        private bool _isViewVisible = true;

        private IUserRepository userRepoistory;

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public SecureString Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool IsViewVisible
        {
            get
            {
                return _isViewVisible;
            }
            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }
        // -> Command
        public ICommand LoginCommand { get; }
        public ICommand RecoverPasswordCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand RememberPasswordCommand { get; }

        public LoginViewModel()
        {
            userRepoistory = new UserRepository();
            LoginCommand = new ViewModelCommand(ExecuteLogicCommand, CanExecuteLoginCommand);
            RecoverPasswordCommand = new ViewModelCommand(p => ExecuteRecoverPassCommand(""));
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(Email) || Email.Length < 3 || Password == null || Password.Length < 3)
                validData = false;
            else
                validData = true;

            return validData;
        }

        private void ExecuteLogicCommand(object obj)
        {
            var isValidUser = userRepoistory.AuthenticateUser(new NetworkCredential(Email, Password));
            if (isValidUser)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Email), null);
                IsViewVisible = false;
            }
            else
            {
                ErrorMessage = "Invalid email or password";
            }
        }

        private void ExecuteRecoverPassCommand(string email)
        {
            throw new NotImplementedException();
        }
    }
}
