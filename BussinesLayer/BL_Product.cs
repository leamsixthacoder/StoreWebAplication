using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using EntityLayer;
namespace BussinesLayer
{
  public class BL_Product
    {
        private DL_Product objDataLayer = new DL_Product();
        public List<Product> List()
        {
            return objDataLayer.List();
        }

        public int Enrol(Product obj, out string Message)
        {

            Message = String.Empty;
            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Message = "El nombre del producto no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Message = "La descripcion del producto no puede ser vacio";
            }
            else if (obj.oMarca.IdMarca == 0)
            {
                Message = "Debe seleccionar una marca";
            }
            else if (obj.oCategoria.IdCategoria == 0)
            {
                Message = "Debe seleccionar una categoria";
            }
            else if (obj.oCategoria.IdCategoria == 0)
            {
                Message = "Debe seleccionar una categoria";
            }
            else if (obj.Precio == 0)
            {
                Message = "Debe ingresar el precio del producto ";
            }
            else if (obj.Stock == 0)
            {
                Message = "Debe ingresar el stock del producto ";
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
        public bool Edit(Product obj, out string Message)
        {

            Message = String.Empty;
            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Message = "El nombre del producto no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Message = "La descripcion del producto no puede ser vacio";
            }
            else if (obj.oMarca.IdMarca == 0)
            {
                Message = "Debe seleccionar una marca";
            }
            else if (obj.oCategoria.IdCategoria == 0)
            {
                Message = "Debe seleccionar una categoria";
            }
            else if (obj.oCategoria.IdCategoria == 0)
            {
                Message = "Debe seleccionar una categoria";
            }
            else if (obj.Precio == 0)
            {
                Message = "Debe ingresar el precio del producto ";
            }
            else if (obj.Stock == 0)
            {
                Message = "Debe ingresar el stock del producto ";
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


        public bool StoredImageData(Product obj, out string Message)
        {

            return objDataLayer.StoredImageData(obj, out Message);  
        }
        public bool Delete(int id, out string Message)
        {

            return objDataLayer.Delete(id, out Message);
        }

    }
}
