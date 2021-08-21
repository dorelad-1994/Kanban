using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class TaskViewModel : NotifiableObject
    {
        /// <summary>
        /// Controller
        /// </summary>
        public BackendController Controller { get; private set; }
        /// <summary>
        /// Loged in user
        /// </summary>
        public UserModel User { get; set; }
        /// <summary>
        /// position of the task in board
        /// </summary>
        private int _position;
        /// <summary>
        /// Id of the task
        /// </summary>
        public int TaskId { get; set; }    
        /// <summary>
        /// Title of the task - binding
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
        /// Derscription of the task - binding
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
        /// Assignee of the task  - binding
        /// </summary>
        private string _Assignee;
        public string Assignee
        {
            get => _Assignee;
            set
            {
                this._Assignee = value;
                RaisePropertyChanged("Assignee");
            }
        }
        /// <summary>
        /// Creation time of the task
        /// </summary>
        private DateTime _CreationTime;
        public DateTime CreationTime
        {
            get => _CreationTime;
        }
        /// <summary>
        /// due date time of the task - binding
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
        /// error message from service - binding
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
        /// error ocured - binding
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
        /// update the task title in collection
        /// </summary>
        public void UpdateTaskTitle()
        {
            ErrorOcured = "";
            ErrorMessage = "";
            try
            {
                Controller.UpdateTaskTitle(User.Email, _position, TaskId, Title);
                
            }
            catch (Exception e)
            {
                ErrorOcured += "Error Message: " + "\n";
                ErrorMessage = ErrorMessage + e.Message;
                throw new Exception(ErrorMessage);
            }
        }
        /// <summary>
        /// update the task description in collection
        /// </summary>
        public void UpdateTaskDescription()
        {
            ErrorOcured = "";
            ErrorMessage = "";
            try
            {
                Controller.UpdateTaskDescription(User.Email, _position, TaskId, Description);
               
            }
            catch (Exception e)
            {
                ErrorOcured += "Error Message: " + "\n";
                ErrorMessage = ErrorMessage + e.Message;
                Console.WriteLine(ErrorMessage);
                throw new Exception(ErrorMessage);
            }
        }
        /// <summary>
        /// update due date of task in collection
        /// </summary>
        public void UpdateTaskDueDate()
        {
            ErrorOcured = "";
            ErrorMessage = "";
            try
            {
                Controller.UpdateTaskDueDate(User.Email, _position, TaskId, DueDate);
             
            }
            catch (Exception e)
            {
                ErrorOcured += "Error Message: " + "\n";
                ErrorMessage = ErrorMessage + e.Message;
                throw new Exception(ErrorMessage);
            }
        }
        /// <summary>
        /// update assignne in collection
        /// </summary>
        public void assigneeTask()
        {
            ErrorOcured = "";
            ErrorMessage = "";
            try
            {
                Controller.AssignTask(User.Email, _position, TaskId, Assignee);
            }
            catch (Exception e)
            {
                ErrorOcured += "Error Message: " + "\n";
                ErrorMessage = ErrorMessage + e.Message;
                throw new Exception(ErrorMessage);
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user">loged in user</param>
        /// <param name="task">the task</param>
        public TaskViewModel(UserModel user, TaskModel task)
        {
            this.Controller = user.Controller;
            this.User = user;
            TaskId = task.ID;
            _Title = task.Title;
            _CreationTime = task.CreationTime;
            _Description = task.Description;
            _Assignee = task.Assignee;
            _DueDate = task.DueDate;
            this._position = task.Parent.Position;
        }
    }
}



