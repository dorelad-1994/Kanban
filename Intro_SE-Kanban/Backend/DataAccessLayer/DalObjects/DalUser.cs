using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalObjects
{
    class DalUser : DalObject
    {
        public const string CNEmail = "Email";
        public const string CNPassword = "Password";
        public const string CNNickname = "Nickname";
        public const string CNBoardCreator = "BoardCreator";
        //-------------------------------------
        public string _BoardCreator { get; set; }
        public string _email { get; set; }
        public string _password { get; set; }
        public string _nickname { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="nickname"></param>
        public DalUser(string email, string password, string nickname, string boardCreator) : base(new DalUserController())
        {
            _email = email;
            _password = password;
            _nickname = nickname;
            _BoardCreator = boardCreator;
        }
        /// <summary>
        /// Constructor for read data only
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="nickname"></param>
        public DalUser(long ID ,string email, string password, string nickname) : base(new DalUserController())
        {
            _ID = ID;
            _email = email;
            _password = password;
            _nickname = nickname;
        }
        /// <summary>
        /// Empty Constructor for BLayer
        /// </summary>
        public DalUser() : base(new DalUserController())
        {
        }
        /// <summary>
        /// Inserts a new user in database
        /// </summary>
        /// <returns>ID of user in database</returns>
        public int Insert()
        {
            return _controller.Insert(this);
        }
        /// <summary>
        /// DELETE ALL DATABASE RECORDS
        /// </summary>
        public void DeleteDataBase()
        {
            _controller.DeleteDataBase();
        }
        /// <summary>
        ///  fetch the list of all existing Users in database
        /// </summary>
        /// <returns>List of users</returns>
        public List<DalUser> SelectAllUsers()
        {
            return ((DalUserController)_controller).SelectAllUsers();
        }
        /// <summary>
        /// create database
        /// </summary>
        public void CreateDataBase()
        {
            _controller.CreateDataBase();
        }
    }
}
