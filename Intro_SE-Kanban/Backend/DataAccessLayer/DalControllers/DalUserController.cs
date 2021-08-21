using IntroSE.Kanban.Backend.DataAccessLayer.DalObjects;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IntroSE.Kanban.Backend.DataAccessLayer.DalControllers
{
    class DalUserController : DalController
    {
        //--------------------------------loger-------------------------------------------------------

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //--------------------------------------------------------------------------------------------------------


        private const string UsersTableName = "Users";
        /// <summary>
        /// Constuctor
        /// </summary>
        public DalUserController() : base(UsersTableName)
        {

        }
        /// <summary>
        /// Inserts into database a new user
        /// </summary>
        /// <param name="user">dal user</param>
        /// <returns>The Id of the user inserted into the database, IF THE USER WASTED ADD THEN RETURN -1</returns>
        public override int Insert(DalObject user)
        {
            DalUser dalUser = (DalUser)user;
            int numOfRowsAffected = -1;
            long id;
            using (var connectionToDB = new SQLiteConnection(_connectionWithDB))
            {
                SQLiteCommand command = new SQLiteCommand(null, connectionToDB);
                try
                {
                    connectionToDB.Open();
                    command.CommandText = $"INSERT INTO {UsersTableName} ({DalUser.CNEmail} ,{DalUser.CNPassword},{DalUser.CNNickname},{DalUser.CNBoardCreator}) " +
                        $"VALUES (@emailVal,@passwordVal,@nicknameVal,@boardCreatorVal);";
                    SQLiteParameter emailVal = new SQLiteParameter(@"emailVal", dalUser._email);
                    SQLiteParameter passwordVal = new SQLiteParameter(@"passwordVal", dalUser._password);
                    SQLiteParameter nicknameVal = new SQLiteParameter(@"nicknameVal", dalUser._nickname);
                    SQLiteParameter boardCreatorVal = new SQLiteParameter(@"boardCreatorVal", dalUser._BoardCreator);

                    command.Parameters.Add(emailVal);
                    command.Parameters.Add(passwordVal);
                    command.Parameters.Add(nicknameVal);
                    command.Parameters.Add(boardCreatorVal);
                    command.Prepare();
                    numOfRowsAffected = command.ExecuteNonQuery();
                    id = connectionToDB.LastInsertRowId;

                }
                catch (Exception)
                {
                    log.Error($"Failed to Insert {dalUser._email} to database");
                    throw new Exception($"Failed to Insert {dalUser._email} to database");
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
                    log.Info($"Inserted {dalUser._email} to database");
                    return (int)id;
                }
                else return -1;
            }
        }
        /// <summary>
        /// contverts the data from database into DalObject
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override DalObject ConvertReaderToObject(SQLiteDataReader reader)
        {
            DalUser user = new DalUser(reader.GetInt64(0),reader.GetString(1), reader.GetString(2), reader.GetString(3));
            return user;

        }
        /// <summary>
        ///  fetch the list of all existing Users in database
        /// </summary>
        /// <returns>List of users</returns>
        public List<DalUser> SelectAllUsers()
        {
            List<DalUser> list = Select().Cast<DalUser>().ToList();
            log.Info($"Selection of {UsersTableName} list from database was success");
            return list;
        }
    }
}
