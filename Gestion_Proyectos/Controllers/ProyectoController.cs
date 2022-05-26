using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gestion_Proyectos_BL;

namespace Gestion_Proyectos.Controllers
{
    public class ProyectoController : Controller
    {
        private readonly Proyecto_BL _proyectoBL;
        public ProyectoController()
        {
            _proyectoBL = new Proyecto_BL();
        }

        // GET: Proyecto
        public ActionResult Index()
        {
            var listaProyectos = _proyectoBL.GetProyectos(Session["filtrosGerencia"] as List<int>, Session["filtrosEstado"] as List<string>, Session["filtrosMember"] as List<int>);
            return View(listaProyectos);
        }
    }
}