using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}