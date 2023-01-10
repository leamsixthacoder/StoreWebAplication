using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityLayer;
using BussinesLayer;
using System.Web.Security;

namespace AdminView.Controllers
{
    public class AccessController : Controller
    {
        // GET: Access
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        public ActionResult Reset()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string email, string pass)
        {
            User oUsuario = new User();


            oUsuario = new BL_Users().List().Where(u => u.Correo == email && u.Clave == BL_Resources.ConvertSha256(pass)).FirstOrDefault();

            if (oUsuario == null)
            {
                ViewBag.Error = "Correo o contraseña no correcta";

                return View();
            }
            else
            {
                if (oUsuario.Restablecer)
                {
                    TempData["IdUsuario"] = oUsuario.IdUsuario;
                    return RedirectToAction("ChangePassword");
                }

                FormsAuthentication.SetAuthCookie(oUsuario.Correo, false);


                ViewBag.Error = null;
                Session["User"] = oUsuario;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(string iduser, string currentpass, string newpass, string confirmpass)
        {
            User oUsuario = new User();


            oUsuario = new BL_Users().List().Where(u => u.IdUsuario == int.Parse(iduser)).FirstOrDefault();

            if (oUsuario.Clave != BL_Resources.ConvertSha256(currentpass))
            {
                TempData["IdUsuario"] = iduser;
                ViewData["vpass"] = "";
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();


            }
            else if (newpass != confirmpass)
            {
                TempData["IdUsuario"] = iduser;
                ViewData["vpass"] = currentpass;


                ViewBag.Error = "Las contraseñas no coinciden";
                return View();


            }
            ViewData["vpass"] = "";

            newpass = BL_Resources.ConvertSha256(newpass);

            string message = string.Empty;

            bool answer = new BL_Users().ChangePassword(int.Parse(iduser), newpass, out message);

            if (answer)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdUsuario"] = iduser;
                ViewBag.Error = message;
                return View();

            }

        }

        [HttpPost]
        public ActionResult Reset(string email)
        {
            User oUsuario = new User();

            oUsuario = new BL_Users().List().Where(u => u.Correo == email).FirstOrDefault();

            if (oUsuario == null)
            {
                ViewBag.Error = "No se Encontro un usuario relacionado a ese correo";

                return View();
            }

            string message = string.Empty;
            bool answer = new BL_Users().ResetPassword(oUsuario.IdUsuario, email, out message);

            if (answer)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Access");

            }
            else
            {
                ViewBag.Error = message;
                return View();

            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["User"] = null;

            return RedirectToAction("Index", "Access");

        }

    }
}