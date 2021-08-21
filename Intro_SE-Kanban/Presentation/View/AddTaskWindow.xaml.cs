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
    /// Interaction logic for AddTaskWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        private AddTaskViewModel viewModel;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user">the user that is logged in</param>
        /// <param name="columnModel">the column  to add to</param>
        public AddTaskWindow(UserModel user, ColumnModel columnModel)
        {
            InitializeComponent();
            this.DataContext = new AddTaskViewModel(user,columnModel);
            viewModel = (AddTaskViewModel)DataContext;
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
        /// event for the clock show
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tickevent(object sender, EventArgs e)
        {
            datetimeLabel.Text = DateTime.Now.ToString();
        }


        /// <summary>
        /// return click to the board
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
        /// Add task button after entering all data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            TaskModel task = viewModel.AddTask();
            if (task != null)
            {
                BoardWindow board = new BoardWindow(viewModel.User); 
                board.Show(); 
                Close();
            }
        }
    }
}
