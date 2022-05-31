using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using EntityLayer;

namespace BussinesLayer
{
    public class BL_Users
    {
        private DL_Users objDataLayer = new DL_Users();

        public List<User> List() { 
            return objDataLayer.List();
        }
    }
}
