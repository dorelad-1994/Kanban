using System;
using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Board
    {
        public readonly IReadOnlyCollection<string> ColumnsNames;
        public readonly string emailCreator;
        internal Board(IReadOnlyCollection<string> columnsNames, string emailCreator) 
        {
            this.ColumnsNames = columnsNames;
            this.emailCreator = emailCreator;
        }

        internal Board(BLBoard temp)
        {
            var l = temp.boardArray.OrderBy(key => key.Key);
            var dic = l.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value); 
            List<string> a = new List<string>();
            foreach (KeyValuePair<int,BLColumn> entry in dic)
            {
                a.Add(entry.Value.Name);
            }
            this.ColumnsNames = new ReadOnlyCollection<string>(a);
            this.emailCreator = temp.Creator;

        }
    }
}
