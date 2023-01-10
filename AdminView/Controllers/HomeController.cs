using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityLayer;
using BussinesLayer;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using AdminView.permissions;

namespace AdminView.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [RolAttributes(Rol.Administrador)]
        public ActionResult Users()
        {

            return View();
        }

        public ActionResult NoAuthorization()
        {

            return View();
        }

        public ActionResult Clients()
        {

            return View();
        }

        [HttpGet]
        public JsonResult ListUsers()
        {
            List<User> olist = new List<User>();
            olist = new BL_Users().List();

            return Json(new { data = olist }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ListClients()
        {
            List<Client> olist = new List<Client>();
            olist = new BL_Client().List();

            return Json(new { data = olist }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult StoredUsers(User obj)
        {
            object result;
            string message = string.Empty;

            if (obj.IdUsuario == 0)
            {
                result = new BL_Users().Enrol(obj, out message);
            }
            else
            {
                result = new BL_Users().Edit(obj, out message);
            }

            return Json(new {result = result, message = message}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public JsonResult DeleteUsers(int id)
        {
            bool result = false;
            string message = string.Empty;

            result = new BL_Users().Delete(id, out message);

            return Json(new { result = result, message }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]

        public JsonResult ListReport(string startdate, string finishdate, string idtransaction)
        {

            List<Report> olist = new List<Report>();

            olist = new BL_Report().Ventas(startdate, finishdate, idtransaction);

            return Json(new { data = olist }, JsonRequestBehavior.AllowGet);

        }



        [HttpGet]

        public JsonResult ViewDashboard()
        {
            Dashboard objeto = new BL_Report().ShowDashboard();
            return Json(new { result = objeto }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]

        public FileResult ExportVenta(string startdate, string finishdate, string idtransaction)
        {
            List<Report> olist = new List<Report>();
            
            if (idtransaction == "")
            {
                idtransaction = "0";
            }
            olist = new BL_Report().Ventas(startdate, finishdate, idtransaction);

            DataTable dt = new DataTable();
            dt.Columns.Add("FechaVenta", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("IdTransaccion", typeof(string));

            foreach(Report report in olist)
            {
                dt.Rows.Add(new object[]
                {
                    report.FechaVenta,
                    report.Cliente,
                    report.Producto,
                    report.Precio,
                    report.Cantidad,
                    report.Total,
                    report.IdTransaccion
                });
            }


            dt.TableName = "Datos";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteVenta" + DateTime.Now.ToString() + ".xlsx");
                }
            }

        }




    }

}