using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalObjects
{
    class DalMember
    {
        //--------------------------------loger-------------------------------------------------------

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //---------------------------------------------------------------------------------------------
        public string _Email { get; set; }
        public string _BoardCreator { get; set; }
        //---------------------------------------------------------------------------------------------
        public const string CNEmail = "Email";
        public const string CNBoardCreator = "BoardCreator";
        private string BASE_PATH = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "database.db"));
        private readonly string _connectionWithDB;
        //---------------------------------------------------------------------------------------------
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardCreator"></param>
        public DalMember(string email, string boardCreator)
        {
            _Email = email;
            _BoardCreator = boardCreator;
            _connectionWithDB = $"Data Source={BASE_PATH}; Version=3;";
        }
        /// <summary>
        /// EMpty Consturcotr for LoadData Only
        /// </summary>
        public DalMember()
        {
            _connectionWithDB = $"Data Source={BASE_PATH}; Version=3;";
        }

        /// <summary>
        /// Fetchs a list of all the members that are not creators of the boards -> only for loaddata
        /// </summary>
        /// <returns></returns>
        public List<DalMember> SelectAllMembers()
        {
            List<DalMember> list = Select().Cast<DalMember>().ToList();
            log.Info("Selection of DalMembers list from database was success");
            return list;
        }
        /// <summary>
        /// converts the reading info from database to a DalMember
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>list of DalMember</returns>
        private DalMember ConvertReaderToDalMember(SQLiteDataReader reader)
        {
            DalMember member = new DalMember(reader.GetString(0),reader.GetString(1));
            return member;
        }
        /// <summary>
        /// Select from database query
        /// </summary>
        /// <returns>list</returns>
        private List<DalMember> Select()
        {
            List<DalMember> dalList = new List<DalMember>();
            using (var connectionToDB = new SQLiteConnection(_connectionWithDB))
            {
                SQLiteCommand command = new SQLiteCommand(null, connectionToDB);
                command.CommandText = $"SELECT {CNEmail}, {CNBoardCreator} FROM Users WHERE {CNEmail} != {CNBoardCreator};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connectionToDB.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        dalList.Add(ConvertReaderToDalMember(dataReader));
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
    }
}
