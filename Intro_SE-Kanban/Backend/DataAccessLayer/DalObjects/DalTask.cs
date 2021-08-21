using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalObjects
{
    class DalTask : DalObject
    {
        public const string CNTitle = "Title";
        public const string CNDescription = "Description";
        public const string CNDueDate = "DueDate";
        public const string CNCreationDate = "CreationDate";
        public const string CNColumnID = "ColumnID";
        public const string CNUserID = "UserID";
        public const string CNBoardID = "BoardID";
        public const string CNAssignee = "Assignee";
        //-----------------------FIELDS-----------------
        public string _AssigneeEmail { get; set; }
        public string _title { get; set; }
        public string _description { get; set; }
        public DateTime _creationDate { get; set; }
        public DateTime _dueDate { get; set; }
        public long _ColumnID { get; set; }
        public long _BoardID { get; set; }
        public long _UserID { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="dueTime"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="ColumnID"></param>
        /// <param name="BoardID"></param>
        /// <param name="UserID"></param>
        public DalTask(int taskID, int ColumnID, int BoardID, int UserID, string title, string description,long CreationDate, DateTime dueDate,string assignee) : base(new DalTaskController())
        {
            _ID = taskID;
            _title = title;
            _description = description;
            _creationDate = new DateTime(CreationDate);
            _dueDate = dueDate;
            _ColumnID = ColumnID;
            _BoardID = BoardID;
            _UserID = UserID;
            _AssigneeEmail = assignee;

        }
        /// <summary>
        /// Constructor for Reading from data in database(CONVERT)
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="ColumnID"></param>
        /// <param name="BoardID"></param>
        /// <param name="UserID"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="dueDate"></param>
        /// <param name="CreationDate"></param>
        public DalTask(long taskID, long ColumnID, long BoardID, long UserID, string title, string description, DateTime CreationDate, DateTime dueDate, string assignee) : base(new DalTaskController())
        {
            _ID = taskID;
            _title = title;
            _description = description;
            _creationDate = CreationDate;
            _dueDate = dueDate;
            _ColumnID = ColumnID;
            _BoardID = BoardID;
            _UserID = UserID;
            _AssigneeEmail = assignee;

        }
        public DalTask() : base(new DalTaskController())
        {

        }
        /// <summary>
        /// Inserts a new Task in database
        /// </summary>
        /// <returns>ID of Task</returns>
        public int Insert()
        {
            return _controller.Insert(this);
        }
        /// <summary>
        /// Updates title in a task
        /// </summary>
        /// <param name="taskID">ID of specific task</param>
        /// <param name="boardID">ID of board that the task is in</param>
        /// <param name="title">new title to update</param>
        public void UpdateTitle(long taskID, long boardID, string title)
        {
            _controller.Update(taskID, boardID, CNTitle, title);
        }
        /// <summary>
        /// Update Descrition in a task
        /// </summary>
        /// <param name="taskID">ID of specific task</param>
        /// <param name="boardID">ID of board that the task is in</param>
        /// <param name="description">new description to update</param>
        public void UpdateDescription(long taskID, long boardID, string description)
        {
            _controller.Update(taskID, boardID, CNDescription, description);
        }
        /// <summary>
        /// Update DueDate in task
        /// </summary>
        /// <param name="taskID">ID of specific task</param>
        /// <param name="boardID">ID of board that the task is in</param>
        /// <param name="date"> new date to update</param>
        public virtual void UpdateDueDate(long taskID, long boardID, DateTime date)
        {
            _controller.Update(taskID, boardID, CNDueDate, date.ToString());
        }
        /// <summary>
        /// fetch the list of all existing tasks in database
        /// </summary>
        /// <returns>list of dal tasks</returns>
        public List<DalTask> SelectAllTasks()
        {
            return ((DalTaskController)_controller).SelectAllTasks();
        }
        /// <summary>
        /// Update the column ID
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="BoardID"></param>
        /// <param name="newColumnID"></param>
        public void UpdateColumnID(long taskID,long BoardID, long newColumnID)
        {
            _controller.Update(taskID, BoardID, CNColumnID, newColumnID);
        }
        /// <summary>
        /// Updated the new Assignee email in database
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="BoardID"></param>
        /// <param name="newAssignee"></param>
        public void UpdateAssignee(long taskID, long BoardID, string newAssignee)
        {
            _controller.Update(taskID, BoardID, CNAssignee, newAssignee);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="BoardID"></param>
        public void DeleteTask(long taskID, long BoardID)
        {
            ((DalTaskController)_controller).DeleteTask(taskID, BoardID);
        }
    }
}
