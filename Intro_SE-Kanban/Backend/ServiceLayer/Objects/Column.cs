using System;
using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Column
    {
        public readonly IReadOnlyCollection<Task> Tasks;
        public readonly string Name;
        public readonly int Limit;
        internal Column(IReadOnlyCollection<Task> tasks, string name, int limit)
        {
            this.Tasks = tasks;
            this.Name = name;
            this.Limit = limit;
        }


        internal Column(BLColumn column)
        {
            List<Task> a = new List<Task>();
            foreach (KeyValuePair<int, BLTask> entry in column.myList)
            {
                int id = entry.Value.ID;
                DateTime cr = entry.Value.creationDate;
                DateTime dd = entry.Value.dueTime;
                string title = entry.Value.title;
                string desc = entry.Value.description;
                string assignee = entry.Value.AssigneeEmail;
                Task task = new Task(id, cr, dd, title, desc,assignee);
                a.Add(task);
            }
            Tasks = new ReadOnlyCollection<Task>(a);
            this.Name = column.Name;
            this.Limit = column.limit; 
        }
        public void printColumn()
        {
            foreach (Task item in Tasks)
            {
                Console.WriteLine($"ID : {item.Id} , Title : {item.Title} , Description : {item.Description}, Creation : {item.CreationTime} , Due : {item.DueDate} , Assignee : {item.emailAssignee}");
            }
        }
    }
}
