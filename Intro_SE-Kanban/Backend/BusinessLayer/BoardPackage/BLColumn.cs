using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DalObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    class BLColumn
    {
        //-------------------------------------------loger---------------------------------------------------------------------------

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //-------------------------------------------fields--------------------------------------------------------------------------

        private Dictionary<int, BLTask> _myList;
        public Dictionary<int, BLTask> myList
        {
            get { return _myList; }
            set { _myList = value; }
        }

        private int _limit;
        public int limit
        {
            get { return _limit; }
        }

        private int _ID;
        public int ID
        {
            get { return _ID; }
        }

        private int _Position;
        public int position
        {
            get { return _Position; }
            set { _Position = value; }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                LegalName(value);
                _Name = value;
                DalColumn c = new DalColumn();
                c.UpdateColumnName(ID, value);
            }
        }

        private int _UserID;
        public int UserID
        {
            get { return _UserID; }
        }

        private int _BoardID;
        private const int _MaxName = 15;

        //-----------------------------------------constarctor-----------------------------------------------------------------------

        /// <summary>
        /// Constractor for create BLColumn
        /// </summary>
        public BLColumn(String name, int position, int BoardID, int UserID)
        {
            LegalName(name);
            _Name = name;
            _myList = new Dictionary<int, BLTask>();
            _limit = 100;
            _Position = position;
            _BoardID = BoardID;
            _UserID = UserID;
            DalColumn dalC = new DalColumn(BoardID, UserID, Name, position);
            _ID = dalC.Insert();

        }

        /// <summary>
        /// Construcotr for loadData only
        /// </summary>
        /// <param name="name"></param>
        /// <param name="position"></param>
        /// <param name="BoardID"></param>
        /// <param name="UserID"></param>
        /// <param name="ID"></param>
        /// <param name="limit"></param>
        public BLColumn(String name, int position, int BoardID, int UserID, int ID, int limit)
        {
            _myList = new Dictionary<int, BLTask>();
            _limit = limit;
            _ID = ID;
            _Name = name;
            _Position = position;
            _BoardID = BoardID;
            _UserID = UserID;
        }

        /// <summary>
        /// constractor for loadData only
        /// </summary>
        public BLColumn()
        {

        }
        //-------------------------------------------methods-------------------------------------------------------------------------

        /// <summary>
        ///  This method add task to the user's kanban
        /// </summary>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date of the new task</param>
        /// <param name="taskId">The Id number of task that will create</param>
        /// <param name="assignee">the email of the user</param>
        public void addTask(string title, string description, DateTime dueTime, int taskID, string assignee)
        {
            BLTask task = new BLTask(dueTime, title, description, taskID, _UserID, _BoardID, ID, assignee);
            _myList.Add(taskID, task);
        }



        /// <summary>
        /// This method update task title
        /// </summary>
        /// <param name="taskId">the task Id number</param>
        /// <param name="title">New title for the task</param>
        /// <param name="assignee">the email of the user</param>
        public void updateTaskTitle(int taskID, string title, string assignee)
        {
            if (_myList.ContainsKey(taskID))  //check if the task is in this column
                _myList[taskID].SetTitle(title, assignee);
            else
            {
                log.Warn("Task " + taskID + " is not in this column");
                throw new ArgumentException("This task is not in this column");
            }
        }


        /// <summary>
        /// This method update task due time
        /// </summary>
        /// <param name="taskId">the task Id number</param>
        /// <param name="dueDate">New due date for the task</param>
        /// <param name="assignee">the email of the user</param>
        public void updateTaskDueTime(int taskID, DateTime dueDate, string assignee)
        {
            if (_myList.ContainsKey(taskID))  //check if the task is in this column
                _myList[taskID].SetDueTime(dueDate, assignee);
            else
            {
                log.Warn("Task " + taskID + " is not in this column");
                throw new ArgumentException("This task is not in this column");
            }
        }


        /// <summary>
        /// This method update task description
        /// </summary>
        /// <param name="taskId">the task Id number</param>
        /// <param name="description">New description for the task</param>
        /// <param name="assignee">the email of the user</param>
        public void updateTaskDescription(int taskID, string description, string assignee)
        {
            if (_myList.ContainsKey(taskID))  //check if the task is in this column
                _myList[taskID].SetDescription(description, assignee);
            else
            {
                log.Warn("Task " + taskID + " is not in this column");
                throw new ArgumentException("This task is not in this column");
            }
        }


        /// <summary>
        /// setter Limit the number of tasks in a specific column
        /// </summary>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        public void setMaxTasks(int limit)
        {
            if (limit < 0) //check if limit is legal
            {
                log.Warn("The limit number is negative");
                throw new ArgumentException("The limit is not legal");
            }
            if (_myList.Count > limit)
            {
                log.Warn("number of existing tasks is larger than input limit");
                throw new Exception($"Can't set limit to a lower value than {myList.Count}");
            }
            _limit = limit;
            DalColumn dc = new DalColumn();
            dc.UpdatelimitColumnTask(_ID, limit);
        }


        /// <summary>
        ///  change the assiggnee of the task
        /// </summary>
        /// <param name="taskID">the id of the task</param>
        /// <param name="email">the email of the user</param>
        /// =<param name="emailAssignee">the email of the new assingee</param>
        public void AssignTask(int taskID, string emailAssignee, string email)
        {
            if (email.Equals(emailAssignee))
            {
                log.Warn($"{emailAssignee} is the current assignee of this task");
                throw new ArgumentException($"{emailAssignee} is the current assignee of this task");
            }
            if (_myList.ContainsKey(taskID) && _myList[taskID].IsAssignee(email))  //check if the task is in this column
                _myList[taskID].AssigneeEmail = emailAssignee;
            else
            {
                log.Warn("Task " + taskID + " is not in this column");
                throw new ArgumentException("This task is not in this column");
            }
        }

        /// <summary>
        /// delete task from the column
        /// </summary>
        /// <param name="taskID">the id of the task</param>
        /// <param name="email">the email of the user</param>
        public void DeleteTask(int taskID, string email)
        {
            if (_myList.ContainsKey(taskID))
            {  //check if the task is in this column
                BLTask task = _myList[taskID];
                task.IsAssignee(email);
                _myList.Remove(taskID);
                DalTask t = new DalTask();
                t.DeleteTask(taskID, task.BoardID);
            }
            else
            {
                log.Warn("Task " + taskID + " is not in this column");
                throw new ArgumentException("This task is not in this column");
            }
        }



        //-----------------------------------Data----------------------------------------------------------

        /// <summary>
        /// Load Data From database
        /// </summary>
        /// <returns>dictionary of all columns in database</returns>
        public Dictionary<int, BLColumn> LoadData()
        {
            try
            {
                DalColumn dc = new DalColumn();
                List<DalColumn> listOfDalColumns = dc.SelectAllColumns();
                Dictionary<int, BLColumn> output = new Dictionary<int, BLColumn>();
                foreach (DalColumn dalc in listOfDalColumns)
                {
                    output.Add(Convert.ToInt32(dalc._ID), new BLColumn(dalc._ColumnName, Convert.ToInt32(dalc._Position), Convert.ToInt32(dalc._BoardID), Convert.ToInt32(dalc._UserID), Convert.ToInt32(dalc._ID), Convert.ToInt32(dalc._LimitC)));
                }
                BLTask bt = new BLTask();
                List<BLTask> listOfTasks = bt.LoadData();
                foreach (BLTask t in listOfTasks)
                {
                    int tasksColumn = t.ColumnID;
                    BLColumn bc = output[tasksColumn];
                    bc.myList.Add(t.ID, t);
                }
                return output;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        //---------------------------------getters&setters&other------------------------------------------------------

        /// <summary>
        /// check if the column is full of tasks
        /// </summary>
        /// <returns>true if the column is full, false if there is place for more task</returns>
        public bool full()
        {
            return _myList.Count == _limit;
        }


        /// <summary>
        /// Updating in all the tasks in one to column to be in the other column and after that deleting emtpy column from database
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="fromColumnID"></param>
        /// <param name="toColumnID"></param>
        public void UpdateColumnAfterDelete(int ID, int fromColumnID, int toColumnID)
        {
            DalColumn c = new DalColumn();
            c.UpdateColumnAfterDelete(ID, fromColumnID, toColumnID);
        }

        private void LegalName(string name)
        {
            if (String.IsNullOrEmpty(name) || name.Length > _MaxName)
            {
                log.Warn($"The name column - '{name}' is not legal");
                throw new ArgumentException($"'{name}' is not legal");
            }
        }
    }
}