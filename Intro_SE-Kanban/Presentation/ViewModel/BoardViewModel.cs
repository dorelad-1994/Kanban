using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using Presentation.Model;
using Presentation.View;

namespace Presentation.ViewModel
{
    class BoardViewModel : NotifiableObject
    {
        /// <summary>
        /// Controller
        /// </summary>
        private BackendController Controller { get;  set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user"></param>
        public BoardViewModel(UserModel user)
        {
            Controller = user.Controller;
            _User = user;
            BoardTitle = user.NickName;
            Board = Controller.GetBoard(user.Email);
        }

        /// <summary>
        /// Error message from service
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
        /// Error occured
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
        /// Board title in window - shows nickname and board kanban
        /// </summary>
        private string _BoardTitle;
        public string BoardTitle
        {
            get => $"{_BoardTitle}'s Kanban Board";
            set
            {
                _BoardTitle = value;
                RaisePropertyChanged("BoardTitle");
            }
        }
        /// <summary>
        /// Current loged in user
        /// </summary>
        private UserModel _User;
        public UserModel User
        {
            get => _User;
            set
            {
                this._User = value;
            }
        }
        /// <summary>
        /// users board from loading the window
        /// </summary>
        public BoardModel Board { get; set; }
        /// <summary>
        /// column name filed
        /// </summary>
        private string _ColumnName;
        public string ColumnName
        {
            get => _ColumnName;
            set
            {
                this._ColumnName = value;
                RaisePropertyChanged("ColumnName");
            }
        }
        /// <summary>
        /// filter search box binding
        /// </summary>
        private string _SearchText;
        public string SearchText
        {
            get => _SearchText;
            set
            {
                this._SearchText = value;
                RaisePropertyChanged("SearchText");
            }
        }
        /// <summary>
        /// column name input binding
        /// </summary>
        private string _InputColumnName;
        public string InputColumnName
        {
            get => _InputColumnName;
            set
            {
                _InputColumnName = value;
                RaisePropertyChanged("InputColumnName");
            }
        }
        /// <summary>
        /// column position binding
        /// </summary>
        private string _InputColumnPosition;
        public string InputColumnPosition
        {
            get => _InputColumnPosition;
            set
            {
                _InputColumnPosition = value;
                RaisePropertyChanged("InputColumnPosition");
            }
        }
        /// <summary>
        /// column limit binding
        /// </summary>
        private string _InputColumnLimit;
        public string InputColumnLimit
        {
            get => _InputColumnLimit;
            set
            {
                _InputColumnLimit = value;
                RaisePropertyChanged("InputColumnLimit");
            }
        }
        /// <summary>
        /// selected task that the user selected binding
        /// </summary>
        private TaskModel _selectedTask;
        public TaskModel SelectedTask
        {
            get
            {
                return _selectedTask;
            }
            set
            {
                _selectedTask = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedTask");
            }
        }
        /// <summary>
        /// filter input binding
        /// </summary>
        private string _Filter;
        public string Filter
        {
            get => _Filter;
            set
            {
                this._Filter = value;
                RaisePropertyChanged("Filter");
            }
        }
       /// <summary>
       /// selected column that the user selected in window - binding
       /// </summary>
        private ColumnModel _selectedColumn;
        public ColumnModel SelectedColumn
        {
            get
            {
                return _selectedColumn;
            }
            set
            {
                EnableForward = false;
                _selectedColumn = value;
                RaisePropertyChanged("SelectedColumn");
            }
        }
        /// <summary>
        /// column input name in binding
        /// </summary>
        private string _EditColumnName;
        public string EditColumnName
        {
            get => _EditColumnName;
            set
            {
                _EditColumnName = value;
                RaisePropertyChanged("EditColumnName");
            }
        }
        /// <summary>
        /// enable the label/textbox in window
        /// </summary>
        private bool _enableForward = false;
        public bool EnableForward
        {
            get => _enableForward;
            set
            {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
            }
        }
        /// <summary>
        /// sort by due date from window
        /// </summary>
        public void SortByDueDate()
        {
            Board.SortByDueDate();
        }
        /// <summary>
        /// log out the current user from project
        /// </summary>
        public void Logout()
        {
            ErrorOcured = "";
            ErrorMessage = "";
            try
            {
                Controller.Logout(User.Email);
            }
            catch (Exception e)
            {
                ErrorOcured += "Error Message: " + "\n";
                ErrorMessage = ErrorMessage + e.Message;
            }
        }
        /// <summary>
        /// delete the selected task
        /// </summary>
        public void DeleteTask()
        {
            ErrorOcured = "";
            ErrorMessage = "";
            try
            {
                Controller.DeleteTask(User.Email, SelectedTask.Parent.Position, SelectedTask.ID);
                Board.Columns[SelectedTask.Parent.Position].RemoveTask(SelectedTask);
            }
            catch (Exception e)
            {
                ErrorOcured += "Error Message: " + "\n";
                ErrorMessage = ErrorMessage + e.Message;
            }
        }
        /// <summary>
        /// filter the columns to input filter
        /// </summary>
        public void FilterTask()
        {
            if (Filter != null)
            {
                Board.Filter(Filter);
            }
        }
        /// <summary>
        /// advance a selected task
        /// </summary>
        public void AdvanceTask()
        {
            ErrorOcured = "";
            ErrorMessage = "";
            try
            {
                Controller.AdvanceTask(User.Email, SelectedTask.Parent.Position, SelectedTask.ID);
                SelectedTask.Parent.Advance(Board.Columns[SelectedTask.Parent.Position + 1], SelectedTask);
            }
            catch (Exception e)
            {
                ErrorOcured += "Error Message: " + "\n";
                ErrorMessage = ErrorMessage + e.Message;
            }
        }
        /// <summary>
        /// move a selected column left
        /// </summary>
        public void MoveLeft()
        {
            ErrorOcured = "";
            ErrorMessage = "";
            try
            {
                int indexOfSelected = SelectedColumn.Position;
                Controller.MoveColumnLeft(User.Email, indexOfSelected);
                Board.Swap(indexOfSelected - 1, indexOfSelected);
            }
            catch (Exception e)
            {
                if (SelectedColumn is null)
                {
                    ErrorOcured += "Error Message: " + "\n";
                    ErrorMessage = ErrorMessage + "Please select a column";
                }
                else
                {
                    ErrorOcured += "Error Message: " + "\n";
                    ErrorMessage = ErrorMessage + e.Message;
                }
            }

        }
        /// <summary>
        /// move a selected column right
        /// </summary>
        public void MoveRight()
        {
            ErrorOcured = "";
            ErrorMessage = "";
            try
            {
                int indexOfSelected = SelectedColumn.Position;
                Controller.MoveColumnRight(User.Email, indexOfSelected);
                Board.Swap(indexOfSelected, indexOfSelected + 1);
            }
            catch (Exception e)
            {
                if (SelectedColumn is null)
                {
                    ErrorOcured += "Error Message: " + "\n";
                    ErrorMessage = ErrorMessage + "Please select a column";
                }
                else
                {
                    ErrorOcured += "Error Message: " + "\n";
                    ErrorMessage = ErrorMessage + e.Message;
                }
            }

        }
        /// <summary>
        /// add a new column in a specific position
        /// </summary>
        public void AddColumn()
        {
            ErrorOcured = "";
            ErrorMessage = "";
            try
            {
                ColumnModel c = Controller.AddColumn(User.Email, Convert.ToInt32(InputColumnPosition), InputColumnName);
                Board.AddColumn(c, Convert.ToInt32(InputColumnPosition));
            }
            catch (FormatException)
            {
                ErrorOcured += "Error Message: " + "\n";
                ErrorMessage = "Position input is not a number";
            }
            catch (Exception e)
            {
                ErrorOcured += "Error Message: " + "\n";
                ErrorMessage = ErrorMessage + e.Message;
            }
        }
        /// <summary>
        /// remove/delete selected column
        /// </summary>
        public void RemoveColumn()
        {
            ErrorOcured = "";
            ErrorMessage = "";
            try
            {
                Controller.RemoveColumn(User.Email, SelectedColumn.Position);
                Board.RemoveColumn(SelectedColumn, SelectedColumn.Position);
            }
            catch (Exception e)
            {
                if (SelectedColumn is null)
                {
                    ErrorOcured += "Error Message: " + "\n";
                    ErrorMessage = ErrorMessage + "Please select a column";
                }
                else
                {
                    ErrorOcured += "Error Message: " + "\n";
                    ErrorMessage = ErrorMessage + e.Message;
                }
            }
        }
        /// <summary>
        /// limit selected column
        /// </summary>
        public void LimitColumn()
        {
            ErrorOcured = "";
            ErrorMessage = "";
            try
            {
                Controller.LimitColumnTask(User.Email, SelectedColumn.Position, Convert.ToInt32(InputColumnLimit));
                Board.LimitColumn(SelectedColumn.Position, Convert.ToInt32(InputColumnLimit));
            }
            catch (FormatException)
            {
                   ErrorOcured += "Error Message: " + "\n";
                   ErrorMessage = "Input is not a number";
            }
            catch (Exception e)
            {
                if (SelectedColumn is null)
                {
                    ErrorOcured += "Error Message: " + "\n";
                    ErrorMessage = ErrorMessage + "Please select a column";
                }
                else
                {
                    ErrorOcured += "Error Message: " + "\n";
                    ErrorMessage = ErrorMessage + e.Message;
                }
            }
            
        }
        /// <summary>
        /// change selected column name
        /// </summary>
        public void ChangeColumnName()
        {
            ErrorOcured = "";
            ErrorMessage = "";
            try
            {
                Controller.ChangeColumnName(User.Email, SelectedColumn.Position, EditColumnName);
                Board.UpdateColumnName(SelectedColumn.Position, EditColumnName);
            }
            catch (Exception e)
            {
                if (SelectedColumn is null)
                {
                    ErrorOcured += "Error Message: " + "\n";
                    ErrorMessage = ErrorMessage + "Please select a column";
                }
                else
                {
                    ErrorOcured += "Error Message: " + "\n";
                    ErrorMessage = ErrorMessage + e.Message;
                }
            }
        }
    }
}


