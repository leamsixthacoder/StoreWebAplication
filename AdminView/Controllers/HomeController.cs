using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityLayer;
using BussinesLayer;

namespace AdminView.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Users()
        {

            return View();
        }

        public JsonResult ListUsers()
        {
            List<User> olist = new List<User>();
            olist = new BL_Users().List();

            return Json(olist, JsonRequestBehavior.AllowGet);
        }

        
    }
}