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
        [HttpGet]
        public JsonResult ListUsers()
        {
            List<User> olist = new List<User>();
            olist = new BL_Users().List();

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

            return Json (new { result = result, message }, JsonRequestBehavior.AllowGet);

        }
    }
}