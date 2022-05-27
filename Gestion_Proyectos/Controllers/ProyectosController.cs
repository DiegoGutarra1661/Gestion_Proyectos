using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gestion_Proyectos_BL;

namespace Gestion_Proyectos.Controllers
{
    public class ProyectosController : Controller
    {
        private readonly Proyecto_BL _proyectoBL;
        private readonly Usuario_BL _usuarioBL;
        private readonly Gerencia_BL _gerenciaBL;
        public ProyectosController()
        {
            _proyectoBL = new Proyecto_BL();
            _usuarioBL = new Usuario_BL();
            _gerenciaBL = new Gerencia_BL();
        }

        // GET: Proyecto
        public ActionResult Index()
        {
            ViewBag.gerencias = _gerenciaBL.GetGerencias();
            ViewBag.members = _usuarioBL.GetMembers();

            var listaProyectos = _proyectoBL.GetProyectos(Session["filtrosGerencia"] as List<int>, Session["filtrosEstado"] as List<string>, Session["filtrosMember"] as List<int>);

           
            return View(listaProyectos);
        }
        public ActionResult AñadirFiltros(List<int> lstGerencia = null, List<string> lstEstado = null, List<int> lstMember = null)
        {
            if (lstGerencia != null)
                Session["filtrosGerencia"] = lstGerencia;
            else
                Session["filtrosGerencia"] = new List<int>();
            if (lstEstado != null)
                Session["filtrosEstado"] = lstEstado;
            else
                Session["filtrosEstado"] = new List<string>();
            if (lstMember != null)
                Session["filtrosMember"] = lstMember;
            else
                Session["filtrosMember"] = new List<int>();
            return RedirectToAction("Index", "Proyectos");
        }
    }
}