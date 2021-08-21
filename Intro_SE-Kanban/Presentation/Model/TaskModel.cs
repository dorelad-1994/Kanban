using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Presentation.Model
{
    public class TaskModel : NotifiableModelObject
    {
        /// <summary>
        /// border color brush
        /// </summary>
        public SolidColorBrush BorderColor => new SolidColorBrush(Assignee.Equals(Logdin) ? Colors.Blue : Colors.Transparent);
        /// <summary>
        /// Over Due Date Tasks Backgroud color
        /// </summary>
        public SolidColorBrush OverDueTask => new SolidColorBrush(DueDate.CompareTo(DateTime.Now) < 0 ? Colors.IndianRed : GetTime(0.75,CreationTime,DueDate)? Colors.Orange : Colors.Transparent);
        /// <summary>
        /// Logded i user email
        /// </summary>
        public string Logdin { get; set; }
        /// <summary>
        /// Id of the task
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Title of the task
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Description of the task
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Date of the creation of the task
        /// </summary>
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// the task's due date time
        /// </summary>
        public DateTime DueDate { get; set; }
        /// <summary>
        /// the assigned email of the task
        /// </summary>
        public string Assignee { get; set; }
        /// <summary>
        /// the column that the task is in
        /// </summary>
        public ColumnModel Parent { get; set; }
        /// <summary>
        /// calculates the 75% time that passed after creation time and that is left for due date
        /// </summary>
        /// <param name="percentage">the precent time needed to calculate</param>
        /// <param name="creationDate">creation time of the task</param>
        /// <param name="dueDate">the task's due date time</param>
        /// <returns></returns>
        private bool GetTime(double percentage, DateTime creationDate, DateTime dueDate)
        {
            // get the difference between the dates
            // you could use TotalSeconds or a higher precision if needed
            var diff = (dueDate - creationDate).TotalMinutes;

            // multiply the result by the percentage
            // assuming a range of [0.0, 1.0]
            double minutes = diff * percentage;

            // add the minutes (or precision chosen) to the startTime
            var result = creationDate.AddMinutes(minutes);
            // and get the result
            return DateTime.Now.CompareTo(result) > 0;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="controller">backend controller</param>
        /// <param name="id">id of the task</param>
        /// <param name="emailAssignee">assigned email of the task</param>
        /// <param name="title">title of the task</param>
        /// <param name="description">description of the task</param>
        /// <param name="creationTime">creation time of the task</param>
        /// <param name="dueDate">due date of the task</param>
        /// <param name="parent">the column that the task is in</param>
        public TaskModel(BackendController controller, int id, string emailAssignee, string title, string description, DateTime creationTime, DateTime dueDate, ColumnModel parent) : base(controller)
        {
            ID = id;
            Assignee = emailAssignee;
            Title = title;
            Description = description;
            CreationTime = creationTime;
            DueDate = dueDate;
            Parent = parent;
        }
    }
}
