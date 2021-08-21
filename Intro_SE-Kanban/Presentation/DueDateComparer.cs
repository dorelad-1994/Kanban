using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    class DueDateComparer : IComparer<TaskModel>
    {
        /// <summary>
        /// Comperator for sorting the board using linq
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(TaskModel x, TaskModel y)
        {
            return DateTime.Compare(x.DueDate, y.DueDate);     
        }
    }
}
