using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using EntityLayer;

namespace BussinesLayer
{
    public  class BL_Sales
    {
        private DL_Sales objDataLayer = new DL_Sales();

        public bool Enroll(Sale obj, DataTable SaleDetail, out string Message)
        {
            return objDataLayer.Enroll(obj, SaleDetail, out Message);
        }
        public List<SaleDetail> ListSales(int idclient)
        {
            return objDataLayer.ListSales(idclient);

        }

    }
}
