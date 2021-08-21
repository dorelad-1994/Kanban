using IntroSE.Kanban.Backend.DataAccessLayer.DalObjects;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalControllers
{
    class DalColumnController : DalController
    {
        //--------------------------------loger-------------------------------------------------------

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //--------------------------------------------------------------------------------------------------------
        private const string ColumnTableName = "Columns";
        /// <summary>
        /// Constructor
        /// </summary>
        public DalColumnController() : base(ColumnTableName)
        {
        }
        /// <summary>
        /// Convert Data to DalColumn object
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>DalColumn object</returns>
        protected override DalObject ConvertReaderToObject(SQLiteDataReader reader)
        {
            DalColumn column = new DalColumn(reader.GetInt64(0), reader.GetInt64(1), reader.GetInt64(2), reader.GetInt64(3), reader.GetString(4), reader.GetInt64(5));
            return column;
        }
        /// <summary>
        ///  fetch the list of all existing Columns in database
        /// </summary>
        /// <returns>list of columns</returns>
        public List<DalColumn> SelectAllColumns()
        {
            List<DalColumn> list = Select().Cast<DalColumn>().ToList();
            log.Info($"Selection of {ColumnTableName} list from database was success");
            return list;
        }
        /// <summary>
        /// Updating in all the tasks in one to column to be in the other column and after that deleting emtpy column from database 
        /// </summary>
        /// <param name="ID">ID of one of the Column's board(doesn't matter who)</param>
        /// <param name="fromColumnID">from what id column to move all tasks</param>
        /// <param name="toColumnID">to which id column move all tasks</param>
        public void UpdateColumnAfterDelete(int ID, int fromColumnID, int toColumnID)
        {
            using (var connectionToDB = new SQLiteConnection(_connectionWithDB))
            {
                int numOfAffectedTask = -1;
                int numOfAffectedColumns = -1;
                SQLiteCommand command = new SQLiteCommand(null, connectionToDB);
                try
                {
                    connectionToDB.Open();
                    command.CommandText = $"UPDATE Tasks SET {DalTask.CNColumnID} = {toColumnID} WHERE {DalTask.CNColumnID} = {fromColumnID};";
                    command.Parameters.Add(new SQLiteParameter(DalTask.CNColumnID, toColumnID));
                    numOfAffectedTask = command.ExecuteNonQuery();

                    command.CommandText = $"DELETE FROM Columns WHERE {DalColumn.CNID} = {fromColumnID};";
                    command.Parameters.Add(new SQLiteParameter("Columns", fromColumnID));
                    numOfAffectedColumns = command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    log.Error($"Failed to UpdateColumn tasks in {toColumnID} after deletion of {fromColumnID} in database");
                    throw new Exception($"Failed to UpdateColumn tasks in {toColumnID} after deletion of {fromColumnID} in database");
                }
                finally
                {
                    command.Dispose();
                    connectionToDB.Close();
                }
                if (numOfAffectedTask < 0)
                {
                    log.Info($"Non-Affected Tasks in Column <{toColumnID}> while updating columns after delete");
                }
                if (numOfAffectedColumns < 0)
                {
                    log.Info($"Non-Affected Columns on Board <{ID}>");
                }
            }
        }
        /// <summary>
        /// Inserts a new column into database
        /// </summary>
        /// <param name="user">dal user obect</param>
        /// <returns>if of the column in database</returns>
        public override int Insert(DalObject column)
        {
            DalColumn dalColumn = (DalColumn)column;
            int numOfRowsAffected = -1;
            long id;
            using (var connectionToDB = new SQLiteConnection(_connectionWithDB))
            {
                SQLiteCommand command = new SQLiteCommand(null, connectionToDB);
                try
                {
                    connectionToDB.Open();
                    command.CommandText = $"INSERT INTO {ColumnTableName} ({DalColumn.CNBoardID} ,{DalColumn.CNUserID},{DalColumn.CNName},{DalColumn.CNPosition}) " +
                        $"VALUES (@boardIDVal,@userIDVal,@columnNameVal,@positionVal);";
                    SQLiteParameter boardIDVal = new SQLiteParameter(@"boardIDVal", dalColumn._BoardID);
                    SQLiteParameter userIDVal = new SQLiteParameter(@"userIDVal", dalColumn._UserID);
                    SQLiteParameter columnNameVal = new SQLiteParameter(@"columnNameVal", dalColumn._ColumnName);
                    SQLiteParameter positionVal = new SQLiteParameter(@"positionVal", dalColumn._Position);
                    command.Parameters.Add(boardIDVal);
                    command.Parameters.Add(userIDVal);
                    command.Parameters.Add(columnNameVal);
                    command.Parameters.Add(positionVal);
                    command.Prepare();
                    numOfRowsAffected = command.ExecuteNonQuery();
                    id = connectionToDB.LastInsertRowId;

                }
                catch (Exception)
                {
                    log.Error($"Failed to Insert {dalColumn._ColumnName} of UserID <{dalColumn._UserID}> to database");
                    throw new Exception($"Failed to Insert {dalColumn._ColumnName} of UserID <{dalColumn._UserID}> to database");
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
                    log.Info($"Inserted {dalColumn._ColumnName} Column to database");
                    return (int)id;
                }
                else return -1;
            }
        }
    }
}
