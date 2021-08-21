using IntroSE.Kanban.Backend.BusinessLayer.UserPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class UserService
    {
        //-------------------fields------------------------------------------
        private UserController _userController;

        //-----------------------Constructer---------------------------------
        /// <summary>
        /// Simple public constructer
        /// </summary>
        public UserService()
        {
            this._userController = new UserController();
        }


        //-------------------------------------------Methods------------------

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="email">The email address of the user to register</param>
        /// <param name="password">The password of the user to register</param>
        /// <param name="nickname">The nickname of the user to register</param>
        /// <returns>A response object. The response should contain a error message in case of an error<returns
        public Response Register(String email, String password, String nickname)
        {
            try
            {
                _userController.Register(email, password, nickname,email);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }


        /// <summary>
        /// Log in an existing user
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response object with a value set to the user, instead the response should contain a error message in case of an error</returns>
        public Response<User> Login(String email, String password)
        {
            try
            {
                BLUser tmp = _userController.Login(email, password);
                User user = new User(tmp.Email, tmp.NickName);
                return new Response<User>(user);
            }
            catch (Exception e)
            {
                return new Response<User>(e.Message);
            }
         
        }

        /// <summary>        
        /// Log out an logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response Logout(string email)
        {
            try
            {
                _userController.Logout(email);
                return new Response();
            }
            catch(Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// check if there is any user logged in on kanban for information to board service
        /// </summary>
        /// <returns>true or false if there is a user logged in </returns>
        public String LoggedIn()
        {
            return _userController.GetUserIsLogedIn();
        }
        /// <summary>
        /// Loades data from directory to User controller
        /// </summary>
        /// <returns>Return response message</returns>
        public Response LoadData()
        {
            try
            {
                _userController.LoadData();
                return new Response();
            }
            catch(Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Delete all the data 
        /// </summary>
        /// <returns>Return response message</returns>
        public Response DeleteData()
        {
            try
            {
                _userController.DeleteData();
                return new Response();
            }
            catch(Exception e)
            {
                return new Response(e.Message);
            }

        }

        /// <summary>
		/// Registers a new user and joins the user to an existing board.
		/// </summary>
		/// <param name="email">The email address of the user to register</param>
		/// <param name="password">The password of the user to register</param>
		/// <param name="nickname">The nickname of the user to register</param>
		/// <param name="emailHost">The email address of the host user which owns the board</param>
		/// <returns>A response object. The response should contain a error message in case of an error<returns>
		public Response Register(string email, string password, string nickname, string emailHost)
        {
            try
            {
                _userController.Register2(email,password,nickname,emailHost);
                return new Response();
            }
            catch(Exception e)
            {
                return new Response(e.Message);
            }

        }

    }
}
