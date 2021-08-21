using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class AddTaskViewModel : NotifiableObject
    {
        /// <summary>
        /// Controller
        /// </summary>
        public BackendController Controller { get; private set; }
        /// <summary>
        /// User Loged in
        /// </summary>
        private UserModel _user;
        public UserModel User
        {
            get => _user;
            set
            {
                _user = value;
            }
        }
        
      /// <summary>
      /// title to add to the new task
      /// </summary>
        private string _Title;
        public string Title
        {
            get => _Title;
            set
            {
                this._Title = value;
                RaisePropertyChanged("Title");
            }
        }
        /// <summary>
        /// description of the new task
        /// </summary>
        private string _Description;
        public string Description
        {
            get => _Description;
            set
            {
                this._Description = value;
                RaisePropertyChanged("Description");
            }
        }
       /// <summary>
       /// due date of the new task
       /// </summary>
        private DateTime _DueDate;
        public DateTime DueDate
        {
            get => _DueDate;
            set
            {
                this._DueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }
        /// <summary>
        /// the column of the task
        /// </summary>
        public ColumnModel column { get; private set; }

        
        /// <summary>
        /// Error message from backend project
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
        /// if error ocured in response
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
        /// Adding the new task
        /// </summary>
        /// <returns></returns>
        public TaskModel AddTask()
        {
            ErrorOcured = "";
            ErrorMessage = "";
            try
            {
                return Controller.AddTask(_user.Email,Title,Description,DueDate,column);
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
       /// <param name="user">loged in user</param>
       /// <param name="columnModel">column to add</param>
        public AddTaskViewModel(UserModel user, ColumnModel columnModel)
        {
            this.Controller = user.Controller;
            this._user = user;
            column = columnModel;
        }
    }
}

