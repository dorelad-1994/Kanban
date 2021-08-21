using System;
using System.Collections.Generic;
using System.Data;
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
using Presentation.Model;
using Presentation.ViewModel;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for BoardWindow.xaml
    /// </summary>
    public partial class BoardWindow : Window
    {
        /// <summary>
        /// field view model
        /// </summary>
        private BoardViewModel viewModel;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userModel">the loged in user</param>
        public BoardWindow(UserModel userModel)
        {

            InitializeComponent();
            this.DataContext = new BoardViewModel(userModel);
            viewModel = (BoardViewModel)DataContext;
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
            datetimeTB.Text = DateTime.Now.ToString();
        }

        /// <summary>
        /// Add Task Click button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTaskBox_Click(object sender, RoutedEventArgs e)
        {
            AddTaskWindow ad = new AddTaskWindow(viewModel.User,viewModel.Board.Columns[0]);
            ad.Show();
            Close();
        }
        /// <summary>
        /// Advance task click button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdvanceTaskBox_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AdvanceTask();
        }
        /// <summary>
        /// Delete task click button after selected task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteTaskBox_Click(object sender, RoutedEventArgs e)
        {
            viewModel.DeleteTask();
        }


        /// <summary>
        /// Add Column button click and gets visible and hidden every click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddColumnButton_Click(object sender, RoutedEventArgs e)
        {
            InputColumnNameTB.Visibility = InputColumnNameTB.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            CNLabel.Visibility = CNLabel.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            inputPosition.Visibility = inputPosition.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            InputPosition.Visibility = InputPosition.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            AddClick.Visibility = AddClick.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

        }
        /// <summary>
        /// Add click of a column after entering info in texbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AddColumn();
            InputColumnNameTB.Visibility = Visibility.Hidden;
            CNLabel.Visibility = Visibility.Hidden;
            inputPosition.Visibility = Visibility.Hidden;
            InputPosition.Visibility = Visibility.Hidden;
            AddClick.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Remove a column button click after selecting one
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveColumnButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.RemoveColumn();
        }
        /// <summary>
        /// limit max tasks selected column - visible and hidden each click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LimitColumnButton_Click(object sender, RoutedEventArgs e)
        {
            InputLimit.Visibility = InputLimit.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            SaveLimit.Visibility = SaveLimit.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            inputLimit.Visibility = inputLimit.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
        /// <summary>
        /// Edit column name click button - visible and hidden each click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditColumnNameButton_Click(object sender, RoutedEventArgs e)
        {
            EditCNBTB.Visibility = EditCNBTB.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            EditCNLabel.Visibility = EditCNLabel.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            EditCNButton.Visibility = EditCNButton.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
        /// <summary>
        /// Find/Filter click button - visible and hidden each click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Find_ClickButton_Click(object sender, RoutedEventArgs e)
        {
            SearchLabel.Visibility = SearchLabel.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            FilterBox.Visibility = FilterBox.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            SearchButton.Visibility = SearchButton.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
        /// <summary>
        /// logout button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogOutButton_Click_1(object sender, RoutedEventArgs e)
        {
            viewModel.Logout();
            LoginWindow login = new LoginWindow();
            login.Show();
            Close();
        }
        /// <summary>
        /// move selected culumn left 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Move_Column_Left_Button(object sender, RoutedEventArgs e)
        {
            viewModel.MoveLeft();
        }
        /// <summary>
        /// move selected column right
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Move_Column_Right_Button(object sender, RoutedEventArgs e)
        {
            viewModel.MoveRight();
        }
        /// <summary>
        /// sort by due date all columns button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortBy_Button(object sender, RoutedEventArgs e)
        {
            viewModel.SortByDueDate();
        }


        /// <summary>
        /// Save the limit inserted button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            viewModel.LimitColumn();
            InputLimit.Visibility = Visibility.Hidden;
            SaveLimit.Visibility = Visibility.Hidden;
            inputLimit.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Save the name of the column button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveCN_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ChangeColumnName();
            EditCNBTB.Visibility = Visibility.Hidden;
            EditCNLabel.Visibility = Visibility.Hidden;
            EditCNButton.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// open task in double click on a task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenTask_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TaskWindow task = new TaskWindow(viewModel.User,viewModel.SelectedTask);
            task.Show();
            this.Close();
        }
        /// <summary>
        /// search button after entering filter 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_Button(object sender, RoutedEventArgs e)
        {
            viewModel.FilterTask();
        }
    }
}
