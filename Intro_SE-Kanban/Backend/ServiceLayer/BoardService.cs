using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class BoardService
    {
        //---------------------------------------------fields----------------------------------------------------

        private BoardController _boardController;

        //---------------------------------------------constractor------------------------------------------------

        /// <summary>
        /// simple constractor
        /// </summary>
        public BoardService()
        {
            this._boardController = new BoardController();
        }

        //---------------------------------------------methods----------------------------------------------------

        /// <summary>
        /// Create board for a user that register.
        /// </summary>
        /// <param name="email">The email of the user that register</param>
        /// <returns>A response obcject - contain a error message in case of an error</returns>
        public Response AddUserBoard(string email)
        {
            try
            {
                _boardController.addUser(email);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Add a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date of the new task</param>
        /// <param name="isLogdin">The email of the user that logdin now</param>
        /// <returns>A response object with a value set to the Task, instead the response should contain a error message in case of an error</returns>
        public Response<Task> AddTask(String email, String title, String description, DateTime dueDate, String isLogdin)
        {
            int temp;
            try
            {
                temp = _boardController.addTask(email, title, description, dueDate, isLogdin);
                return new Response<Task>(new Task(temp, DateTime.Now, dueDate, title, description, email));
            }
            catch (Exception e)
            {
                return new Response<Task>(e.Message);
            }

        }

        /// <summary>
        /// Update task title
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <param name="isLogdin">The email of the user that logdin now</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskTitle(String email, int columnOrdinal, int taskId, String title, String isLogdin)
        {
            try
            {
                _boardController.UpdateTaskTitle(email, columnOrdinal, taskId, title, isLogdin);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Update the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <param name="isLogdin">The email of the user that logdin now , if no one - null</param>
        /// returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDueDate(String email, int columnOrdinal, int taskId, DateTime dueDate, String isLogdin)
        {
            try
            {
                _boardController.updateTaskDueTime(email, columnOrdinal, taskId, dueDate, isLogdin);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Update the description of a task
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <param name="isLogdin">The email of the user that logdin now</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDescription(String email, int columnOrdinal, int taskID, String description, String isLogdin)
        {
            try
            {
                _boardController.updateTaskDescription(email, columnOrdinal, taskID, description, isLogdin);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Advance a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="isLogdin">The email of the user that logdin now</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AdvanceTask(String email, int columnOrdinal, int taskID, String isLogdin)
        {
            try
            {
                _boardController.advanceTask(email, columnOrdinal, taskID, isLogdin);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Limit the number of tasks in a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <param name="isLogdin">The email of the user that logdin now</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response LimitColumnTasks(String email, int columnOrdinal, int limit, String isLogdin)
        {
            try
            {
                _boardController.LimitColumnTasks(email, columnOrdinal, limit, isLogdin);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnName">Column name</param>
        /// <param name="isLogdin">The email of the user that logdin now</param>
        /// <returns>A response object with a value set to the Column.The response contain a error message in case of an error</returns>
        public Response<Column> GetColumn(String email, String columnName, String isLogedin)
        {
            try
            {
                BLColumn temp = _boardController.getColumn(email, columnName, isLogedin);
                Column column = new Column(temp);
                return new Response<Column>(column);
            }
            catch (Exception e)
            {
                return new Response<Column>(e.Message);
            }
        }

        /// <summary>
        /// Returns a column given it's identifier.
        /// The first column is identified by 0, the ID increases by 1 for each column
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Column ID</param>
        /// <param name="isLogdin">The email of the user that logdin now</param>
        /// <returns>A response object with a value set to the Column, The response contain a error message in case of an error</returns>
        public Response<Column> GetColumn(String email, int columnOrdinal, String isLogdin)
        {
            try
            {
                String columnName = _boardController.intToString(columnOrdinal, email);
                return GetColumn(email, columnName, isLogdin);
            }
            catch (Exception e)
            {
                return new Response<Column>(e.Message);
            }



        }

        /// <summary>
        /// Returns the board of a user. The user must be logged in
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="isLogdin">The email of the user that logdin now</param>
        /// <returns>A response object with a value set to the board, instead the response contain a error message in case of an error</returns>
        public Response<Board> GetBoard(String email, String isLogdin)
        {
            try
            {
                BLBoard temp = _boardController.getBoard(email, isLogdin);
                Board board = new Board(temp);
                return new Response<Board>(board);
            }
            catch (Exception e)
            {
                return new Response<Board>(e.Message);
            }
        }
        /// <summary>
        /// Loading data of all boards from database
        /// </summary>
        /// <returns>response </returns>
        public Response LoadData()
        {
            try
            {
                _boardController.LoadData();
                return new Response();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        /// <summary>
        /// Remove a column 
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="isLogdin">The email of the user that logdin now</param>
        /// <param name="columnOrdinal">the column ordinal</param>
        /// <returns>Response message</returns>
        public Response RemoveColumn(String email, int columnOrdinal, String isLogdIn)
        {
            try
            {
                _boardController.RemoveColumn(email, columnOrdinal, isLogdIn);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Add a column 
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="isLogdin">The email of the user that logdin now</param>
        /// <param name="name">The name of the new column</param>
        /// <param name="columnOrdinal">the column ordinal</param>
        /// <returns>Response message</returns>
        public Response<Column> AddColumn(String email, int columnOrdinal, String name, String isLogedIn)
        {
            try
            {
                BLColumn temp = _boardController.AddColumn(email, columnOrdinal, name, isLogedIn);
                List<Task> a = new List<Task>();

                IReadOnlyCollection<Task> tempList = new ReadOnlyCollection<Task>(a);
                Column column = new Column(tempList, name, temp.limit);
                return new Response<Column>(column);
            }
            catch (Exception e)
            {
                return new Response<Column>(e.Message);
            }

        }

        /// <summary>
        /// Move column to the right 
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="isLogdin">The email of the user that logdin now</param
        /// <param name="columnOrdinal">the column ordinal</param>
        /// <returns>Return a response column </returns>
        public Response<Column> MoveColumnRight(String email, int columnOrdinal, String isLogedIn)
        {
            try
            {
                String columnName = _boardController.intToString(columnOrdinal, email);
                BLColumn temp = _boardController.MoveColumnRight(email, columnOrdinal, isLogedIn);
                Column column = new Column(temp);
                return new Response<Column>(column);
            }
            catch (Exception e)
            {
                return new Response<Column>(e.Message);
            }

        }

        /// <summary>
        /// Move column to the left 
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="isLogdin">The email of the user that logdin now</param
        /// <param name="columnOrdinal">the column ordinal</param>
        /// <returns>Return a response column </returns>
        public Response<Column> MoveColumnLeft(String email, int columnOrdinal, String isLogedIn)
        {
            try
            {
                String columnName = _boardController.intToString(columnOrdinal, email);
                BLColumn temp = _boardController.MoveColumnLeft(email, columnOrdinal, isLogedIn);
                Column column = new Column(temp);
                return new Response<Column>(column);
            }
            catch (Exception e)
            {
                return new Response<Column>(e.Message);
            }

        }

        /// <summary>
        /// Delete all the data 
        /// </summary>
        /// <returns>Return response message</returns>
        public Response DeleteData()
        {
            try
            {
                _boardController.DeleteData();
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>        
		/// <param name="emailAssignee">Email of the user to assign to task to</param>
        /// <param name="isLogedIn">Email of the user that logdin now</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee, String isLogedIn)
        {
            try
            {
                _boardController.AssignTask(email, columnOrdinal, taskId, emailAssignee, isLogedIn);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Delete a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>        		
        /// <param name="isLogedIn">Email of the user that logdin now</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response DeleteTask(string email, int columnOrdinal, int taskId, string isLogedIn)
        {
            try
            {
                _boardController.DeleteTask(email, columnOrdinal, taskId, isLogedIn);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// add the new user to board of the creator
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="creator">the email of the creator of the board</param>
        /// <returns></returns>
        public Response addMember(string email, string creator)
        {
            try
            {
                _boardController.addMember(email, creator);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }


        /// <summary>
        /// change the colulmn name
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="newName">the new name for the column</param>
        /// <param name="isLogedIn">Email of the user that logdin now</param>
        /// <returns></returns>
        public Response ChangeColumnName(string email, int columnOrdinal, string newName, string isLogedIn)
        {
            try
            {
                _boardController.ChangeColumnName(email, columnOrdinal, newName, isLogedIn);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }
    }
}
