using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Presentation.ViewModel;

namespace Presentation.Model
{
    public class ColumnModel : NotifiableModelObject
    {
        //-----------------getters & setters---------------------------------------------------
        /// <summary>
        /// Collection of the Tasks in the column
        /// </summary>
        public ObservableCollection<TaskModel> Tasks { get; set; }
        /// <summary>
        /// A copy of the collection for Filter usage
        /// </summary>
        public ObservableCollection<TaskModel> NonFilteredTasks { get; set; }
        /// <summary>
        /// get/set name of column
        /// </summary>
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }
        /// <summary>
        /// get set of position of the column in the board
        /// </summary>
        public int Position { get; set; }
        /// <summary>
        /// limit of the column tasks
        /// </summary>
        private int _limit;
        public int Limit
        {
            get => _limit;
            set
            {
                _limit = value;
                RaisePropertyChanged("Limit");
            }
        }


        /// <summary>
        /// Filter the column by title/description
        /// </summary>
        /// <param name="filter">the stirng to filter by</param>
        public void Filter(string filter)
        {
            List<TaskModel> t = new List<TaskModel>(NonFilteredTasks);
            NonFilteredTasks.Where(task => !task.Description.ToLower().Contains(filter.ToLower()) && !task.Title.ToLower().Contains(filter.ToLower())).ToList().ForEach(ta => t.Remove(ta));
            Tasks.Clear();
            t.ForEach(task => Tasks.Add(task));

        }
        /// <summary>
        /// sort the column tasks by due date
        /// </summary>
        public void SortByDueDate()
        {
            List<TaskModel> t = new List<TaskModel>(Tasks);
            t.Sort(new DueDateComparer());
            Tasks.Clear();
            t.ForEach(task => Tasks.Add(task));
        }
        /// <summary>
        /// advance a task to the next column
        /// </summary>
        /// <param name="insertColumn">a specific column</param>
        /// <param name="task">a specific task to advance</param>
        public void Advance(ColumnModel insertColumn, TaskModel task)
        {
            Tasks.Remove(task);
            insertColumn.Tasks.Add(task);
            task.Parent = insertColumn;
            NonFilteredTasks.Remove(task); //
            insertColumn.NonFilteredTasks.Add(task); //
        }
        /// <summary>
        /// remove a task from the collection
        /// </summary>
        /// <param name="task">the specific task to remove</param>
        public void RemoveTask(TaskModel task)
        {
            Tasks.Remove(task);
            NonFilteredTasks.Remove(task); //
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="controller">backend controller</param>
        /// <param name="name">name of the column</param>
        /// <param name="limit">max limit tasks</param>
        /// <param name="listOfTasks">list of all tasks</param>
        public ColumnModel(BackendController controller, string name, int limit, int position) : base(controller)
        {
            Name = name;
            Limit = limit;
            this.Position = position;

        }
    }
}
