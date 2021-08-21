using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using IntroSE.Kanban.Backend.DataAccessLayer.DalObjects;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal abstract class DalController
    {
        //--------------------------------loger-------------------------------------------------------

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //-------------------------------Fields--------------------------------------------------------------------

        private string BASE_PATH = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "database.db"));
        protected readonly string _connectionWithDB;
        private readonly string _TableName;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tablename"></param>
        public DalController(string tablename)
        {
            _connectionWithDB = $"Data Source={BASE_PATH}; Version=3;";
            _TableName = tablename;
        }
        /// <summary>
        /// DELETE ALL DATA RECORD FROM DATABASE
        /// </summary>
        public void DeleteDataBase()
        {
            using (var connectionToDB = new SQLiteConnection(_connectionWithDB))
            {
                SQLiteCommand command = new SQLiteCommand(null, connectionToDB);
                try
                {
                    connectionToDB.Open();
                    command.CommandText = "PRAGMA foreign_keys = true;";
                    command.ExecuteNonQuery();
                    command.CommandText = $"DELETE FROM Users";
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    log.Error("Failed to DELETE All Charts from database");
                    throw new Exception("Failed to DELETE All Charts from database");
                }
                finally
                {
                    command.Dispose();
                    connectionToDB.Close();
                }
            }
        }
        /// <summary>
        /// Updates a filed where row in database is uniq and change value in field
        /// </summary>
        /// <param name="ID">PK OF OBJECT</param>
        /// <param name="ColumnName">NAME IN CHART</param>
        /// <param name="Value">VALUE TO SET</param>
        public void Update(long ID, string ColumnName, string Value)
        {
            int updated = -1;
            using (var connectionToDB = new SQLiteConnection(_connectionWithDB))
            {
                SQLiteCommand command = new SQLiteCommand(null, connectionToDB);
                try
                {
                    connectionToDB.Open();
                    command.CommandText = $"UPDATE {_TableName} SET {ColumnName} = '{Value}' WHERE {DalObject.CNID} = {ID};";
                    command.Parameters.Add(new SQLiteParameter(ColumnName, Value));
                    updated = command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    log.Error($"Failed to update table {_TableName} in Column {ColumnName} where ID is <{ID}> in runtime");
                    throw new Exception($"Failed to update table {_TableName} in Column {ColumnName} where ID is <{ID}> in runtime");
                }
                finally
                {
                    command.Dispose();
                    connectionToDB.Close();
                }
                if (updated == 0)
                {
                    log.Error($"Failed to updated <{ID}> in {_TableName} in {ColumnName} column");
                    throw new Exception($"Failed to updated <{ID}> in {_TableName} in {ColumnName} column");
                }
            }
        }
        /// <summary>
        /// Update Task - title/description/duetime
        /// </summary>
        /// <param name="taskID">ID of the task</param>
        /// <param name="boardID">ID of the board that the task is in</param>
        /// <param name="ColumnName">Name of the Column in chart on database</param>
        /// <param name="Value"> value to change/update</param>
        public void Update(long taskID, long boardID, string ColumnName, string Value)
        {
            int updated = -1;
            using (var connectionToDB = new SQLiteConnection(_connectionWithDB))
            {
                SQLiteCommand command = new SQLiteCommand(null, connectionToDB);
                try
                {
                    connectionToDB.Open();
                    command.CommandText = $"UPDATE {_TableName} SET {ColumnName} = '{Value}' WHERE ({DalObject.CNID} = {taskID} AND {DalTask.CNBoardID} = {boardID});";
                    command.Parameters.Add(new SQLiteParameter(ColumnName, Value));
                    updated = command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    log.Error($"Failed to update task<{taskID}> in table {_TableName} in {ColumnName} column of board<{boardID}> in runtime");
                    throw new Exception($"Failed to update task<{taskID}> in table {_TableName} in {ColumnName} column of board<{boardID}> in runtime");
                }
                finally
                {
                    command.Dispose();
                    connectionToDB.Close();
                }
                if (updated == 0)
                {
                    log.Error($"Didn't update task {ColumnName} with ID<{taskID}> in {_TableName} table");
                    throw new Exception($"Didn't update task {ColumnName} with ID<{taskID}> in {_TableName} table");
                }
            }
        }
        /// <summary>
        /// Update Task - maxDescription
        /// </summary>
        /// <param name="taskID">ID of the task</param>
        /// <param name="boardID">ID of the board that the task is in</param>
        /// <param name="ColumnName">Name of the Column in chart on database</param>
        /// <param name="Value"> value to change/update</param>
        public void Update(long taskID, long boardID, string ColumnName, long Value)
        {
            int updated = -1;
            using (var connectionToDB = new SQLiteConnection(_connectionWithDB))
            {
                SQLiteCommand command = new SQLiteCommand(null, connectionToDB);
                try
                {
                    connectionToDB.Open();
                    command.CommandText = $"UPDATE {_TableName} SET {ColumnName} = {Value} WHERE ({DalObject.CNID} = {taskID} AND {DalTask.CNBoardID} = {boardID});";
                    command.Parameters.Add(new SQLiteParameter(ColumnName, Value));
                    updated = command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    log.Error($"Failed to update task<{taskID}> in table {_TableName} in {ColumnName} column of board<{boardID}> in runtime");
                    throw new Exception($"Failed to update task<{taskID}> in table {_TableName} in {ColumnName} column of board<{boardID}> in runtime");
                }
                finally
                {
                    command.Dispose();
                    connectionToDB.Close();
                }
                if (updated == 0)
                {
                    log.Error($"Didn't update task {ColumnName} with ID<{taskID}> in {_TableName} table");
                    throw new Exception($"Didn't update task {ColumnName} with ID<{taskID}> in {_TableName} table");
                }
            }
        }




        /// <summary>
        /// Updates a filed where row in database is uniq and change value in field
        /// </summary>
        /// <param name="ID">PK OF OBJECT</param>
        /// <param name="ColumnName">NAME IN CHART</param>
        /// <param name="Value">VALUE TO SET</param>
        public void Update(long ID, string ColumnName, long Value)
        {
            int updated = -1;
            using (var connectionToDB = new SQLiteConnection(_connectionWithDB))
            {
                SQLiteCommand command = new SQLiteCommand(null, connectionToDB);
                try
                {
                    connectionToDB.Open();
                    command.CommandText = $"UPDATE {_TableName} SET {ColumnName} = {Value} WHERE {DalObject.CNID} = {ID};";
                    command.Parameters.Add(new SQLiteParameter(ColumnName, Value));
                    updated = command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    log.Error($"Failed to update table {_TableName} in Column {ColumnName} where ID is <{ID}> in runtime");
                    throw new Exception($"Failed to update table {_TableName} in Column {ColumnName} where ID is <{ID}> in runtime");
                }
                finally
                {
                    command.Dispose();
                    connectionToDB.Close();
                }
                if (updated == 0)
                {
                    log.Error($"Failed to updated <{ID}> in {_TableName} in {ColumnName} column");
                    throw new Exception($"Failed to updated <{ID}> in {_TableName} in {ColumnName} column");
                }
            }

        }
        /// <summary>
        /// Selects the chard from database 
        /// </summary>
        /// <returns>list of dalList object</returns>
        protected List<DalObject> Select()
        {
            List<DalObject> dalList = new List<DalObject>();
            using (var connectionToDB = new SQLiteConnection(_connectionWithDB))
            {
                SQLiteCommand command = new SQLiteCommand(null, connectionToDB);
                command.CommandText = $"SELECT * FROM {_TableName}";
                SQLiteDataReader dataReader = null;
                try
                {
                    connectionToDB.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        dalList.Add(ConvertReaderToObject(dataReader));
                    }
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }
                    command.Dispose();
                    connectionToDB.Close();
                }
            }
            return dalList;
        }
        /// <summary>
        /// Convert from data to DalObject
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>Dal from the specified type</returns>
        protected abstract DalObject ConvertReaderToObject(SQLiteDataReader reader);
        /// <summary>
        /// Inserts a row in database
        /// </summary>
        /// <param name="user"></param>
        /// <returns>return the id of the object created in database</returns>
        public abstract int Insert(DalObject DalObject);

        

        /// <summary>
        /// creates tables in database.db
        /// </summary>
        public void CreateDataBase()
        {
            if (!File.Exists(BASE_PATH))
            {
                SQLiteConnection.CreateFile(BASE_PATH);
                using (var connectionToDB = new SQLiteConnection(_connectionWithDB))
                {
                    string UserTableQuery = "CREATE TABLE \"Users\" ( \"ID\" INTEGER, \"Email\" TEXT NOT NULL UNIQUE, \"Password\" TEXT NOT NULL,	\"Nickname\" TEXT NOT NULL,\"BoardCreator\"	TEXT NOT NULL,	PRIMARY KEY(\"ID\"));";
                    string BoardTableQuery = "CREATE TABLE \"Boards\" (\"ID\"	INTEGER,\"UserID\"	INTEGER NOT NULL,\"TaskCounter\"	INTEGER NOT NULL,\"Creator\"	TEXT NOT NULL UNIQUE,PRIMARY KEY(\"ID\"),FOREIGN KEY(\"UserID\") REFERENCES \"Users\"(\"ID\") ON DELETE CASCADE);";
                    string ColumnTableQuery = "CREATE TABLE \"Columns\" (\"ID\" INTEGER,\"BoardID\" INTEGER NOT NULL,\"UserID\" INTEGER NOT NULL,\"LimitC\" INTEGER NOT NULL DEFAULT 100,\"ColumnName\" TEXT NOT NULL ,\"Position\" INTEGER NOT NULL,PRIMARY KEY(\"ID\"),FOREIGN KEY(\"BoardID\") REFERENCES \"Boards\"(\"ID\"),FOREIGN KEY(\"UserID\") REFERENCES \"Users\"(\"ID\") ON DELETE CASCADE);";
                    string TaskTableQuery = "CREATE TABLE \"Tasks\" (\"ID\"	INTEGER,\"ColumnID\"	INTEGER NOT NULL,\"BoardID\"	INTEGER,\"UserID\"	INTEGER NOT NULL,\"Title\"	TEXT NOT NULL,\"Description\"	TEXT,\"CreationDate\"	TEXT NOT NULL,\"DueDate\"	TEXT NOT NULL,\"Assignee\"	TEXT NOT NULL,PRIMARY KEY(\"ID\",\"BoardID\"),FOREIGN KEY(\"ColumnID\") REFERENCES \"Columns\"(\"ID\"),FOREIGN KEY(\"BoardID\") REFERENCES \"Boards\"(\"ID\"),FOREIGN KEY(\"UserID\") REFERENCES \"Users\"(\"ID\") ON DELETE CASCADE);";
                    SQLiteCommand command = new SQLiteCommand(null, connectionToDB);
                    try
                    {
                        connectionToDB.Open();
                        command.CommandText = UserTableQuery;
                        command.ExecuteNonQuery();
                        command.CommandText = BoardTableQuery;
                        command.ExecuteNonQuery();
                        command.CommandText = ColumnTableQuery;
                        command.ExecuteNonQuery();
                        command.CommandText = TaskTableQuery;
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        log.Error($"Creating Database Tables falied during process, error msg : {e.Message}");
                        throw new Exception($"Creating Database Tables falied during process, error msg : {e.Message}");
                    }
                    finally
                    {
                        command.Dispose();
                        connectionToDB.Close();
                        log.Info("Created Tables in databes successfully");
                    }
                }

            }

        }

    }
}