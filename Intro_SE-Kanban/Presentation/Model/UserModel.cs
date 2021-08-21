using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class UserModel : NotifiableModelObject
    {
        /// <summary>
        /// Email of the user
        /// </summary>
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
            }
        }
        /// <summary>
        /// the nick name of the user
        /// </summary>
        private string _nickname;
        public string NickName
        {
            get => _nickname;
            set
            {
                _nickname = value;
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="controller">backend controller</param>
        /// <param name="email"> email of the user</param>
        /// <param name="nickname"> nick name of the user</param>
        public UserModel(BackendController controller, string email, string nickname) : base(controller)
        {
            Email = email;
            NickName = nickname;
        }
    }
}
