using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using EntityLayer;
namespace BussinesLayer
{
    public class BL_Cart
    {
        private DL_Cart objDataLayer = new DL_Cart();

        public bool ExistCart(int idclient, int idproduct)
        {
            return objDataLayer.ExistCart(idclient, idproduct);
        }

        public bool CartOperation(int idclient, int idproduct, bool sumar, out string Message)
        {
            return objDataLayer.CartOperation(idclient, idproduct, sumar, out Message);
        }

        public int CartAmount(int idclient)
        {
            return objDataLayer.CartAmount(idclient);
        }

        public List<Cart> List(int idclient)
        {
            return objDataLayer.List(idclient);
        }

        public bool Delete(int idclient, int idproduct)
        {
            return objDataLayer.Delete(idclient, idproduct);

        }



    }
}
