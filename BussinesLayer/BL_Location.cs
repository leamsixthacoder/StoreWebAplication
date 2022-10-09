using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using EntityLayer;

namespace BussinesLayer
{
    public class BL_Location
    {
        private DL_Location objDataLayer = new DL_Location();

        public List<Department> GetDepartments()
        {
            return objDataLayer.GetDepartments();
        }

        public List<Provincia> GetProvincias(string iddepartment)
        {
            return objDataLayer.GetProvincias(iddepartment);
        }

        public List<Distritic> GetDistritics(string idprovincia, string iddepartment)
        {
            return objDataLayer.GetDistritics(idprovincia,iddepartment);
        }
    }
}
