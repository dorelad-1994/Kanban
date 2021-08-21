using IntroSE.Kanban.Backend.DataAccessLayer.DalObjects;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalControllers
{
    class DalBoardController : DalController
    {
        //--------------------------------loger-------------------------------------------------------

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //--------------------------------------------------------------------------------------------------------
        private const string BoardTableName = "Boards";
        /// <summary>
        /// Constructor
        /// </summary>
        public DalBoardController() : base(BoardTableName)
        {
        }
        /// <summary>
        /// Converts data from database into a DalBoard
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>DalBoard</returns>
        protected override DalObject ConvertReaderToObject(SQLiteDataReader reader)
        {
            DalBoard board = new DalBoard(reader.GetInt64(0), reader.GetInt64(1), reader.GetInt64(2), reader.GetString(3));
            return board;
        }
        /// <summary>
        /// Insers a new board into the database
        /// </summary>
        /// <param name="dalObject"></param>
        /// <returns>the new boards ID</returns>
        public override int Insert(DalObject dalObject)
        {
            DalBoard dalBoard = (DalBoard)dalObject;
            int numOfRowsAffected = -1;
            long id;
            using (var connectionToDB = new SQLiteConnection(_connectionWithDB))
            {
                SQLiteCommand command = new SQLiteCommand(null, connectionToDB);
                try
                {
                    connectionToDB.Open();
                    command.CommandText = $"INSERT INTO {BoardTableName} ({DalBoard.CNUserID}, {DalBoard.CNTaskCounter},{DalBoard.CNCreator}) " +
                        $"VALUES (@userIDVal,@TaskCounterVal,@creatorVal);";
                    SQLiteParameter userIDVal = new SQLiteParameter(@"userIDVal", dalBoard._UserID);
                    SQLiteParameter TaskCounterVal = new SQLiteParameter(@"TaskCounterVal", dalBoard._TaskCounter);
                    SQLiteParameter creatorVal = new SQLiteParameter(@"creatorVal", dalBoard._Creator);
                    command.Parameters.Add(userIDVal);
                    command.Parameters.Add(TaskCounterVal);
                    command.Parameters.Add(creatorVal);
                    command.Prepare();
                    numOfRowsAffected = command.ExecuteNonQuery();
                    id = connectionToDB.LastInsertRowId;
                }
                catch (Exception)
                {
                    log.Error($"Failed to Insert board of UserID<{dalBoard._UserID}> to database");
                    throw new Exception($"Failed to Insert board of UserID <{dalBoard._UserID}> to database");
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
                    log.Info($"Inserted board <{id}> of User with ID <{dalBoard._UserID}> to database");
                    return (int)id;
                }
                else return -1;
            }
        }
        /// <summary>
        /// fetch the list of all existing boards in database
        /// </summary>
        /// <returns>list of dal boards</returns>
        public List<DalBoard> SelectAllBoards()
        {
            List<DalBoard> list = Select().Cast<DalBoard>().ToList();
            log.Info($"Selection of {BoardTableName} list from database was success");
            return list;
        }
        /// <summary>
        /// get the user id by email from database for BLBoard constructor
        /// </summary>
        /// <param name="email"></param>
        /// <returns>id of email user in database</returns>
        /// <summary>
        /// get the user id by email from database for BLBoard constructor
        /// </summary>
        /// <param name="email"></param>
        /// <returns>id of email user in database</returns>
        public int getUserID(string email)
        {
            long id;
            using (var connectionToDB = new SQLiteConnection(_connectionWithDB))
            {
                SQLiteCommand command = new SQLiteCommand(null, connectionToDB);
                command.CommandText = $"SELECT {DalObject.CNID} FROM Users WHERE {DalUser.CNEmail} = '{email}';";
                SQLiteDataReader dataReader = null;
                try
                {
                    connectionToDB.Open();
                    dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        id = (long)dataReader.GetValue(0);
                    }
                    else id = -1;
                    dataReader.Close();
                }
                catch (Exception)
                {
                    log.Error($"Select of {email} ID  form database failed during proccess");
                    throw new Exception($"Select of {email} ID  form database failed during proccess");
                }
                finally
                {
                    command.Dispose();
                    connectionToDB.Close();
                }
                if (id == -1)
                {
                    log.Error($"Select of {email} ID  form database wasn't found in chart");
                    throw new Exception($"Select of {email} ID form database wasn't found in chart");
                }
                else return (int)id;
            }
        }
    }
}
