using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gestion_Proyectos_BL;
using Gestion_Proyectos_BE;

namespace Gestion_Proyectos.Controllers
{
    
    public class RolesController : Controller
    {
        private readonly Rol_BL _rolBL;
        private readonly Permiso_BL _permisoBL;
        public RolesController()
        {
            _rolBL = new Rol_BL();
            _permisoBL = new Permiso_BL();
        }

        // GET: Roles
        public ActionResult Index()
        {
            var listaRoles = _rolBL.GetRoles();
            return View(listaRoles);
        }

        public ActionResult Editar(int idRol)
        {
            Rol_BE reg = _rolBL.BuscarRol(idRol);
            reg.ListaPermisos = _rolBL.GetPermisoRoles(idRol);
            ViewBag.permisos = _permisoBL.GetPermisos();
            return View(reg);
        }

        [HttpPost]
        public ActionResult Editar(Rol_BE reg)
        {
            TempData["mensaje"] = _rolBL.EditarRol(reg);
            return RedirectToAction("Index", "Roles");
        }

        public ActionResult Registrar()
        {
            ViewBag.permisos = _permisoBL.GetPermisos();
            return View(new Rol_BE());
        }

        [HttpPost]
        public ActionResult Registrar(Rol_BE reg)
        {
            TempData["mensaje"] = _rolBL.RegistrarRol(reg);
            return RedirectToAction("Index", "Roles");
        }
    }
}