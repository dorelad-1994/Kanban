using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalObjects
{
    internal abstract class DalObject
    {
        public const string CNID = "ID";
        protected DalController _controller {get;set;}
        public long _ID { get; set; } = -1;
        protected DalObject(DalController DalTypeController)
        {
            _controller = DalTypeController;
        }
    }
}
