
using IntroSE.Kanban.Backend.DataAccessLayer.DalObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.UserPackage
{

    class BLUser
    {
        //-----------------------loger---------------------------------------------
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //-------------------fields------------------------------------------
        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        private string _nickname;
        public string NickName
        {
            get { return _nickname; }
            set { _nickname = value; }
        }
        private int _ID;
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        //-----------------------Constructer---------------------------------

        /// <summary>
        /// Constructer of BLUser object
        /// </summary>
        /// <param name="email">email key of user</param>
        /// <param name="password">user's password</param>
        /// <param name="nickname">user's nickname</param>
        /// <param name="emailHost">the board creator</param>
        public BLUser(string email, string password, string nickname, string emailHost)
        {
            if (!IsValidEmail(email)) //checks is its a real email address
            {
                log.Error("Not an actual email address");
                throw new ArgumentException("Invalid email address");
            }
            if (!PasswordIsLegal(password))
            {
                log.Warn(email + " entered illegal password");
                throw new Exception("Illegal password (Required - 5-20 characters,uppercase,lowercase and a number)");
            }
            if (String.IsNullOrEmpty(nickname))
            {
                log.Error("nickname input is null or empty");
                throw new ArgumentException("Invalid nickname");
            }
            _email = email;
            _password = password;
            _nickname = nickname;
            DalUser dUser = new DalUser(_email, _password, _nickname, emailHost);
            _ID = dUser.Insert();
        }

        /// <summary>
        /// Construcotr for loadData only
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="nickname"></param>
        /// <param name="id"></param>
        public BLUser(string email, string password, string nickname, int id)
        {
            _email = email;
            _password = password;
            _nickname = nickname;
            _ID = id;
        }

        /// <summary>
        /// Empty Constructor For loadDataOnly
        /// </summary>
        public BLUser()
        {
        }

        //-------------------------------------------Methods------------------

        /// <summary>
        /// create data base
        /// </summary>
        public void CreateDataBase()
        {
            DalUser u = new DalUser();
            u.CreateDataBase();
        }


        /// <summary>
        /// checks password of an excisting user in login proccess
        /// </summary>
        /// <param name="pass">The password of the user to login</param>
        /// <returns></returns> Returns true or false if its the right password
        public bool CheckPassword(string pass)
        {
            if (String.IsNullOrEmpty(pass))
            {
                log.Error("Input pass is null/empty");
                throw new ArgumentException("Invalid password");
            }
            return _password.Equals(pass);
        }


        /// <summary>
        /// checks if the password is legal as the requirments are
        /// </summary>
        /// <param name="pass">The password of the user to register</param>
        /// <returns></returns> Returns true or false if its legal
        public bool PasswordIsLegal(string pass)
        {
            bool HasUpperCase = false;
            bool HasLowerCase = false;
            bool HasNumber = false;
            if (String.IsNullOrWhiteSpace(pass) || (pass.Length < 5 | pass.Length > 25))
            {
                log.Error("Illegal password inserted : empty/null/not in expected length");
                return false;
            }
            foreach (char c in pass)
            {
                if (!HasUpperCase && Char.IsUpper(c))
                    HasUpperCase = true;
                if (!HasLowerCase && Char.IsLower(c))
                    HasLowerCase = true;
                if (!HasNumber && Char.IsDigit(c))
                    HasNumber = true;
            }
            return HasLowerCase & HasNumber & HasUpperCase;
        }


        /// <summary>
        /// check is email is a real email string
        /// </summary>
        /// <param name="email">input email to check</param>
        /// <returns>return true or false if its an email </returns>
        internal bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                log.Error("email is null/empty");
                return false;
            }

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }


        /// <summary>
        /// load Data
        /// </summary>
        /// <returns>Dictionary of all database users</returns>
        public Dictionary<string, BLUser> LoadData()
        {
            try
            {
                Dictionary<string, BLUser> output = new Dictionary<string, BLUser>();
                DalUser du = new DalUser();
                List<DalUser> listOfDal = du.SelectAllUsers();
                foreach (DalUser u in listOfDal)
                {
                    BLUser user = new BLUser(u._email, u._password, u._nickname, Convert.ToInt32(u._ID));
                    output.Add(u._email, user);
                }
                return output;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        /// <summary>
        /// delete all data from database
        /// </summary>
        public void DeleteData()
        {
            DalUser u = new DalUser();
            u.DeleteDataBase();
        }


    }
}
