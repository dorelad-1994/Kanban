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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        /// <summary>
        /// filed view model
        /// </summary>
        RegisterViewModel viewModel;
        /// <summary>
        /// Constructor
        /// </summary>
        public RegisterWindow()
        {
            InitializeComponent();
            this.DataContext = new RegisterViewModel();
            viewModel = (RegisterViewModel)DataContext;
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
        /// Clock event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tickevent(object sender, EventArgs e)
        {
            datetimeLabel.Text = DateTime.Now.ToString();
        }
        /// <summary>
        /// Register click button after entering all data and open the new board of user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.Register())
            {
                UserModel user = viewModel.Login();
                BoardWindow board = new BoardWindow(user);
                board.Show();
                Close();
            }
            PasswordTB.Clear();
        }
        /// <summary>
        /// return button to login window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Return_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}
