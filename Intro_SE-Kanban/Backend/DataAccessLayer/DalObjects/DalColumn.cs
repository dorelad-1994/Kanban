using System;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalObjects
{
    class DalColumn : DalObject
    {
        public const string CNLimit = "LimitC";
        public const string CNPosition = "Position";
        public const string CNName = "ColumnName";
        public const string CNBoardID = "BoardID";
        public const string CNUserID = "UserID";

        //--------------------------------------------
        public string _ColumnName { get; set; }
        public long _LimitC { get; set; }
        public long _Position { get; set; }
        public long _BoardID { get; set; }
        public long _UserID { get; set; }
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="BoardID"></param>
        /// <param name="UserID"></param>
        /// <param name="Limit"></param>
        /// <param name="Position"></param>
        /// <param name="ColumnName"></param>
        public DalColumn(long BoardID, long UserID, string ColumnName, long Position) : base(new DalColumnController())
        {
            _ColumnName = ColumnName;
            _Position = Position;
            _BoardID = BoardID;
            _UserID = UserID;
        }



        /// <summary>
        /// Constructor for ConvertReadFrom Database
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="BoardID"></param>
        /// <param name="UserID"></param>
        /// <param name="Limit"></param>
        /// <param name="Position"></param>
        /// <param name="ColumnName"></param>
        public DalColumn(long ID,long BoardID, long UserID, long Limit, string ColumnName, long Position) : base(new DalColumnController())
        {
            _ID = ID;
            _ColumnName = ColumnName;
            _LimitC = Limit;
            _Position = Position;
            _BoardID = BoardID;
            _UserID = UserID;
        }
        /// <summary>
        /// Empty Constructor For database Usage Only
        /// </summary>
        public DalColumn() : base(new DalColumnController())
        {
        }
        /// <summary>
        /// Update limit field in specific column on database
        /// </summary>
        /// <param name="id">Id of Column</param>
        /// <param name="limit">new imit</param>
        public void UpdatelimitColumnTask(long id, long limit)
        {
            _controller.Update(id, CNLimit, limit);
        }
        /// <summary>
        /// Inserts a new Column in database
        /// </summary>
        /// <returns>ID of Column in database</returns>
        public int Insert()
        {
            return _controller.Insert(this);
        }
        /// <summary>
        /// fetch the list of all existing Columns in database
        /// </summary>
        /// <returns>List of Columns</returns>
        public List<DalColumn> SelectAllColumns()
        {
            return ((DalColumnController)_controller).SelectAllColumns();
        }
        /// <summary>
        /// Updating in all the tasks in one to column to be in the other column and after that deleting emtpy column from database 
        /// </summary>
        /// <param name="ID">ID of one of the Column's board(doesn't matter who)</param>
        /// <param name="fromColumnID">from what id column to move all tasks</param>
        /// <param name="toColumnID">to which id column move all tasks</param>
        public void UpdateColumnAfterDelete(int ID, int fromColumnID, int toColumnID)
        {
            ((DalColumnController)_controller).UpdateColumnAfterDelete(ID, fromColumnID, toColumnID);
        }
        /// <summary>
        /// Updating the position in database
        /// </summary>
        /// <param name="columnID">column ID</param>
        /// <param name="position">position of column in board</param>
        public void UpdatePosition(long columnID, long position)
        {
            _controller.Update(columnID, CNPosition, position);
        }
        /// <summary>
        /// Updated Column Name in database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void UpdateColumnName(int id, string value)
        {
            _controller.Update(id, CNName, value);
        }
    }
}
