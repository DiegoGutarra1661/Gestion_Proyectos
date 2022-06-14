using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gestion_Proyectos.Controllers;
using Gestion_Proyectos_BE;

namespace Gestion_Proyectos.Filters
{
    public class VerifySession : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Usuario_BE usuario = HttpContext.Current.Session["usuario"] as Usuario_BE;
            if (usuario == null)
            {
                if (filterContext.Controller is AccountController == false)
                {
                    if (filterContext.Controller is RequerimientoController == false)
                    {

                        var urlHelp = new UrlHelper(HttpContext.Current.Request.RequestContext);
                        filterContext.Result = new RedirectResult(urlHelp.Action("Login", "Account", new { rutaOrigen = filterContext.HttpContext.Request.Url.ToString() }));
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}