using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gestion_Proyectos_BL;
using Gestion_Proyectos_BE;


namespace Gestion_Proyectos.Controllers
{
    public class UsuariosController : Controller
    {

        private readonly Usuario_BL _usuarioBL;
        private readonly Area_BL _areaBL;
        private readonly Rol_BL _rolBL;
        public UsuariosController()
        {
            _usuarioBL = new Usuario_BL();
            _areaBL = new Area_BL();
            _rolBL = new Rol_BL();
        }
        // GET: Usuarios
        public ActionResult Index()
        {
            var usuarios = _usuarioBL.GetUsuarios();

            return View(usuarios);
        }

        public ActionResult Editar(int idUsuario)
        {
            Usuario_BE reg = _usuarioBL.BuscarUsuario(idUsuario);
            reg.ListaRoles = _usuarioBL.GetRolesUsuario(idUsuario);
            ViewBag.roles = _rolBL.GetRoles();
            ViewBag.areas = new SelectList(_areaBL.GetAreas(), "IdArea", "Descripcion", reg.IdArea);
            return View(reg);
        }

        [HttpPost]
        public ActionResult Editar(Usuario_BE reg)
        {
            TempData["mensaje"] = _usuarioBL.EditarUsuario(reg);
            return RedirectToAction("Index", "Usuarios");
        }

        public ActionResult Registrar()
        {
            ViewBag.roles = _rolBL.GetRoles();
            ViewBag.areas = new SelectList(_areaBL.GetAreas(), "IdArea", "Descripcion");
            return View(new Usuario_BE());
        }

        [HttpPost]
        public ActionResult Registrar(Usuario_BE reg)
        {
            TempData["mensaje"] = _usuarioBL.RegistrarUsuario(reg);
            return RedirectToAction("Index", "Usuarios");
        }
    }
}