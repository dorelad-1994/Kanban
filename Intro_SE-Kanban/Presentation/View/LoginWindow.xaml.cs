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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        /// <summary>
        /// filed view model
        /// </summary>
        private LoginViewModel viewModel;
        /// <summary>
        /// Constructor
        /// </summary>
        public LoginWindow()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel();
            viewModel = (LoginViewModel)DataContext;
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
        /// login click button (default)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            UserModel user = viewModel.Login();
            if (user != null)
            {
                BoardWindow b = new BoardWindow(user);
                b.Show();
                Close();
            }
            PasswordTB.Clear();
        }
        /// <summary>
        /// register click button - open new window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow reg = new RegisterWindow();
            reg.Show();
            this.Close();
        }

    }
}