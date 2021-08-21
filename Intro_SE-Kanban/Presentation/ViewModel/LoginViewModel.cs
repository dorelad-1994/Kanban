using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class LoginViewModel : NotifiableObject
    {
        /// <summary>
        /// Controller
        /// </summary>
        public BackendController Controller { get; private set; } 
        /// <summary>
        /// User's emaail - binding
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
        /// Error message from service - binding
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
        /// Error ocured - binding
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
        private string _password;
        /// <summary>
        /// Password input - binding
        /// </summary>
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
        /// Constructor
        /// </summary>
        public LoginViewModel()
        {
            Controller = new BackendController();
        }
        /// <summary>
        /// login user to project
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
            catch(Exception e)
            {
                ErrorOcured += "Error Message: "+ "\n";
                ErrorMessage = ErrorMessage + e.Message;
                return null;
            }
        }

    }
}
