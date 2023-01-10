using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EntityLayer;

namespace AdminView.permissions
{
    public class RolAttributes: ActionFilterAttribute
    {
        private Rol _idRol;

        public RolAttributes(Rol idRol)
        {
            _idRol = idRol;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

            if (HttpContext.Current.Session["User"] != null)
            {
                User user = HttpContext.Current.Session["User"] as User;

                if(user.IdRol != this._idRol && user.IdRol != Rol.Administrador)
                {
                    filterContext.Result = new RedirectResult("~/Home/NoAuthorization");
                }
            }
            base.OnActionExecuted(filterContext);
        }
    }
}
