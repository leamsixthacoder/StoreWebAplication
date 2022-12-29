using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BussinesLayer;
using EntityLayer;
using System.IO;
using System.Threading.Tasks;
using System.Data;
using EntityLayer.Paypal;
using StoreView.filter;

namespace StoreView.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            return View();
        }
        [SessionValidate]
        [Authorize]
        public ActionResult Cart()
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

        [HttpPost]

        public JsonResult AddCart(int idproduct)
        {
            int idclient = ((Client)Session["Cliente"]).IdCliente;

            bool exists = new BL_Cart().ExistCart(idclient, idproduct);

            bool result = false;
            string message = string.Empty;

            if (exists)
            {
                message = "El producto ya existe en el carrito";
            }
            else
            {
                result = new BL_Cart().CartOperation(idclient, idproduct, true, out message);

            }

            return Json(new {result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        [SessionValidate]
        [Authorize]

        [HttpGet]
        public JsonResult CartAmount()
        {
            int idclient = ((Client)Session["Cliente"]).IdCliente;
            int amount = new BL_Cart().CartAmount(idclient);

            return Json(new { amount = amount }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]

        public JsonResult ListCartProducts()
        {
            int idclient = ((Client)Session["Cliente"]).IdCliente;

            List<Cart> oList = new List<Cart>();

            bool conversion;

            oList =  new BL_Cart().List(idclient).Select(oc => new Cart() { 
            
                oProducto = new Product()
                {
                    IdProducto = oc.oProducto.IdProducto,
                    Nombre = oc.oProducto.Nombre,
                    oMarca = oc.oProducto.oMarca,
                    Precio = oc.oProducto.Precio,
                    RutaImagen = oc.oProducto.RutaImagen,
                    NombreImagen = oc.oProducto.NombreImagen,
                    Base64 = BL_Resources.ConvertBase64( Path.Combine(oc.oProducto.RutaImagen, oc.oProducto.NombreImagen), out conversion),
                    Extension = Path.GetExtension(oc.oProducto.NombreImagen)
                },
                Cantidad = oc.Cantidad
            }).ToList();

            return Json(new { data = oList }, JsonRequestBehavior.AllowGet);


        }



        [HttpPost]

        public JsonResult CartOperation(int idproduct, bool sumar)
        {
            int idclient = ((Client)Session["Cliente"]).IdCliente;

            bool result = false;
            string message = string.Empty;

            result = new BL_Cart().CartOperation(idclient, idproduct, sumar, out message);

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public JsonResult DeleteCart(int idproduct)
        {
            int idclient = ((Client)Session["Cliente"]).IdCliente;

            bool result = false;
            string message = string.Empty;

            result = new BL_Cart().Delete(idclient, idproduct);

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetDepartments()
        {
            List<Department> oList = new List<Department>();
            oList = new BL_Location().GetDepartments();
            return Json(new { list = oList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProvincias(string IdDepartment)
        {
            List<Provincia> oList = new List<Provincia>();
            oList = new BL_Location().GetProvincias(IdDepartment);
            return Json(new { list = oList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDistritic(string IdProvincia,string IdDepartment)
        {
            List<Distritic> oList = new List<Distritic>();
            oList = new BL_Location().GetDistritics(IdProvincia, IdDepartment);
            return Json(new { list = oList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public async Task<JsonResult> PaymentProccess(List<Cart> ocartList, Sale sale)
        {
            decimal total = 0;

            DataTable sale_detail = new DataTable();
            sale_detail.Locale = new System.Globalization.CultureInfo("en-US");
            sale_detail.Columns.Add("IdProduct", typeof(string));
            sale_detail.Columns.Add("Cant",typeof(int));
            sale_detail.Columns.Add("Total",typeof(decimal));
       
            List<Item> oItemlist = new List<Item>();
            
            foreach (Cart ocart in ocartList)
            {
                decimal subtotal = Convert.ToDecimal(ocart.Cantidad.ToString()) * ocart.oProducto.Precio;

                total += subtotal;

                oItemlist.Add(new Item()
                {
                    name = ocart.oProducto.Nombre,
                    quantity = ocart.Cantidad.ToString(),
                    unit_amount = new UnitAmount()
                    {
                        currency_code = "USD",
                        value = ocart.oProducto.Precio.ToString()
                    }
                });


                sale_detail.Rows.Add(new object[]
                {
                    ocart.oProducto.IdProducto,
                    ocart.Cantidad,
                    subtotal
                });
            }

            PurchaseUnit purchaseUnit = new PurchaseUnit()
            {
                amount = new Amount()
                {
                    currency_code = "USD",
                    value = total.ToString(),
                    breakdown = new Breakdown()
                    {
                        item_total = new ItemTotal()
                        {
                            currency_code = "USD",
                            value = total.ToString(),
                        }
                    }
                },
                description ="compra de articulo de mi tienda",
                items = oItemlist
            };

            Checkout_Order checkout_Order = new Checkout_Order()
            {
                intent = "CAPTURE",
                purchase_units = new List<PurchaseUnit>() { purchaseUnit},
                application_context = new ApplicationContext()
                {
                    brand_name = "MiTienda.com",
                    landing_page = "NO_PREFERENCE",
                    user_action = "PAY_NOW",
                    return_url = "https://localhost:44387/Shop/Paymentmade",
                    cancel_url = "https://localhost:44387/Shop/Cart"
                }
            };

            sale.MontoTotal = total;
            sale.IdCliente = ((Client)Session["Cliente"]).IdCliente;

            TempData["Sale"] = sale;
            TempData["SaleDetail"] = sale_detail;

            BL_Paypal bL_Paypal = new BL_Paypal();
            Response_Paypal<Response_Checkout> response_Paypal = new Response_Paypal<Response_Checkout>();
            response_Paypal = await bL_Paypal.CreateRequest(checkout_Order);


            return Json(response_Paypal, JsonRequestBehavior.AllowGet);

        }

        [SessionValidate]
        [Authorize]

        public async Task<ActionResult> Paymentmade()
        {

            string token = Request.QueryString["token"];
            BL_Paypal opaypal = new BL_Paypal();
            Response_Paypal<Response_Capture> response_Paypal = new Response_Paypal<Response_Capture>();

            response_Paypal = await opaypal.ApprovePayment(token);

            ViewData["Status"] = response_Paypal.Status;

            if(response_Paypal.Status)
            {
                Sale oSale = (Sale)TempData["Sale"];

                DataTable sale_detail = (DataTable)TempData["SaleDetail"];

                oSale.idTransaccion = response_Paypal.Response.purchase_units[0].payments.captures[0].id;

                string message = string.Empty;

                bool response = new BL_Sales().Enroll(oSale, sale_detail, out message);

                ViewData["IdTransaccion"] = oSale.idTransaccion;
            }

            return View();
        }

        [SessionValidate]
        [Authorize]
        public ActionResult MyOrders()
        {
            int idclient = ((Client)Session["Cliente"]).IdCliente;

            List<SaleDetail> oList = new List<SaleDetail>();

            bool conversion;

            oList = new BL_Sales().ListSales(idclient).Select(oc => new SaleDetail()
            {

                oProducto = new Product()
                {
                    Nombre = oc.oProducto.Nombre,
                    Precio = oc.oProducto.Precio,
                    Base64 = BL_Resources.ConvertBase64(Path.Combine(oc.oProducto.RutaImagen, oc.oProducto.NombreImagen), out conversion),
                    Extension = Path.GetExtension(oc.oProducto.NombreImagen)
                },
                Cantidad = oc.Cantidad,
                Total= oc.Total,
                idTransaccion = oc.idTransaccion
            }).ToList();

            return View(oList);

        }

    }

}