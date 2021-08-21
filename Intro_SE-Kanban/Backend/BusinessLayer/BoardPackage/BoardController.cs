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
    class BoardController
    {
        //--------------------------------loger-------------------------------------------------------

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //-------------------------------fields-------------------------------------------------------

        private Dictionary<String, BLBoard> _boardList;
        public Dictionary<String, BLBoard> boardList
        {
            get { return _boardList; }
            set { _boardList = value; }
        }

        //-----------------------------constractor----------------------------------------------------

        /// <summary>
        /// simple public constractor
        /// </summary>
        public BoardController()
        {
            _boardList = new Dictionary<String, BLBoard>();
        }

        //-------------------------------methods------------------------------------------------------


        /// <summary>
        ///  This method add task to the user's kanban
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date of the new task</param>
        /// <param name="isLogdin">Email of the user that logdin now</param>
        public int addTask(String email, String title, String description, DateTime dueDate, String isLogdin)
        {
            email = toLower(email);  // changing the letters of the email to lower.
            IsLogdin(isLogdin, email);  //the user that login can change only his board
            BLBoard myBoard = _boardList[email];
            int TaskID = myBoard.addTask(title, description, dueDate, email);  // constractor of BLTask check input
            log.Info("Task " + TaskID + " has been added by " + email);
            return TaskID;
        }

        /// <summary>
        /// This method update task title
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column position.</param>
        /// <param name="taskId">the task Id number</param>
        /// <param name="title">New title for the task</param>
        /// <param name="isLogdin">Email of the user that logdin now</param>
        public void UpdateTaskTitle(String email, int columnOrdinal, int taskID, String title, String isLogdin)
        {
            email = toLower(email);  // changing the letters of the email to lower.
            IsLogdin(isLogdin, email);  //the user that login can change only his board
            BLBoard myBoard = _boardList[email];
            myBoard.updateTaskTitle(columnOrdinal, taskID, title, email);  // BLColumn check if column ordinal is legal , BLTask check if title is legal
        }

        /// <summary>
        /// This method update task due time
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column position.</param>
        /// <param name="taskId">the task Id number</param>
        /// <param name="dueDate">New due date for the task</param>
        /// <param name="isLogdin">Email of the user that logdin now</param>
        public void updateTaskDueTime(String email, int columnOrdinal, int taskID, DateTime dueDate, String isLogdin)
        {
            email = toLower(email);  // changing the letters of the email to lower.
            IsLogdin(isLogdin, email);  //the user that login can change only his board
            BLBoard myBoard = _boardList[email];
            myBoard.updateTaskDueTime(columnOrdinal, taskID, dueDate, email); // BLColumn check if column ordinal is legal , BLTask check if dueDate is legal
        }

        /// <summary>
        /// This method update task description
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column position.</param>
        /// <param name="taskId">the task Id number</param>
        /// <param name="description">New description for the task</param>
        /// <param name="isLogdin">Email of the user that logdin now</param>
        public void updateTaskDescription(String email, int columnOrdinal, int taskID, String description, String isLogdin)
        {
            email = toLower(email);  // changing the letters of the email to lower.
            IsLogdin(isLogdin, email);  //the user that login can change only his board
            BLBoard myboard = _boardList[email];
            myboard.updateTaskDescription(columnOrdinal, taskID, description, email); // BLColumn check if column ordinal is legal , BLTask check if description is legal
        }

        /// <summary>
        /// Advance a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column position.</param>
        /// <param name="taskId">the task Id number</param>
        /// <param name="isLogdin">Email of the user that logdin now</param>
        public void advanceTask(String email, int columnOrdinal, int taskId, String isLogdin)
        {
            email = toLower(email);  // changing the letters of the email to lower.
            IsLogdin(isLogdin, email);  //the user that login can change only his board
            BLBoard myboard = _boardList[email];
            myboard.advanceTask(columnOrdinal, taskId, email); //BLBoard check if column ordinal is legal and if taskid is exsist
        }

        /// <summary>
        /// Limit the number of tasks in a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="columnOrdinal">The column position.</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <param name="isLogdin">Email of the user that logdin now</param>
        public void LimitColumnTasks(string email, int columnOrdinal, int limit, String isLogdin)
        {
            email = toLower(email);  // changing the letters of the email to lower.
            IsLogdin(isLogdin, email);  //the user that login can change only his board
            BLBoard myboard = _boardList[email];
            myboard.limitColumnTasks(columnOrdinal, limit, email); // BLBoard check if column ordinal is legalm, BLColumn check if limit is legal.
            log.Info("limited max tasks of column '" + columnOrdinal + "' to '" + limit + "' tasks");
        }

        /// <summary>
        /// Returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnName">Column name</param>
        /// <param name="isLogdin">Email of the user that logdin now</param>
        /// <returns>return a BLCloumn</returns>
        public BLColumn getColumn(String email, String columnName, String isLogdin)
        {
            email = toLower(email);  // changing the letters of the email to lower.
            IsLogdin(isLogdin, email);  //the user that login can change only his board
            BLBoard myboard = _boardList[email];
            return myboard.getColumn(columnName);  //BLBoard check if column name is legal
        }

        /// <summary>
        /// Returns a column given it's identifier.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Column position</param>
        /// <param name="isLogdin">Email of the user that logdin now</param>
        /// <returns>return a BLColumn</returns>
        public BLColumn getColumn(String email, int columnOrdinal, String isLogdin)
        {
            email = toLower(email);  // changing the letters of the email to lower.
            IsLogdin(isLogdin, email);  //the user that login can change only his board
            BLBoard myboard = _boardList[email];
            return myboard.getColumn(columnOrdinal); // BLBoard check if column ordinal is legal
        }

        /// <summary>
        /// Returns the board of a user. The user must be logged in
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="isLogdin">Email of the user that logdin now</param>
        /// <returns>return a BLBoard</returns>
        public BLBoard getBoard(String email, String isLogdin)
        {
            email = toLower(email);  // changing the letters of the email to lower.
            IsLogdin(isLogdin, email);  //the user that login can change only his board
            log.Info(email + " got his board");
            return _boardList[email];
        }

        /// <summary>
        /// Removes a column by column position 
        /// </summary>
        /// <param name="email">the email of the user</param>
        /// <param name="columnOrdinal">The column position</param>
        /// <param name="isLogdin">Email of the user that logdin now</param>
        public void RemoveColumn(string email, int columnOrdinal, string isLogdin)
        {
            email = toLower(email);  // changing the letters of the email to lower.
            IsLogdin(isLogdin, email);  //the user that login can change only his board
            _boardList[email].RemoveColumn(columnOrdinal, email); //BLboard check if the column oridnal is legal and email is the Creator
            log.Info($"{email} Remove the Column - '{columnOrdinal}' in his board ");
        }

        /// <summary>
        /// Adds a new column, given it's name and a location to place it.
        /// </summary>
        /// <param name="email">the email of the user</param>
        /// <param name="columnOrdinal">the column position</param>
        /// <param name="Name">The name of the new column</param>
        /// <param name="isLogdin">email of the user that logdin now</param>
        /// <returns>BLColumn that Add to the board or Exception</returns>
        public BLColumn AddColumn(string email, int columnOrdinal, string Name, string isLogdin)
        {
            email = toLower(email);  // changing the letters of the email to lower.
            IsLogdin(isLogdin, email);  //the user that login can change only his board
            BLColumn output = _boardList[email].AddColumn(columnOrdinal, Name, email);   //BLboard check if the column oridnal is legal
            log.Info($"{email} add A new Column - '{Name}' to his board ");
            return output;
        }

        /// <summary>
        /// /// Moves a column to the right, swapping it with the column wich is currently located there.
        /// </summary>
        /// <param name="email">the email of the user</param>
        /// <param name="columnOrdinal">The column position</param>
        /// <param name="isLogdin">Email of the user that logdin now</param>
        /// <returns>BLColumn that Add to the board or Exception</returns>
        public BLColumn MoveColumnRight(string email, int columnOrdinal, string isLogdin)
        {
            email = toLower(email);  // changing the letters of the email to lower.
            IsLogdin(isLogdin, email);  //the user that login can change only his board
            BLColumn output = _boardList[email].MoveColumnRight(columnOrdinal, email);  //BLboard check if the column oridnal is legal
            log.Info($"{email} moved right the Column - '{columnOrdinal}' in his board ");
            return output;
        }

        /// <summary>
        /// /// Moves a column to the left, swapping it with the column wich is currently located there.
        /// </summary>
        /// <param name="email">the email of the user</param>
        /// <param name="columnOrdinal">The column position</param>
        /// <param name="isLogdin">Email of the user that logdin now</param>
        /// <returns>BLColumn that Add to the board or Exception</returns>
        public BLColumn MoveColumnLeft(string email, int columnOrdinal, string isLogdin)
        {
            email = toLower(email);  // changing the letters of the email to lower.
            IsLogdin(isLogdin, email);  //the user that login can change only his board
            BLColumn output = _boardList[email].MoveColumnLeft(columnOrdinal, email);  //BLboard check if the column oridnal is legal
            log.Info($"{email} moved left the Column - '{columnOrdinal}' in his board ");
            return output;
        }

        /// <summary>
        ///  change the assingee of the task
        /// </summary>
        /// <param name="email">the email of the user</param>
        /// <param name="columnOrdinal">the column position</param>
        /// <param name="isLogdin">email of the user that logdin now</param>
        /// <param name="taskID">the id number of the task</param>
        /// <param name="emailAssignee">the email of the new assingee</param>
        public void AssignTask(string email, int columnOrdinal, int taskID, string emailAssignee, string isLogedIn)
        {
            email = toLower(email);  // changing the letters of the email to lower.
            IsLogdin(isLogedIn, email);  //the user that login can change only his board
            if (_boardList.ContainsKey(emailAssignee) && _boardList[email] == _boardList[emailAssignee])  // check if the emailAssignee is member in board's email
            {
                _boardList[email].AssignTask(columnOrdinal, taskID, emailAssignee, email);   //BLboard check if the column oridnal is legal
                log.Info($"{email} assigned task - '{taskID}' to {emailAssignee} ");
            }
            else
            {
                log.Error(emailAssignee + "is not member in this board");
                throw new ArgumentException(emailAssignee + "is not member in this board");
            }
        }


        /// <summary>
        ///  delete a task from the board
        /// </summary>
        /// <param name="email">the email of the user</param>
        /// <param name="columnOrdinal">the column position</param>
        /// <param name="isLogdin">email of the user that logdin now</param>
        /// <param name="taskID">the id number of the task</param>
        public void DeleteTask(string email, int columnOrdinal, int taskID, string isLogedIn)
        {
            email = toLower(email);  // changing the letters of the email to lower.
            IsLogdin(isLogedIn, email);  //the user that login can change only his board
            _boardList[email].DeleteTask(columnOrdinal, taskID, email);   //BLboard check if the column oridnal is legal
            log.Info($"{email} delete task - '{taskID}'");
        }


        /// <summary>
        /// change the column name in the board
        /// </summary>
        /// <param name="email">the email of the user</param>
        /// <param name="columnOrdinal">the column position</param>
        /// <param name="isLogdin">email of the user that logdin now</param>
        /// <param name="newName">the new name for the column</param>

        public void ChangeColumnName(string email, int columnOrdinal, string newName, string isLogedIn)
        {
            email = toLower(email);  // changing the letters of the email to lower.
            IsLogdin(isLogedIn, email);  //the user that login can change only his board
            _boardList[email].ChangeColumnName(email, columnOrdinal, newName);   //BLboard check if the column oridnal is legal and if email is the creator
            log.Info($"{email} change column '{columnOrdinal}' name to '{newName}'");
        }


        //---------------------------------Data--------------------------------------------------

        /// <summary>
        /// Deleting All Boards Data
        /// </summary>
        public void DeleteData()
        {
            _boardList = new Dictionary<string, BLBoard>();
        }

        /// <summary>
        /// Loading data from database
        /// </summary>
        public void LoadData()
        {
            try
            {
                BLBoard tmp = new BLBoard();
                Dictionary<int, BLBoard> filledBoards = tmp.LoadData();
                Dictionary<string, BLBoard> listOfBoards = new Dictionary<string, BLBoard>();
                foreach (KeyValuePair<int, BLBoard> entry in filledBoards)
                {
                    BLBoard b = entry.Value;
                    listOfBoards.Add(b.Creator, b);
                }
                boardList = listOfBoards;
                DalMember member = new DalMember();
                List<DalMember> listOfMembers = member.SelectAllMembers();
                foreach (DalMember m in listOfMembers)
                {
                    BLBoard board = boardList[m._BoardCreator];
                    boardList.Add(m._Email, board);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //-------------------------------------other--------------------------------------------

        /// <summary>
        /// check if the email is the current user that logdin
        /// </summary>
        /// <param name="isLogdin"></param>
        /// <param name="email"></param>
        private void IsLogdin(string isLogdin, string email)
        {
            if (!(isLogdin.Equals(email)))     //the user that login can change only his board
            {
                log.Error("Tried to change user's board that not logdin");
                throw new ArgumentException("This user is not logdin");
            }
        }


        /// <summary>
        /// check if the email is null and if not change to lower letters
        /// </summary>
        /// <param name="email">The board's email that the action apply on him </param>
        /// <returns>if not null return the email in lower letters. if null return Exception</returns>
        private String toLower(String email)
        {
            if (email == null)
            {
                log.Error("The email argument is null");
                throw new ArgumentNullException("The email argument is null");
            }
            else
                return email.ToLower();
        }


        /// <summary>
        /// create a board for a user that register.
        /// </summary>
        /// <param name="email">The email of the user that register</param>
        public void addUser(String email)
        {
            email = toLower(email);  // changing the letters of the email to lower.
            BLBoard NewUserBoard = new BLBoard(email);
            _boardList.Add(email, NewUserBoard); //add board and 3 deafult columns
        }

        /// <summary>
        /// add a new member to exsist board
        /// </summary>
        /// <param name="email"></param>
        /// <param name="creator"></param>
        public void addMember(string email, string creator)
        {
            BLBoard board = _boardList[creator];
            _boardList.Add(email, board);
        }

        /// <summary>
        /// return the name of the column by column ordinal.
        /// </summary>
        /// <param name="columnOrdinal">Column ID</param>
        /// <param name="email">the email of the user</param>
        /// <returns>String with the name of the column or Exception</returns>
        public String intToString(int columnOrdinal, string email)
        {
            if (columnOrdinal >= 0 && columnOrdinal <= _boardList[email].boardArray.Count())
                return _boardList[email].boardArray[columnOrdinal].Name;
            else
            {
                log.Warn("column ordinal is illegal");
                throw new ArgumentException("column ordinal is illegal");
            }
        }


    }
}


