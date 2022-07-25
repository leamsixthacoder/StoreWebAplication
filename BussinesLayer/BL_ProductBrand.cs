using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using EntityLayer;
namespace BussinesLayer
{
    public class BL_ProductBrand
    {
        private DL_ProductBrand objDataLayer = new DL_ProductBrand();
        public List<ProductBrand> List()
        {
            return objDataLayer.List();
        }

        public int Enrol(ProductBrand obj, out string Message)
        {

            Message = String.Empty;
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Message = "La descripcion de la marca no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Message))
            {

                return objDataLayer.Enrol(obj, out Message);
            }
            else
            {
                return 0;
            }

        }
        public bool Edit(ProductBrand obj, out string Message)
        {

            Message = String.Empty;
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Message = "La descripcion de la marca no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Message))
            {
                return objDataLayer.Edit(obj, out Message);
            }
            else
            {
                return false;
            }
        }

        public bool Delete(int id, out string Message)
        {

            return objDataLayer.Delete(id, out Message);
        }

    }
}
