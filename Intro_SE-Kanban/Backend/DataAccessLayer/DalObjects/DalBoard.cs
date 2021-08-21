using System;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalObjects
{
    class DalBoard : DalObject
    {
        public const string CNUserID = "UserID";
        public const string CNTaskCounter = "TaskCounter";
        public const string CNCreator = "Creator";
        public long _UserID { get; set; }
        public long _TaskCounter { get; set; }
        public string _Creator { get; set; }
        /// <summary>
        /// Constructor for BLayer
        /// </summary>
        /// <param name="UsersID"></param>
        public DalBoard(long UsersID,long taskCounter, string creator) : base(new DalBoardController())
        {
            _TaskCounter = taskCounter;
            _Creator = creator;
            _UserID = UsersID;
        }
        /// <summary>
        /// Constructor for ConvertReadFrom Database
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="UsersID"></param>
        public DalBoard(long ID, long UsersID, long taskCounter,string creator) : base(new DalBoardController())
        {
            _Creator = creator;
            _ID = ID;
            _UserID = UsersID;
            _TaskCounter = taskCounter;
        }
        /// <summary>
        /// Empty Constructor for Constructor in BLBoard ONLY
        /// </summary>
        public DalBoard() : base(new DalBoardController())
        {
        }
        /// <summary>
        ///  /// Inserts a new board in database
        /// </summary>
        /// <returns><returns>ID of board in database</returns></returns>
        public int Insert()
        {
            return _controller.Insert(this);
        }
        /// <summary>
        ///  fetch the list of all existing Boards in database
        /// </summary>
        /// <returns>List of boards</returns>
        public List<DalBoard> SelectAllBoards()
        {
            return ((DalBoardController)_controller).SelectAllBoards();
        }
        /// <summary>
        /// gets the PK ID user from database - for BLBorad
        /// </summary>
        /// <param name="email">email of existing user</param>
        /// <returns>Id of email user</returns>
        public int getUserID(string email)
        {
            return ((DalBoardController)_controller).getUserID(email);
        }
        /// <summary>
        /// Updates the board's amout of tasks in the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newCounter"></param>
        public void UpdateTaskCounter(long id, long newCounter)
        {
            _controller.Update(id, CNTaskCounter, newCounter);
        }
    }
}
