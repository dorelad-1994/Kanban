using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class BoardModel : NotifiableModelObject
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="controller">backend controller</param>
        /// <param name="boardCreator">creator of the board</param>
        /// <param name="columns">list of all the columns</param>
        public BoardModel(BackendController controller, string boardCreator, List<ColumnModel> columns) : base(controller)
        {
            Creator = boardCreator;
            Columns = new ObservableCollection<ColumnModel>(columns);
        }

        /// <summary>
        /// All the loaded columns of the board
        /// </summary>
        public ObservableCollection<ColumnModel> Columns { get; set; }
        /// <summary>
        /// the creator of the board
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// swaping in the collection between two columns
        /// </summary>
        /// <param name="oldIndex">the index of the column to move</param>
        /// <param name="newIndex">the index to move to</param>
        public void Swap(int oldIndex, int newIndex)
        {
            Columns.Move(oldIndex, newIndex);
            Columns[oldIndex].Position = oldIndex;
            Columns[newIndex].Position = newIndex;
        }

        /// <summary>
        /// Adds the column to the colletion for the user to see
        /// </summary>
        /// <param name="c">the specific column to add</param>
        /// <param name="position">where to add in the collection</param>
        public void AddColumn(ColumnModel c, int position)
        {
            Columns.Insert(position, c);
            for (int i = position + 1; i < Columns.Count; i++)
            {
                Columns[i].Position = Columns[i].Position + 1;
            }
        }
        /// <summary>
        /// Remove the column from the collection board
        /// </summary>
        /// <param name="selectedColumn">the column to remove</param>
        /// <param name="columnPosition">where it is set</param>
        public void RemoveColumn(ColumnModel selectedColumn, int columnPosition)
        {
            if (columnPosition == 0)
            {
                selectedColumn.Tasks.ToList().ForEach(t => Columns[1].Tasks.Add(t));
                Columns[1].Tasks.ToList().ForEach(t => t.Parent=Columns[1]);
                selectedColumn.NonFilteredTasks.ToList().ForEach(t => Columns[1].NonFilteredTasks.Add(t));
                Columns[1].NonFilteredTasks.ToList().ForEach(t => t.Parent = Columns[1]);
            }
            else
            {
                selectedColumn.Tasks.ToList().ForEach(t => Columns[columnPosition - 1].Tasks.Add(t));
                Columns[columnPosition - 1].Tasks.ToList().ForEach(t => t.Parent = Columns[columnPosition - 1]);
                selectedColumn.NonFilteredTasks.ToList().ForEach(t => Columns[columnPosition - 1].NonFilteredTasks.Add(t));
                Columns[columnPosition - 1].NonFilteredTasks.ToList().ForEach(t => t.Parent = Columns[columnPosition - 1]);
            }
            for (int i = columnPosition + 1; i < Columns.Count; i++)
            {
                Columns[i].Position = Columns[i].Position - 1;
            }
            Columns.Remove(selectedColumn);
            Columns.ToList().ForEach(c => Console.WriteLine($"Name = {c.Name} --> Position : {c.Position}"));
        }
        /// <summary>
        /// Limit the amount of tasks in a column that is in a specific position in board
        /// </summary>
        /// <param name="position">the position of the column in board</param>
        /// <param name="limit">the new limit to set</param>
        public void LimitColumn(int position, int limit)
        {
            Columns[position].Limit = limit;
        }
        /// <summary>
        /// Updated the column name
        /// </summary>
        /// <param name="position">where the column is in the board</param>
        /// <param name="name">the new name to set</param>
        public void UpdateColumnName(int position, string name)
        {
            Columns[position].Name = name;
        }
        /// <summary>
        /// sorts all tasks in columns by due date (from small to big)
        /// </summary>
        public void SortByDueDate()
        {
            Columns.ToList().ForEach(c => c.SortByDueDate());
        }
        /// <summary>
        /// filters the columns by title/description of the tasks
        /// </summary>
        /// <param name="filter"></param>
        public void Filter(string filter)
        {
            Columns.ToList().ForEach(c => c.Filter(filter));
        }

    }
}
