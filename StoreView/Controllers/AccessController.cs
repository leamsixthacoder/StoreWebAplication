using EntityLayer;
using BussinesLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace StoreView.Controllers
{
    public class AccessController : Controller
    {
        // GET: Access
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Enrol()
        {
            return View();
        }

      
        public ActionResult Reset()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Enrol(Client objecto)
        {

            int result;
            string message = string.Empty;

            ViewData["Nombres"] = string.IsNullOrEmpty(objecto.Nombres) ? "" : objecto.Nombres;
            ViewData["Apellidos"] = string.IsNullOrEmpty(objecto.Apellidos) ? "" : objecto.Apellidos;
            ViewData["Correo"] = string.IsNullOrEmpty(objecto.Correo) ? "" : objecto.Correo;

            if (objecto.Clave != objecto.ConfirmarClave)
            {
                ViewBag.Error = "las Contraseñas no coinciden";
                return View();

            }
            result = new BL_Client().Enrol(objecto, out message);

            if (result > 0)
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

        [HttpPost]
        public ActionResult Index(string email, string password)
        {
            Client oclient = null;

            oclient = new BL_Client().List().Where(x => x.Correo == email && x.Clave == BL_Resources.ConvertSha256(password)).FirstOrDefault();
            
            if (oclient == null)
            {
                ViewBag.Error = "Correo o Contraseña no son correctas";
                return View();
            }
            else
            {
                if (oclient.Restablecer)
                {
                    TempData["idCliente"] = oclient.IdCliente;
                    return RedirectToAction("ChangePassword", "Access");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(oclient.Correo, false);

                    Session["Cliente"] = oclient;

                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Shop");
                }
            }


        }
        [HttpPost]
        public ActionResult Reset(string email)
        {
            Client oCliente = new Client();

            oCliente = new BL_Client().List().Where(u => u.Correo == email).FirstOrDefault();

            if (oCliente == null)
            {
                ViewBag.Error = "No se encontro un cliente relacionado a ese correo";

                return View();
            }

            string message = string.Empty;
            bool answer = new BL_Client().ResetPassword(oCliente.IdCliente, email, out message);

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

        [HttpPost]

        public ActionResult ChangePassword(string idclient, string currentpass, string newpass, string confirmpass)
        {

            Client oUsuario = new Client();


            oUsuario = new BL_Client().List().Where(u => u.IdCliente == int.Parse(idclient)).FirstOrDefault();

            if (oUsuario.Clave != BL_Resources.ConvertSha256(currentpass))
            {
                TempData["IdCliente"] = idclient;
                ViewData["vpass"] = "";
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();


            }
            else if (newpass != confirmpass)
            {
                TempData["IdCliente"] = idclient;
                ViewData["vpass"] = currentpass;


                ViewBag.Error = "Las contraseñas no coinciden";
                return View();


            }
            ViewData["vpass"] = "";

            newpass = BL_Resources.ConvertSha256(newpass);

            string message = string.Empty;

            bool answer = new BL_Client().ChangePassword(int.Parse(idclient), newpass, out message);

            if (answer)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["IdCliente"] = idclient;
                ViewBag.Error = message;
                return View();

            }
        }

        public ActionResult Logout()
        {
            Session["Cliente"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Shop");

        }
    }
}