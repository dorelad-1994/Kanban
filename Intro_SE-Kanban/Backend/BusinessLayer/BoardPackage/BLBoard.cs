using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DalObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    class BLBoard
    {
        //-------------------------------------fields-------------------------------------------

        private Dictionary<int, BLColumn> _boardArray;
        public Dictionary<int, BLColumn> boardArray
        {
            get { return _boardArray; }
            set { _boardArray = value; }
        }

        private int _UserID;
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        private int _ID;
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private int _TaskCounter;
        public int TaskCounter
        {
            get { return _TaskCounter; }
            set { _TaskCounter = value; }
        }
        private string _Creator;
        public string Creator
        {
            get { return _Creator; }
        }

        //-----------------------------------constractor-----------------------------------------

        /// <summary>
        /// Constractor of BLBoard - create Array of BLColumns(list) 
        /// </summary>
        public BLBoard(string email)
        {
            _boardArray = new Dictionary<int, BLColumn>();
            _TaskCounter = 1;
            _Creator = email;
            DalBoard dalB = new DalBoard();
            _UserID = dalB.getUserID(email);
            DalBoard dalB2 = new DalBoard(_UserID, _TaskCounter, email);
            _ID = dalB2.Insert();
            _boardArray.Add(0, new BLColumn("backlog", 0, _ID, _UserID));
            _boardArray.Add(1, new BLColumn("in progress", 1, _ID, _UserID));
            _boardArray.Add(2, new BLColumn("done", 2, _ID, _UserID));
        }

        /// <summary>
        /// Construcotr for loadData only
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="UserID"></param>
        /// <param name="TaskCounter"></param>
        public BLBoard(int ID, int UserID, int TaskCounter, string email)
        {
            _boardArray = new Dictionary<int, BLColumn>();
            _ID = ID;
            _UserID = UserID;
            _TaskCounter = TaskCounter;
            _Creator = email;
        }

        /// <summary>
        /// constractor for loadData only
        /// </summary>
        public BLBoard()
        {

        }

        //-------------------------------------loger--------------------------------------------
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //-------------------------------------methods------------------------------------------


        /// <summary>
        ///  This method add task to the user's kanban
        /// </summary>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date of the new task</param>
        /// <param name="taskId">The Id number of task that will create</param>
        /// <param name="assignee">the email of the user</param>
        public int addTask(String title, String description, DateTime dueDate, String assignee)
        {
            if (_boardArray[0].full()) //check if the column is full
            {
                log.Warn("The column is full");
                throw new Exception($"{boardArray[0].Name} is at max limit of tasks setted");
            }
            _boardArray[0].addTask(title, description, dueDate, _TaskCounter, assignee); //addTask Only to most left column
            _TaskCounter = _TaskCounter + 1;
            DalBoard db = new DalBoard();
            db.UpdateTaskCounter(ID, _TaskCounter);
            return _TaskCounter - 1;
        }

        /// <summary>
        /// This method update task title
        /// </summary>
        /// <param name="columnOrdinal">The column ID.</param>
        /// <param name="taskId">the task Id number</param>
        /// <param name="title">New title for the task</param>
        /// <param name="assignee">the email of the user</param>
        public void updateTaskTitle(int columnOrdinal, int taskID, String title, string assignee)
        {
            if (legalTaskID(taskID) && NotLastColumn(columnOrdinal))     //update tasks only if the task is not the most right column and the task is in this column
                _boardArray[columnOrdinal].updateTaskTitle(taskID, title, assignee);
        }

        /// <summary>
        /// This method update task due time
        /// </summary>
        /// <param name="columnOrdinal">The column ID.</param>
        /// <param name="taskId">the task Id number</param>
        /// <param name="dueDate">New due date for the task</param>
        /// <param name="assignee">the email of the user</param>
        public void updateTaskDueTime(int columnOrdinal, int taskID, DateTime dueDate, string assignee)
        {
            if (legalTaskID(taskID) && NotLastColumn(columnOrdinal))     //update tasks only if the task is not the most right column and the task is in this column
                _boardArray[columnOrdinal].updateTaskDueTime(taskID, dueDate, assignee);
        }

        /// <summary>
        /// This method update task description
        /// </summary>
        /// <param name="columnOrdinal">The column ID.</param>
        /// <param name="taskId">the task Id number</param>
        /// <param name="description">New description for the task</param>
        /// <param name="assignee">the email of the user</param>
        public void updateTaskDescription(int columnOrdinal, int taskID, String description, string assignee)
        {
            if (legalTaskID(taskID) && NotLastColumn(columnOrdinal))    //update tasks only if the task is not the most right column and the task is in this column
                _boardArray[columnOrdinal].updateTaskDescription(taskID, description, assignee);
        }

        /// <summary>
        /// Advance a task to the next column
        /// </summary>
        /// <param name="columnOrdinal">The column ID.</param>
        /// <param name="taskId">the task Id number</param>
        /// <param name="assignee">the email of the user</param>
        public void advanceTask(int columnOrdinal, int taskId, string assignee)
        {
            if (legalTaskID(taskId) && NotLastColumn(columnOrdinal))   //update tasks only if the task is in this board and the columnordinal is not most left column
            {
                Dictionary<int, BLTask> listTask = _boardArray[columnOrdinal].myList;
                if (!listTask.ContainsKey(taskId)) //check if the task is in this column
                {
                    log.Warn("Column '" + columnOrdinal + "' does not contain this task");
                    throw new ArgumentException("The task is not in this column");
                }
                if (_boardArray[columnOrdinal + 1].full()) //check if the next column is full
                {
                    log.Warn("The next column is full");
                    throw new Exception(($"{boardArray[columnOrdinal + 1].Name} is at max limit of tasks setted"));
                }

                BLTask toAdvance = listTask[taskId];
                toAdvance.IsAssignee(assignee); //check if the user is the assignee of this task
                toAdvance.ColumnID = _boardArray[columnOrdinal + 1].ID;
                _boardArray[columnOrdinal + 1].myList.Add(taskId, toAdvance);  // add the task to the next column
                _boardArray[columnOrdinal].myList.Remove(taskId);  //remove the task from the column
                log.Info("Task '" + taskId + "' was moved to the next column");
                DalTask dalt = new DalTask();
                dalt.UpdateColumnID(taskId, _ID, _boardArray[columnOrdinal + 1].ID);
            }
        }

        /// <summary>
        /// Limit the number of tasks in a specific column
        /// </summary>
        /// <param name="columnOrdinal">The column ID.</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <param name="email">the email of the user</param>
        public void limitColumnTasks(int columnOrdinal, int limit, string email)
        {
            if (LegalColumnOrdinal(columnOrdinal) && IsCreator(email))
                _boardArray[columnOrdinal].setMaxTasks(limit);
        }

        /// <summary>
        /// Returns a column given it's name
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <returns>return a BLCloumn</returns>
        public BLColumn getColumn(String columnName)
        {
            if (columnName == null)
            {
                log.Warn("The column name is null");
                throw new ArgumentNullException("The column name is null");
            }
            else
            {
                foreach (KeyValuePair<int, BLColumn> entry in _boardArray)
                {
                    if (entry.Value.Name.Equals(columnName))
                    {
                        log.Info($"The user got '{columnName}' column");
                        int i = entry.Key;
                        return _boardArray[i];
                    }
                }
                log.Warn("The column name is not legal");
                throw new ArgumentException("The column name is not legal");
            }
        }

        /// <summary>
        /// Returns a column given it's identifier.
        /// </summary>
        /// <param name="columnOrdinal">Column ID</param>
        /// <returns>return a BLColumn</returns>
        public BLColumn getColumn(int columnOrdinal)
        {
            if (LegalColumnOrdinal(columnOrdinal))  //check if column ordinal is legal
                log.Info("The user got requested column");
            return _boardArray[columnOrdinal];
        }


        /// <summary>
        /// Removes a column by column position 
        /// </summary>
        /// <param name="columnOrdinal">The column position</param>
        /// <param name="email">the email of the user</param>
        public void RemoveColumn(int columnOrdinal, string email)
        {
            IsCreator(email); //check if the user is the creator of the board 
            if (_boardArray.Count() == 2)
            {
                log.Warn("Board can't delete when there is only 2 columns");
                throw new ArgumentException("Minimum amount of columns is 2, deletion failed");
            }
            if (columnOrdinal == 0)
            {
                BLColumn rightToDelete = _boardArray[columnOrdinal + 1];
                BLColumn toDelete = _boardArray[columnOrdinal];
                int spaceRight = rightToDelete.limit - rightToDelete.myList.Count();
                if (((rightToDelete.limit == -1) || (spaceRight - toDelete.myList.Count() >= 0)))
                {
                    foreach (KeyValuePair<int, BLTask> entry in toDelete.myList)
                    {
                        BLTask toMove = entry.Value;
                        toMove.ColumnID = rightToDelete.ID;
                        rightToDelete.myList.Add(toMove.ID, toMove);
                        //update all columnId for all tasks in the column that remove
                    }
                    _boardArray.Remove(columnOrdinal);
                    DalColumn dc2 = new DalColumn();
                    dc2.UpdateColumnAfterDelete(_ID, toDelete.ID, rightToDelete.ID);
                }
            }
            else if (LegalColumnOrdinal(columnOrdinal))
            {
                BLColumn toDelete = _boardArray[columnOrdinal];
                BLColumn leftToDelete = _boardArray[columnOrdinal - 1];
                int spaceLeft = leftToDelete.limit - leftToDelete.myList.Count();
                if (((leftToDelete.limit == -1) || (spaceLeft - toDelete.myList.Count() >= 0)))
                {
                    foreach (KeyValuePair<int, BLTask> entry in toDelete.myList)
                    {
                        BLTask toMove = entry.Value;
                        toMove.ColumnID = leftToDelete.ID;
                        leftToDelete.myList.Add(toMove.ID, toMove);
                        //update all columnId for all tasks in the column that remove
                    }
                    _boardArray.Remove(columnOrdinal);
                    DalColumn dc1 = new DalColumn();
                    dc1.UpdateColumnAfterDelete(_ID, toDelete.ID, leftToDelete.ID);
                }
            }
            else
            {
                log.Warn("Can't move the tasks left or right");
                throw new ArgumentException("Can't move tasks to right/left column");
            }
            DalColumn dc = new DalColumn();
            for (int position = columnOrdinal + 1; position <= _boardArray.Count(); position++)
            {
                _boardArray.Add(position - 1, _boardArray[position]);
                dc.UpdatePosition(_boardArray[position].ID, position - 1);
                _boardArray.Remove(position);
                _boardArray[position - 1].position = position - 1;
                //update the position of all columns that right to the column that removed
            }
        }

        /// <summary>
        /// Adds a new column, given it's name and a location to place it.
        /// </summary>
        /// <param name="columnOrdinal">the column position</param>
        /// <param name="Name">The name of the new column</param>
        /// <param name="email">the email of the user</param>
        /// <returns>BLColumn that Add to the board or Exception</returns>
        public BLColumn AddColumn(int columnOrdinal, string name, string email)
        {
            IsCreator(email); //check if the user is the creator of the board
            BLColumn toAdd;
            if (columnOrdinal == _boardArray.Count() || LegalColumnOrdinal(columnOrdinal))
            {
                foreach (KeyValuePair<int, BLColumn> entry in _boardArray)
                {
                    if (entry.Value.Name.Equals(name))
                    {
                        log.Error($"the column {name} allready exists in board");
                        throw new Exception($"The column {name} allready exists in board");
                    }
                }
                toAdd = new BLColumn(name, columnOrdinal, ID, UserID);
                _boardArray.Add(_boardArray.Count, toAdd);
                for (int i = _boardArray.Count() - 1; i > columnOrdinal; i--)
                {
                    MoveColumnLeft(i, email);
                }
                return toAdd;
            }
            else throw new Exception("Illegal Argument Exception");
        }

        /// <summary>
        /// /// Moves a column to the right, swapping it with the column wich is currently located there.
        /// </summary>
        /// <param name="columnOrdinal">The column position</param>
        /// <param name="email">the email of the user</param>
        /// <returns>BLColumn that Add to the board or Exception</returns>
        public BLColumn MoveColumnRight(int columnOrdinal, string email)
        {
            if (LegalColumnOrdinal(columnOrdinal) && columnOrdinal != _boardArray.Count() - 1 && IsCreator(email))
            {
                BLColumn toRight = _boardArray[columnOrdinal];
                BLColumn toLeft = _boardArray[columnOrdinal + 1];
                _boardArray.Remove(columnOrdinal);
                _boardArray.Remove(columnOrdinal + 1);
                _boardArray.Add(columnOrdinal + 1, toRight);
                _boardArray.Add(columnOrdinal, toLeft);
                toLeft.position = columnOrdinal;
                toRight.position = columnOrdinal + 1;
                DalColumn dc = new DalColumn();
                dc.UpdatePosition(toRight.ID, columnOrdinal + 1);
                dc.UpdatePosition(toLeft.ID, columnOrdinal);
                //update position of the 2 columns
                return _boardArray[columnOrdinal + 1];
            }
            else
            {
                log.Warn("Can't move right the rightmost column");
                throw new ArgumentException("Can't move right the rightmost column");
            }
        }

        /// <summary>
        /// /// Moves a column to the left, swapping it with the column wich is currently located there.
        /// </summary>
        /// <param name="columnOrdinal">The column position</param>
        /// <param name="email">the email of the user</param>
        /// <returns>BLColumn that Add to the board or Exception</returns>
        public BLColumn MoveColumnLeft(int columnOrdinal, string email)
        {
            if (LegalColumnOrdinal(columnOrdinal) && columnOrdinal != 0 && IsCreator(email))
            {
                BLColumn toLeft = _boardArray[columnOrdinal];
                BLColumn toRight = _boardArray[columnOrdinal - 1];
                _boardArray.Remove(columnOrdinal);
                _boardArray.Remove(columnOrdinal - 1);
                _boardArray.Add(columnOrdinal - 1, toLeft);
                _boardArray.Add(columnOrdinal, toRight);
                toLeft.position = columnOrdinal - 1;
                toRight.position = columnOrdinal;
                //update position of the 2 columns
                DalColumn dc = new DalColumn();
                dc.UpdatePosition(toLeft.ID, columnOrdinal - 1);
                dc.UpdatePosition(toRight.ID, columnOrdinal);
                return _boardArray[columnOrdinal - 1];
            }
            else
            {
                log.Warn("Can't move left the leftmost column");
                throw new ArgumentException("Can't move left the leftmost column");
            }
        }


        /// <summary>
        /// change the asiggnee of the task
        /// </summary>
        /// <param name="columnOrdinal">the column postion</param>
        /// <param name="taskID">the id of the task</param>
        /// <param name="emailAssignee">the email of the assignee</param>
        /// <param name="email">the email of the user</param>
        public void AssignTask(int columnOrdinal, int taskID, string emailAssignee, string email)
        {
            if (NotLastColumn(columnOrdinal))
            {
                _boardArray[columnOrdinal].AssignTask(taskID, emailAssignee, email);
            }
        }

        /// <summary>
        /// delete task from the board
        /// </summary>
        /// <param name="columnOrdinal">the column posiotion</param>
        /// <param name="taskID">the task id </param>
        /// <param name="email">the email of the user</param>
        public void DeleteTask(int columnOrdinal, int taskID, string email)
        {
            if (NotLastColumn(columnOrdinal))
            {
                _boardArray[columnOrdinal].DeleteTask(taskID, email);
            }

        }

        /// <summary>
        /// change the name of the column
        /// </summary>
        /// <param name="email">the email of the user</param>
        /// <param name="columnOrdinal">the column position</param>
        /// <param name="newName">the new name for the column</param>
        public void ChangeColumnName(string email, int columnOrdinal, string newName)
        {
            LegalColumnOrdinal(columnOrdinal);
            IsCreator(email);
            _boardArray[columnOrdinal].Name = newName;
        }

        /// <summary>
        /// Load Data
        /// </summary>
        /// <returns>dictionary of boards</returns>
        public Dictionary<int, BLBoard> LoadData()
        {
            try
            {
                Dictionary<int, BLBoard> boardList = new Dictionary<int, BLBoard>();
                DalBoard db = new DalBoard();
                List<DalBoard> listOfDalBorads = db.SelectAllBoards();
                foreach (DalBoard b in listOfDalBorads)
                {
                    BLBoard board = new BLBoard(Convert.ToInt32(b._ID), Convert.ToInt32(b._UserID), Convert.ToInt32(b._TaskCounter), b._Creator);
                    boardList.Add(board.UserID, board);
                }

                BLColumn tmp = new BLColumn();
                Dictionary<int, BLColumn> listOfColumns = tmp.LoadData();
                foreach (KeyValuePair<int, BLColumn> entry in listOfColumns)
                {
                    BLColumn c = entry.Value;
                    BLBoard board = boardList[c.UserID];
                    board._boardArray.Add(c.position, c);
                }
                return boardList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        //--------------------------------------others---------------------------------

        /// <summary>
        /// check if the Task ID is exsist
        /// </summary>
        /// <param name="taskID">the ID  of task</param>
        /// <returns>true if exsist or exception if not </returns>
        internal bool legalTaskID(int taskID)
        {
            if (taskID >= _TaskCounter)  // check if taskId is legal
            {
                log.Warn("Task " + taskID + " was never created");
                throw new ArgumentException("This task was never created");
            }
            return true;
        }

        /// <summary>
        /// check if the column ordinal is legal for the board
        /// </summary>
        /// <param name="columnOrdinal">the column position</param>
        /// <returns>true if exsist or exception if not</returns>
        internal bool LegalColumnOrdinal(int columnOrdinal)
        {
            if (columnOrdinal < 0 || columnOrdinal >= _boardArray.Count()) //check if column oridnal is legal
            {
                log.Warn("The column ordinal is not legal");
                throw new ArgumentException($"The column position is not legal, please enter a number from 0 to {boardArray.Count}");
            }
            return true;
        }

        /// <summary>
        /// return if the column ordinal is not in the last position 
        /// </summary>
        /// <param name="columnOrdinal">the column position</param>
        /// <returns>true if is correct , exception if not </returns>
        private bool NotLastColumn(int columnOrdinal)
        {
            if (columnOrdinal < 0 || columnOrdinal >= _boardArray.Count() - 1)
            {
                log.Warn("Can't update task from the rightmost column");
                throw new ArgumentException("Can't update task from the rightmost column");
            }
            return true;
        }
        /// <summary>
        /// check if the email is the creator of the board
        /// </summary>
        /// <param name="email">email to check</param>
        /// <returns>true if he is the creator else false</returns>
        private bool IsCreator(string email)
        {
            if (_Creator.Equals(email))
                return true;
            else
            {
                log.Warn($"{email} is not the creator of this board");
                throw new Exception($"{email} is not the creator of this board");
            }
        }
    }
}


