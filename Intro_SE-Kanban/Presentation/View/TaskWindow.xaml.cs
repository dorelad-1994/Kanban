using Presentation.Model;
using Presentation.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        /// <summary>
        /// Fields
        /// </summary>
        private TaskViewModel viewModel;   
        private bool Des;
        private bool Tit;
        private bool Ass;
        private bool Due;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user">loged in user</param>
        /// <param name="task">selected task</param>
        public TaskWindow(UserModel user , TaskModel task)
        {
            InitializeComponent();
            this.DataContext = new TaskViewModel(user,task);
            viewModel = (TaskViewModel)DataContext;
            Des = false;
            Tit = false;
            Ass = false;
            Due = false;
            startclock();
        }
        /// <summary>
        /// Clock
        /// </summary>
        private void startclock()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += tickevent;
            timer.Start();
        }
        /// <summary>
        /// clock event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tickevent(object sender, EventArgs e)
        {
            datetimeLabel.Text = DateTime.Now.ToString();
        }


        /// <summary>
        /// Return button to board
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Return_Click(object sender, RoutedEventArgs e)
        {
            BoardWindow board = new BoardWindow(viewModel.User);
            board.Show();
            this.Close();
        }
        /// <summary>
        /// edit title button - makes readonly false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditTitle_Click(object sender, RoutedEventArgs e)
        {
            Tit = true;
            TitleTextBox.IsReadOnly = false;
        }
        /// <summary>
        /// edit assignee button - button - makes readonly false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditAssignee_Click(object sender, RoutedEventArgs e)
        {
            Ass = true;
            AssigneeTextBox.IsReadOnly = false;
        }
        /// <summary>
        /// edit due date button  - button - makes readonly false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditDueDate_Click(object sender, RoutedEventArgs e)
        {
            Due = true;
            CalenderEdit.Visibility = CalenderEdit.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
        /// <summary>
        /// edit description button - button - makes readonly false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditDescription_Click(object sender, RoutedEventArgs e)
        {
            Des = true;
            DescriptionTextBox.IsReadOnly = false;
        }
        /// <summary>
        /// save button click after all changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Tit)
                    viewModel.UpdateTaskTitle();
                if (Des)
                    viewModel.UpdateTaskDescription();
                if (Due)
                    viewModel.UpdateTaskDueDate();
                if (Ass)
                    viewModel.assigneeTask();
                BoardWindow board = new BoardWindow(viewModel.User);
                board.Show();
                Close();
            }
            catch(Exception)
            {
                
            }
        }


    }
}
