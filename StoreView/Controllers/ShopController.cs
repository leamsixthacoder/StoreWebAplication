using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BussinesLayer;
using EntityLayer;
using System.IO;
namespace StoreView.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetailProduct( int idproduct = 0)
        {

            Product oproduct = new Product();
            bool conversion;

            oproduct = new BL_Product().List().Where(p => p.IdProducto == idproduct).FirstOrDefault();

            if(oproduct != null)
            {
                oproduct.Base64 = BL_Resources.ConvertBase64(Path.Combine(oproduct.RutaImagen, oproduct.NombreImagen), out conversion);
                oproduct.Extension = Path.GetExtension(oproduct.NombreImagen);
            }

            return View(oproduct);
        }

        [HttpGet]

        public JsonResult ListCategory()
        {
            List <ProductCategory> list = new List<ProductCategory>();
            list = new BL_ProductCategory().List();
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public JsonResult ListBrandforCategory(int idcategory)
        {
            List<ProductBrand> list = new List<ProductBrand>();
            list = new BL_ProductBrand().ListBrandforCategory(idcategory);
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public JsonResult ListProduct(int idcategory, int idbrand)
        {
            List<Product> list = new List<Product>();
            bool conversion;

            list = new BL_Product().List().Select(p => new Product()
            {
                IdProducto = p.IdProducto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                oMarca = p.oMarca,
                oCategoria = p.oCategoria,
                Precio = p.Precio,
                Stock = p.Stock,
                RutaImagen = p.RutaImagen,
                Base64 = BL_Resources.ConvertBase64(Path.Combine(p.RutaImagen, p.NombreImagen), out conversion),
                Extension = Path.GetExtension(p.NombreImagen),
                Activo = p.Activo

            }).Where(p => p.oCategoria.IdCategoria == (idcategory == 0 ? p.oCategoria.IdCategoria : idcategory) && 
            p.oMarca.IdMarca ==(idbrand == 0 ? p.oMarca.IdMarca : idbrand) &&
            p.Stock > 0 && p.Activo == true
            ).ToList();

            var jsonresult = Json(new { data = list }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;
            return jsonresult;
        }
    }
}