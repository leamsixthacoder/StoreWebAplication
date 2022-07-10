using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityLayer;
using BussinesLayer;
using Newtonsoft.Json;
using System.Configuration;
using System.IO;

namespace AdminView.Controllers
{
    public class MaintenanceController : Controller
    {
        // GET: Maintenance
        public ActionResult ProductCategory()
        {
            return View();
        }
        public ActionResult ProductBrand ()
        {
            return View();
        }
        public ActionResult Product()
        {
            return View();
        }

        #region Category

        [HttpGet]
        public JsonResult ListProductCategory()
        {
            List<ProductCategory> olist = new List<ProductCategory>();
            olist = new BL_ProductCategory().List();
            return Json(new { data = olist }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult StoredProductCategory(ProductCategory obj)
        {
            object result;
            string message = string.Empty;

            if (obj.IdCategoria == 0)
            {
                result = new BL_ProductCategory().Enrol(obj, out message);
            }
            else
            {
                result = new BL_ProductCategory().Edit(obj, out message);
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public JsonResult DeleteProductCategory(int id)
        {
            bool result = false;
            string message = string.Empty;

            result = new BL_ProductCategory().Delete(id, out message);

            return Json(new { result = result, message }, JsonRequestBehavior.AllowGet);

        }

        #endregion


        #region Brand

        [HttpGet]
        public JsonResult ListProductBrand()
        {
            List<ProductBrand> olist = new List<ProductBrand>();
            olist = new BL_ProductBrand().List();
            return Json(new { data = olist }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult StoredProductBrand(ProductBrand obj)
        {
            object result;
            string message = string.Empty;

            if (obj.IdMarca == 0)
            {
                result = new BL_ProductBrand().Enrol(obj, out message);
            }
            else
            {
                result = new BL_ProductBrand().Edit(obj, out message);
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public JsonResult DeleteProductBrand(int id)
        {
            bool result = false;
            string message = string.Empty;

            result = new BL_ProductBrand().Delete(id, out message);

            return Json(new { result = result, message }, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Product

        [HttpGet]
        public JsonResult ListProduct()
        {
            List<Product> olist = new List<Product>();
            olist = new BL_Product().List();
            return Json(new { data = olist }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult StoredProduct(string obj, HttpPostedFileBase imageFile)
        {
            object result;
            string message = string.Empty;
            bool succesfull_operation = true;
            bool succesfull_image_stored = true;

            Product oproduct = new Product();
            oproduct = JsonConvert.DeserializeObject<Product>(obj);


            if (oproduct.IdProducto == 0)
            {
                int idgenerateProduct = new BL_Product().Enrol(oproduct, out message);

                if (idgenerateProduct !=0)
                {
                    oproduct.IdProducto = idgenerateProduct;
                }
                else
                {
                    succesfull_operation = false;
                }
            }
            else
            {
                succesfull_operation = new BL_Product().Edit(oproduct, out message);
            }

            if (succesfull_operation)
            {
                if (imageFile!= null)
                {
                    string stored_route = ConfigurationManager.AppSettings["PhotosServer"];
                    string extension = Path.GetExtension(imageFile.FileName);
                    string image_name = string.Concat(oproduct.IdProducto.ToString(),extension);
                    try
                    {
                        imageFile.SaveAs(Path.Combine(stored_route,image_name));
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        succesfull_image_stored = false;

                    }

                    if (succesfull_image_stored)
                    {
                        oproduct.RutaImagen = stored_route;
                        oproduct.NombreImagen = image_name;
                        bool rspta = new BL_Product().StoredImageData(oproduct, out message);
                    }
                    else
                    {
                        message = "Se guardo el producto pero hubo problemas con la imagen";
                    }
                }
            }
            return Json(new { succesfull_operation = succesfull_operation, idGenarate = oproduct.IdProducto , message = message }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]

        public JsonResult ProductImage(int id)
        {
            bool convertion;
            Product oproduct = new BL_Product().List().Where(p => p.IdProducto == id).FirstOrDefault();

            string textBase64 = BL_Resources.ConvertBase64(Path.Combine(oproduct.RutaImagen, oproduct.NombreImagen),  out convertion);

            return Json(new
            {
                convertion = convertion,
                textBase64 = textBase64,
                extension = Path.GetExtension(oproduct.NombreImagen)
            },
            JsonRequestBehavior.AllowGet
            );
        }

        [HttpPost]

        public JsonResult DeleteProduct(int id)
        {
            bool result = false;
            string message = string.Empty;

            result = new BL_Product().Delete(id, out message);

            return Json(new { result = result, message }, JsonRequestBehavior.AllowGet);

        }



        #endregion
    }
}