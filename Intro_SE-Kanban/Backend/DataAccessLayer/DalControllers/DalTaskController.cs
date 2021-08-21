using IntroSE.Kanban.Backend.DataAccessLayer.DalObjects;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalControllers
{
    class DalTaskController : DalController
    {
        //--------------------------------loger-------------------------------------------------------

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //--------------------------------------------------------------------------------------------------------

        private const string TaskTableName = "Tasks";
        /// <summary>
        /// Constructor
        /// </summary>
        public DalTaskController() : base(TaskTableName)
        {
        }
        /// <summary>
        /// Convert Data to DalColumn object
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>DalTask object</returns>
        protected override DalObject ConvertReaderToObject(SQLiteDataReader reader)
        {
            DalTask task = new DalTask(reader.GetInt64(0), reader.GetInt64(1), reader.GetInt64(2), reader.GetInt64(3), reader.GetString(4), reader.IsDBNull(5) ? null : reader.GetString(5), DateTime.Parse(reader.GetString(6)), DateTime.Parse(reader.GetString(7)),reader.GetString(8));
            return task;
        }
        /// <summary>
        /// Inserts a new task into database
        /// </summary>
        /// <param name="DalObject">dal task object</param>
        /// <returns>ID of task in chart</returns>
        public override int Insert(DalObject task)
        {
            DalTask dalTask = (DalTask)task;
            int numOfRowsAffected = -1;
            long id;
            using (var connectionToDB = new SQLiteConnection(_connectionWithDB))
            {
                SQLiteCommand command = new SQLiteCommand(null, connectionToDB);
                try
                {
                    connectionToDB.Open();
                    command.CommandText = $"INSERT INTO {TaskTableName} ({DalObject.CNID} ,{DalTask.CNColumnID},{DalTask.CNBoardID},{DalTask.CNUserID},{DalTask.CNTitle},{DalTask.CNDescription},{DalTask.CNCreationDate},{DalTask.CNDueDate},{DalTask.CNAssignee}) " +
                        $"VALUES (@IDVal,@columnIDVal,@boardIDVal,@userIDVal,@titleVal,@descriptionVal,@creationDateVal,@dueDateVal,@assigneeVal);";
                    SQLiteParameter IDVal = new SQLiteParameter(@"IDVal", dalTask._ID);
                    SQLiteParameter columnIDVal = new SQLiteParameter(@"columnIDVal", dalTask._ColumnID);
                    SQLiteParameter boardIDVal = new SQLiteParameter(@"boardIDVal", dalTask._BoardID);
                    SQLiteParameter userIDVal = new SQLiteParameter(@"userIDVal", dalTask._UserID);
                    SQLiteParameter titleVal = new SQLiteParameter(@"titleVal", dalTask._title);
                    SQLiteParameter descriptionVal = new SQLiteParameter(@"descriptionVal", dalTask._description);
                    SQLiteParameter creationDateVal = new SQLiteParameter(@"creationDateVal", dalTask._creationDate.ToString());
                    SQLiteParameter dueDateVal = new SQLiteParameter(@"dueDateVal", dalTask._dueDate.ToString());
                    SQLiteParameter assigneeVal = new SQLiteParameter(@"assigneeVal", dalTask._AssigneeEmail);
                    command.Parameters.Add(IDVal);
                    command.Parameters.Add(columnIDVal);
                    command.Parameters.Add(boardIDVal);
                    command.Parameters.Add(userIDVal);
                    command.Parameters.Add(titleVal);
                    command.Parameters.Add(descriptionVal);
                    command.Parameters.Add(creationDateVal);
                    command.Parameters.Add(dueDateVal);
                    command.Parameters.Add(assigneeVal);
                    command.Prepare();
                    numOfRowsAffected = command.ExecuteNonQuery();
                    id = connectionToDB.LastInsertRowId;

                }
                catch (Exception)
                {
                    log.Error($"Failed to Insert task<{dalTask._ID}> of UserID <{dalTask._UserID}> to database");
                    throw new Exception($"Failed to Insert task<{dalTask._ID}> of UserID <{dalTask._UserID}> to database");
                }
                finally
                {
                    command.Dispose();
                    connectionToDB.Close();
                }
                if (id == 0)
                {
                    log.Error("Id LastInserRowID Failure");
                    throw new Exception("Id LastInserRowID Failure");
                }
                if (numOfRowsAffected > 0)
                {
                    log.Info($"Inserted task<{dalTask._ID}> into Column<{dalTask._ColumnID}> to database");  
                    return (int)id;
                }
                else return -1;
            }
        }
        /// <summary>
        /// fetch the list of all existing tasks in database
        /// </summary>
        /// <returns>list of tasks</returns>
        public List<DalTask> SelectAllTasks()
        {
            List<DalTask> list = Select().Cast<DalTask>().ToList();
            log.Info($"Selection of {TaskTableName} list from database was success");
            return list;
        }
        /// <summary>
        /// Delete Task Row in database
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="BoardId"></param>
        public void DeleteTask(long taskId, long BoardId)
        {
            using (var connectionToDB = new SQLiteConnection(_connectionWithDB))
            {
                SQLiteCommand command = new SQLiteCommand(null, connectionToDB);
                try
                {
                    connectionToDB.Open();
                    command.CommandText = $"DELETE From {TaskTableName} WHERE ({DalTask.CNID} = {taskId} AND {DalTask.CNBoardID} = {BoardId})";
                    command.ExecuteNonQuery();

                }
                catch (Exception)
                {
                    log.Error($"Failed to DELETE task<{taskId}> in board<{BoardId}> from database");
                    throw new Exception($"Failed to DELETE task<{taskId}> in board<{BoardId}> from database");
                }
                finally
                {
                    command.Dispose();
                    connectionToDB.Close();
                }
            }
        }
    }
}
