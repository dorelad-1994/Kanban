using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DalObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace IntroSE.Kanban.Backend.BusinessLayer.UserPackage
{
    class UserController
    {
        //-----------------------loger---------------------------------------------
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //-------------------fields------------------------------------------
        private Dictionary<string, BLUser> _listOfUsers;
        public Dictionary<string, BLUser> ListOfUsers
        {
            get { return _listOfUsers; }
            set { _listOfUsers = value; }
        }

        private bool _userIsLogedIn;
        public bool UserIslogedIn
        {
            get { return _userIsLogedIn; }
            set { _userIsLogedIn = value; }
        }

        private BLUser _CurrentLogedUser;
        public BLUser CurrentLogedUser
        {
            get { return _CurrentLogedUser; }
            set { _CurrentLogedUser = value; }
        }

        //-----------------------Constructor---------------------------------
        /// <summary>
        /// Simple public constructer
        /// </summary>
        public UserController()
        {
            _listOfUsers = new Dictionary<string, BLUser>();
            _userIsLogedIn = false;
            CurrentLogedUser = null;
            BLUser u = new BLUser();
            u.CreateDataBase();
        }

        //-------------------------------------------Methods------------------


        ///<summary>This method registers a new user to the system.</summary>
        ///<param name="email">the user e-mail address, used as the username for logging the system.</param>
        ///<param name="password">the user password.</param>
        ///<param name="nickname">the user nickname.</param>
        ///<param name="emailHost">the email of the creator - same like email</param>
        /// <returns> void </returns>
        public void Register(string email, string password, string nickname, string emailHost)
        {
            email = email.ToLower();
            emailHost = emailHost.ToLower();
            if (UserExists(email)) //checks if user is already registered
            {
                log.Warn(email + " is allready registred");
                throw new ArgumentException(email + " is allready registred, please login");
            }
            BLUser newUser = new BLUser(email, password, nickname, emailHost); //in BLUser checks all input + lower case email + saved in database
            log.Info(email + " have been registered to Kanban");
            _listOfUsers.Add(newUser.Email, newUser);
        }
        /// <summary>
        /// This method registers a new user to the system with email host board
        /// </summary>
        ///<param name="email">the user e-mail address, used as the username for logging the system.</param>
        ///<param name="password">the user password.</param>
        ///<param name="nickname">the user nickname.</param>
        ///<param name="emailHost">the email of the creator board</param>
        public void Register2(string email, string password, string nickname, string emailHost)
        {
            UserHostExists(emailHost);
            Register(email, password, nickname, emailHost);
        }

        /// <summary>
        /// Log in an existing user
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A BuessnessLayer User object</returns>
        public BLUser Login(string email, string password)
        {
            string emailL = email.ToLower(); // convert to lower
            if (UserIslogedIn) // checks if a current user is logged in
            {
                log.Error("Tried to login while other user is logged in");
                throw new Exception("A User is loged in allready");
            }
            if (!UserExists(emailL)) //checks if a user email is in the dictionary or null
            {
                log.Warn(email + " is not registered to Kanban");
                throw new Exception(email + " is not registered to Kanban, please register");
            }
            BLUser user = _listOfUsers[emailL];
            if (!user.CheckPassword(password)) //check if input pass match the real pass of the user
            {
                log.Warn(email + "'s input password does not match his/hers current password");
                throw new Exception("Invalid password");
            }
            UserIslogedIn = true;
            CurrentLogedUser = user;
            log.Info(email + " logged in to Kanban");
            return user;
        }

        /// <summary>        
        /// Log out an logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns> void </returns>
        public void Logout(string email)
        {
            if (email == null)
            {
                log.Warn("email input for logout is null");
                throw new ArgumentException("email input is null");
            }
            string emailL = email.ToLower(); // convert to lower
            if (!(ListOfUsers.ContainsKey(emailL)))
            {
                log.Warn(email + " : user does not exist in Kanban");
                throw new Exception("User does not exist");
            }
            if (!(CurrentLogedUser.Email.Equals(emailL)))
            {
                log.Error(email + ": " + "Such user is currently not logged in");
                throw new Exception("This is not the current logged in user");
            }
            log.Info(email + " logged out from Kanban");
            CurrentLogedUser = null;
            UserIslogedIn = false;
        }


        //----------------------------------------------------DATA-------------------------------------------
        /// <summary>
        /// Load Data From database
        /// </summary>
        public void LoadData()
        {
            try
            {
                BLUser u = new BLUser();
                ListOfUsers = u.LoadData();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// Delete all persistent records from kanban users
        /// </summary>
        public void DeleteData()
        {
            ListOfUsers = new Dictionary<string, BLUser>();
            CurrentLogedUser = null;
            UserIslogedIn = false;
            BLUser u = new BLUser();
            u.DeleteData();
        }

        //-------------------------------------------------other-------------------------------------------

        /// <summary>
        /// checks if user exists on kanban
        /// </summary>
        /// <param name="email">email key to check if the list of user contains it</param>
        /// <returns>returns true or false the user exists</returns>
        internal virtual bool UserExists(string email)
        {
            if (email == null)
            {
                log.Error("email input in Login is null");
                throw new ArgumentException("please enter email field");
            }
            return _listOfUsers.ContainsKey(email);
        }
        /// <summary>
        /// check if the emailhost if exsist
        /// </summary>
        /// <param name="emailHost"></param>
        private void UserHostExists(string emailHost)
        {
            if (emailHost == null)
            {
                log.Error("email input in Login is null");
                throw new ArgumentException(("please enter email host field"));
            }
            if (!_listOfUsers.ContainsKey(emailHost.ToLower()))
            {
                log.Error("emailHost input does not exist in Kanban");
                throw new ArgumentException($"{emailHost} is not registed");
            }
        }
        /// <summary>
        /// getter for boardcontroller
        /// </summary>
        /// <returns>the email of the user that is currently logged in</returns>
        public string GetUserIsLogedIn()
        {
            if (CurrentLogedUser == null)
            {
                return "";
            }
            return CurrentLogedUser.Email;
        }
    }
}
