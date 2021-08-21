using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DalObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    class BLTask
    {
        //-----------------------------------------------------loger---------------------------------------------------------------

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //-----------------------------------------------------fields-------------------------------------------------------------

        private DateTime _creationDate;
        public DateTime creationDate
        {
            get { return _creationDate; }

        }

        private DateTime _dueTime;
        public DateTime dueTime
        {
            get { return _dueTime; }
        }

        private string _title;
        public string title
        {
            get { return _title; }
        }

        private string _description;
        public string description
        {
            get { return _description; }
        }

        private int _ID;
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private int _UserID;
        public int UserID
        {
            get { return _UserID; }
        }

        private int _BoardID;
        public int BoardID
        {
            get { return _BoardID; }
            set { _BoardID = value; }
        }

        private int _ColumnID;
        public int ColumnID
        {
            get { return _ColumnID; }
            set { _ColumnID = value; }
        }

        private string _AssigneeEmail;
        public string AssigneeEmail
        {
            get { return _AssigneeEmail; }
            set
            {
                _AssigneeEmail = value;
                DalTask t = new DalTask();
                t.UpdateAssignee(ID, BoardID, value);
            }
        }

        private const int TitleMax = 50;
        private const int DescriptionMax = 300;

        //----------------------------------------------------constractor---------------------------------------------------------

        /// <summary>
        /// Constractor for create BLTask with checks the arguments
        /// </summary>
        /// <param name="dueTime"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="ID"></param>
        /// <param name="UserID"></param>
        /// <param name="BoardID"></param>
        /// <param name="ColumnID"></param>
        /// <param name="assignee">the assignee of the task - deaflt creator of task</param>
        public BLTask(DateTime dueTime, string title, string description, int ID, int UserID, int BoardID, int ColumnID, string assignee)
        {
            if (DescriptionIsLegal(description))//check description
                _description = description;

            if (TitleIsLegal(title))   //check title
                _title = title;

            if (DueTimeIsLegal(dueTime))  //check due time
                _dueTime = dueTime;

            _creationDate = DateTime.Now;
            _UserID = UserID;
            _BoardID = BoardID;
            _ColumnID = ColumnID;
            _AssigneeEmail = assignee;
            DalTask dalT = new DalTask(ID, _ColumnID, _BoardID, _UserID, _title, _description, _creationDate.Ticks, _dueTime, assignee);
            dalT.Insert();
            _ID = ID;


        }
        /// <summary>
        /// Construcotr for loadData only
        /// </summary>
        /// <param name="creationDate"></param>
        /// <param name="dueTime"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="ID"></param>
        /// <param name="UserID"></param>
        /// <param name="BoardID"></param>
        /// <param name="ColumnID"></param>
        /// <param name="assignee">the assignee of the task - deaflt creator of task</param>
        public BLTask(int ID, int ColumnID, int BoardID, int UserID, string title, string description, DateTime creationDate, DateTime dueTime, string assignee)
        {
            _creationDate = creationDate;
            _dueTime = dueTime;
            _title = title;
            _description = description;
            _ID = ID;
            _UserID = UserID;
            _BoardID = BoardID;
            _ColumnID = ColumnID;
            _AssigneeEmail = assignee;
        }

        /// <summary>
        /// Empty Consturcotr for LoadData Usage Only
        /// </summary>
        public BLTask()
        {
        }


        //-----------------------------------------------methods----------------------------------

        /// <summary>
        /// check if the title is legal 
        /// </summary>
        /// <param name="title">The title of the task</param>
        /// <returns>true if is legal , false if illegal</returns>
        internal bool TitleIsLegal(String title)
        {
            if (String.IsNullOrWhiteSpace(title))
            {
                log.Warn("Title is not legal - null/empty");
                throw new ArgumentException("Empty title field , please enter a title");
            }
            if (title.Length > TitleMax)
            {
                log.Warn("Title is too long");
                throw new ArgumentException("The title is too long (max 50)");
            }
            return true;
        }

        /// <summary>
        /// check if the description is legal
        /// </summary>
        /// <param name="description">The description of the task</param>
        /// <returns>true if is legal , false if illegal</returns>
        internal bool DescriptionIsLegal(String description)
        {
            if (description != null && description.Length > DescriptionMax)
            {
                log.Warn("description is too long");
                throw new ArgumentException("The description is too long (max 300)");
            }
            return true;
        }

        /// <summary>
        /// check if the due time is legal
        /// </summary>
        /// <param name="dueTime">The due time for the task</param>
        /// <returns>true if is legal , false if illegal</returns>
        internal virtual bool DueTimeIsLegal(DateTime dueTime)
        {
            if (DateTime.Compare(dueTime, DateTime.Now) < 0)
            {
                log.Warn("Illegal due time");
                throw new ArgumentException("The due time is from the past");
            }
            else
                return true;
        }

        //----------------------------------------------LoadData----------------------------------------------------------------------

        /// <summary>
        /// load data
        /// </summary>
        /// <returns>List of BLtasks</returns>
        public List<BLTask> LoadData()
        {
            try
            {
                List<BLTask> output = new List<BLTask>();
                DalTask dalt = new DalTask();
                List<DalTask> listOfDalTasks = dalt.SelectAllTasks();
                foreach (DalTask dt in listOfDalTasks)
                {
                    BLTask t = new BLTask(Convert.ToInt32(dt._ID), Convert.ToInt32(dt._ColumnID), Convert.ToInt32(dt._BoardID), Convert.ToInt32(dt._UserID), dt._title, dt._description, dt._creationDate, dt._dueDate, dt._AssigneeEmail);
                    output.Add(t);
                }
                return output;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        //--------------------------------------------------------getters&setters---------------------------------------------------------

        /// <summary>
        /// setter
        /// </summary>
        /// <param name="dueTime">The due time for the task</param>
        internal virtual void SetDueTime(DateTime dueTime, string assignee)
        {
            if (DueTimeIsLegal(dueTime))
            {
                if (IsAssignee(assignee))
                {
                    log.Info("Task ID :'" + ID + "' - due time has been updated");
                    _dueTime = dueTime;
                    DalTask dt = new DalTask();
                    dt.UpdateDueDate(_ID, _BoardID, dueTime);
                }
            }
        }

        /// <summary>
        /// setter
        /// </summary>
        /// <param name="title">The title of the task</param>
        public void SetTitle(string title, string assignee)
        {
            if (TitleIsLegal(title) && IsAssignee(assignee))
            {
                log.Info("Task ID :'" + ID + "' - title has been updated");
                _title = title;
                DalTask dt = new DalTask();
                dt.UpdateTitle(_ID, _BoardID, title);
            }
        }

        /// <summary>
        /// setter
        /// </summary>
        /// <param name="description">The Description of the task</param>
        public void SetDescription(string description, string assignee)
        {
            if (DescriptionIsLegal(description) && IsAssignee(assignee))
            {
                log.Info("Task ID :'" + ID + "' - description has been updated");
                _description = description;
                DalTask dt = new DalTask();
                dt.UpdateDescription(_ID, _BoardID, description);
            }
        }

        /// <summary>
        /// check if the email is the assingee
        /// </summary>
        /// <param name="assignee"></param>
        /// <returns>true if the assingee is the asnnigge else false</returns>
        internal virtual bool IsAssignee(String assignee)
        {
            if (_AssigneeEmail.Equals(assignee))
                return true;
            else
            {
                log.Warn($"{assignee} is not the asignee of task '{_ID}'");
                throw new ArgumentException($"{assignee} is not the asignee of the task");
            }
        }
    }
}