using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class NotifiableModelObject : NotifiableObject
    {
        /// <summary>
        /// controller to service connection
        /// </summary>
        public BackendController Controller { get; private set; }
        /// <summary>
        /// NotifiableModelObject
        /// </summary>
        /// <param name="controller"></param>
        protected NotifiableModelObject(BackendController controller)
        {
            this.Controller = controller;
        }
    }
}
