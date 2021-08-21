using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class RegisterViewModel : NotifiableObject
    {
        /// <summary>
        /// Controller
        /// </summary>
        public BackendController Controller { get; private set; }  
        /// <summary>
        /// Email of register user - binding
        /// </summary>
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }
        /// <summary>
        /// Password input for registration - binding
        /// </summary>
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                RaisePropertyChanged("Password");
            }
        }
        /// <summary>
        /// Nickname input for registarion - binding
        /// </summary>
        private string _nickName;
        public string NickName
        {
            get => _nickName;
            set
            {
                _nickName = value;
                RaisePropertyChanged("NickName");
            }
        }
        /// <summary>
        /// error message form service - binding
        /// </summary>
        private string _errorMsg;
        public string ErrorMessage
        {
            get => _errorMsg;
            set
            {
                _errorMsg = value;
                RaisePropertyChanged("ErrorMessage");
            }
        }
        /// <summary>
        /// error ocured -binding
        /// </summary>
        private string _errorOcured;
        public string ErrorOcured
        {
            get => _errorOcured;
            set
            {
                _errorOcured = value;
                RaisePropertyChanged("ErrorOcured");
            }
        }
        /// <summary>
        /// Email host input - binding
        /// </summary>
        private string _emailHost;
        public string EmailHost
        {
            get => _emailHost;
            set
            {
                _emailHost = value;
                RaisePropertyChanged("EmailHost");
            }
        }
        /// <summary>
        /// Register the new user with all the info input
        /// </summary>
        /// <returns></returns>
        public bool Register()
        {
            ErrorOcured = "";
            ErrorMessage = "";
            try
            {
                if (String.IsNullOrEmpty(EmailHost))
                {
                    Controller.Register(Email, Password, NickName);
                    return true;
                }
                else
                {
                    Controller.Register(Email, Password, NickName,EmailHost);
                    return true;
                }
            }
            catch (Exception e)
            {
                ErrorOcured += "Error Message: " + "\n";
                ErrorMessage += e.Message;
                return false;
            }
        }

        /// <summary>
        /// login the user after registration
        /// </summary>
        /// <returns></returns>
        public UserModel Login()
        {
            ErrorOcured = "";
            ErrorMessage = "";
            try
            {
                return Controller.Login(Email, Password);
            }
            catch (Exception e)
            {
                ErrorOcured += "Error Message: " + "\n";
                ErrorMessage = ErrorMessage + e.Message;
                return null;
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public RegisterViewModel()
        {
            Controller = new BackendController();
        }
    }
}
