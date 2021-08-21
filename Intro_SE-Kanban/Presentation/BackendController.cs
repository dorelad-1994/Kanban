using IntroSE.Kanban.Backend.ServiceLayer;
using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Presentation
{
    public class BackendController
    {
        /// <summary>
        /// field
        /// </summary>
        public IService Service { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public BackendController()
        {
            Service = new Service();
            Service.LoadData();
        }
        /// <summary>
        /// Login to Kanban
        /// </summary>
        /// <param name="email">email to sign in</param>
        /// <param name="password">password of user</param>
        /// <returns>UserModel object</returns>
        public UserModel Login(string email, string password)
        {
            Response<User> user = Service.Login(email, password);
            if (user.ErrorOccured)
            {
                throw new Exception(user.ErrorMessage);
            }
            return new UserModel(this, email, user.Value.Nickname);
        }
        /// <summary>
        /// Register a new user to kanban with new board
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <param name="password">password of the user</param>
        /// <param name="nickname">nickname of the user</param>
        public void Register(string email, string password, string nickname)
        {
            Response reg = Service.Register(email, password, nickname);
            if (reg.ErrorOccured)
            {
                throw new Exception(reg.ErrorMessage);
            }
        }
        /// <summary>
        /// Register a new user into an existing board in kanban
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <param name="password">password of the user</param>
        /// <param name="nickname">nickname of the user</param>
        /// <param name="emailHost">email of the existing user that has a board in kanban</param>
        public void Register(string email, string password, string nickname, string emailHost)
        {
            Response reg = Service.Register(email, password, nickname, emailHost);
            if (reg.ErrorOccured)
            {
                throw new Exception(reg.ErrorMessage);
            }
        }
        /// <summary>
        /// Logout from Kanban
        /// </summary>
        /// <param name="email">email of the user that is currently logged in</param>
        public void Logout(string email)
        {
            Response logout = Service.Logout(email);
            if (logout.ErrorOccured)
            {
                throw new Exception(logout.ErrorMessage);
            }
        }
        /// <summary>
        /// Add a task to the first column (backlog)
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <param name="title">title of the task</param>
        /// <param name="description">description of the task</param>
        /// <param name="dueDate">due date and time</param>
        /// <returns>TaskModel object</returns>
        public TaskModel AddTask(string email, string title, string description, DateTime dueDate, ColumnModel column)
        {
            Response<Task> task = Service.AddTask(email, title, description, dueDate);
            if (task.ErrorOccured)
            {
                throw new Exception(task.ErrorMessage);
            }
            return new TaskModel(this, task.Value.Id, task.Value.emailAssignee, task.Value.Title, task.Value.Description, task.Value.CreationTime, task.Value.DueDate, column);
        }
        /// <summary>
        /// Update a specific task title
        /// </summary>
        /// <param name="email">email of the loged in user</param>
        /// <param name="columnOrdinal">position of the column in the board that the task is in</param>
        /// <param name="taskID"> id of the task</param>//
        /// <param name="newTitle">new title to update</param>
        public void UpdateTaskTitle(string email, int columnOrdinal, int taskID, string newTitle)
        {
            Response res = Service.UpdateTaskTitle(email, columnOrdinal, taskID, newTitle);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        /// <summary>
        /// Update a specific task description
        /// </summary>
        /// <param name="email">email of the loged in user</param>
        /// <param name="columnOrdinal">position of the column in the board that the task is in</param>
        /// <param name="taskID"> id of the task</param>
        /// <param name="newDesc">new description to update</param>
        public void UpdateTaskDescription(string email, int columnOrdinal, int taskID, string newDesc)
        {
            Response res = Service.UpdateTaskDescription(email, columnOrdinal, taskID, newDesc);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        /// <summary>
        /// Update a specific task Due Time and Date
        /// </summary>
        /// <param name="email">email of the loged in user</param>
        /// <param name="columnOrdinal">position of the column in the board that the task is in</param>
        /// <param name="taskID"> id of the task</param>
        /// <param name="newDueDate"> new due date time</param>
        public void UpdateTaskDueDate(string email, int columnOrdinal, int taskID, DateTime newDueDate)
        {
            Response res = Service.UpdateTaskDueDate(email, columnOrdinal, taskID, newDueDate);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        /// <summary>
        /// Advance a specific task to the next column from left to right ('done' column not encluded)
        /// </summary>
        /// <param name="email">email of the current logged in user</param>
        /// <param name="columnOrdinal">Column ordinal where the task is to be advanced</param>
        /// <param name="taskID">task id</param>
        public void AdvanceTask(string email, int columnOrdinal, int taskID)
        {
            Response ad = Service.AdvanceTask(email, columnOrdinal, taskID);
            if (ad.ErrorOccured)
            {
                throw new Exception(ad.ErrorMessage);
            }
        }
        /// <summary>
        /// Limit a specific Column to number of tasks
        /// </summary>
        /// <param name="email">email of the current logged in user</param>
        /// <param name="columnOrdinal">ordinal of the column to be limited</param>
        /// <param name="newLimit"> new limit number</param>
        public void LimitColumnTask(string email, int columnOrdinal, int newLimit)
        {
            Response lim = Service.LimitColumnTasks(email, columnOrdinal, newLimit);
            if (lim.ErrorOccured)
            {
                throw new Exception(lim.ErrorMessage);
            }
        }
        /// <summary>
        /// Gets the specific column of the current logged in user
        /// </summary>
        /// <param name="email">current logged in user</param>
        /// <param name="columnName">column name</param>
        /// <returns>ColumnModel object</returns>
        public ColumnModel GetColumn(string email, string columnName, int position)
        {
            Response<Column> c = Service.GetColumn(email, columnName);
            if (c.ErrorOccured)
            {
                throw new Exception(c.ErrorMessage);
            }
            ColumnModel column = new ColumnModel(this, c.Value.Name, c.Value.Limit, position);
            List<TaskModel> listOfTasks = new List<TaskModel>(
                c.Value.Tasks.Select((t) => new TaskModel(this, t.Id, t.emailAssignee, t.Title, t.Description, t.CreationTime, t.DueDate, column)).ToList());
            listOfTasks.ForEach(t => t.Logdin = email);
            column.Tasks = new ObservableCollection<TaskModel>(listOfTasks);
            column.NonFilteredTasks = new ObservableCollection<TaskModel>(listOfTasks);
            return column;
        }
        
        
        /// <summary>
        /// Gets the current logged in user's board
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public BoardModel GetBoard(string email)
        {
            Response<Board> board = Service.GetBoard(email);
            if (board.ErrorOccured)
            {
                throw new Exception(board.ErrorMessage);
            }
            int position = 0;
            List<ColumnModel> listOfColumns = board.Value.ColumnsNames.Select(name => GetColumn(email, name, position++)).ToList();
            return new BoardModel(this, board.Value.emailCreator, listOfColumns);
        }
        /// <summary>
        /// Removes a Column from users board
        /// </summary>
        /// <param name="email">email of current logged in user</param>
        /// <param name="columnOrdinal">specific column ordinal</param>
        public void RemoveColumn(string email, int columnOrdinal)
        {
            Response res = Service.RemoveColumn(email, columnOrdinal);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        /// <summary>
        /// Add a Column to a specific board
        /// </summary>
        /// <param name="email"> email of current logged in user</param>
        /// <param name="columnOrdinal">the place to put the column in board</param>
        /// <param name="name">name of the new column</param>
        /// <returns></returns>
        public ColumnModel AddColumn(string email, int columnOrdinal, string name)
        {
            Response<Column> c = Service.AddColumn(email, columnOrdinal, name);
            if (c.ErrorOccured)
            {
                throw new Exception(c.ErrorMessage);
            }
            ColumnModel column = new ColumnModel(this, c.Value.Name, c.Value.Limit, columnOrdinal);
            List<TaskModel> listOfTasks = new List<TaskModel>();
            column.Tasks = new ObservableCollection<TaskModel>(listOfTasks);
            column.NonFilteredTasks = new ObservableCollection<TaskModel>(listOfTasks);
            return column;
        }
        /// <summary>
        /// Moves a column to the right
        /// </summary>
        /// <param name="email">email of the current logged in user</param>
        /// <param name="columnOrdinal">column position in board to move</param>
        /// <returns>the column that was moved</returns>
        public void MoveColumnRight(string email, int columnOrdinal)
        {
            Response<Column> c = Service.MoveColumnRight(email, columnOrdinal);
            if (c.ErrorOccured)
            {
                throw new Exception(c.ErrorMessage);
            }
            //List<TaskModel> listOfTasks = new List<TaskModel>(
            //   c.Value.Tasks.Select((task) => new TaskModel(this, task.Id, task.emailAssignee, task.Title, task.Description, task.CreationTime, task.DueDate)).ToList());
            //return new ColumnModel(this, c.Value.Name, c.Value.Limit, listOfTasks, columnOrdinal);
        }
        /// <summary>
        /// Move a column to the left
        /// </summary>
        /// <param name="email">email of the current logged in user</param>
        /// <param name="columnOrdinal">column position in board to move</param>
        /// <returns>the column that was moved</returns>
        public void MoveColumnLeft(string email, int columnOrdinal)
        {
            Response<Column> c = Service.MoveColumnLeft(email, columnOrdinal);
            if (c.ErrorOccured)
            {
                throw new Exception(c.ErrorMessage);
            }
            //List<TaskModel> listOfTasks = new List<TaskModel>(
            //   c.Value.Tasks.Select((t) => new TaskModel(this, t.Id, t.emailAssignee, t.Title, t.Description, t.CreationTime, t.DueDate)).ToList());
            //return new ColumnModel(this, c.Value.Name, c.Value.Limit, listOfTasks, columnOrdinal);
        }
        /// <summary>
        /// Assigns a specific task in board to a different user
        /// </summary>
        /// <param name="email">email of the current logged in user</param>
        /// <param name="columnOrdinal">the ordinal where the task is in</param>
        /// <param name="taskID">id of the specific task</param>
        /// <param name="emailAssignee">email to assign the task</param>
        public void AssignTask(string email, int columnOrdinal, int taskID, string emailAssignee)
        {
            Response at = Service.AssignTask(email, columnOrdinal, taskID, emailAssignee);
            if (at.ErrorOccured)
            {
                throw new Exception(at.ErrorMessage);
            }
        }
        /// <summary>
        /// delete a specific task in a specific column
        /// </summary>
        /// <param name="email">email of the current logged in user</param>
        /// <param name="columnOrdinal">the ordinal where the task is in</param>
        /// <param name="taskID">id of the specific task</param>
        public void DeleteTask(string email, int columnOrdinal, int taskID)
        {
            Response dt = Service.DeleteTask(email, columnOrdinal, taskID);
            if (dt.ErrorOccured)
            {
                throw new Exception(dt.ErrorMessage);
            }
        }
        /// <summary>
        /// Changes the column name
        /// </summary>
        /// <param name="email">email of the current logged in user</param>
        /// <param name="columnOrdinal">ordinal of the column to change the name</param>
        /// <param name="newName">new name to set</param>
        public void ChangeColumnName(string email, int columnOrdinal, string newName)
        {
            Response cc = Service.ChangeColumnName(email, columnOrdinal, newName);
            if (cc.ErrorOccured)
            {
                throw new Exception(cc.ErrorMessage);
            }
        }


    }
}
