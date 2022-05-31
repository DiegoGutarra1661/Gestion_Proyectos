﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gestion_Proyectos_BL;
using Gestion_Proyectos_BE;

namespace Gestion_Proyectos.Controllers
{
    public class ProyectosController : Controller
    {
        private readonly Proyecto_BL _proyectoBL;
        private readonly Usuario_BL _usuarioBL;
        private readonly Gerencia_BL _gerenciaBL;
        private readonly Tarea_BL _tareBL;
        public ProyectosController()
        {
            _proyectoBL = new Proyecto_BL();
            _usuarioBL = new Usuario_BL();
            _gerenciaBL = new Gerencia_BL();
            _tareBL = new Tarea_BL();
        }

        // GET: Proyecto
        public ActionResult Index()
        {
            ViewBag.gerencias = _gerenciaBL.GetGerencias();
            ViewBag.members = _usuarioBL.GetTeamMembers();

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

        public ActionResult Editar(int idProyecto)
        {
            Proyecto_BE reg = _proyectoBL.BuscarProyecto(idProyecto);
            ViewBag.sponsors = new SelectList(_usuarioBL.GetSponsors(), "IdUsuario", "Alias", reg.IdUsuarioSponsor);
            ViewBag.tareas = _tareBL.GetTareasPorProyecto(idProyecto);

            return View(reg);
        }

        public ActionResult GetTareas(int idProyecto)
        {
            IEnumerable<Tarea_BE> listaTareas = _tareBL.GetTareasPorProyecto(idProyecto);
            return Json(listaTareas, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult Editar(Proyecto_BE reg, IEnumerable<Tarea_BE> tareas)
        {

            TempData["mensaje"] = _proyectoBL.EditarProyecto(reg, tareas);
            return RedirectToAction("Index", "Proyectos");
        }

        public ActionResult Registrar()
        {
            ViewBag.sponsors = new SelectList(_usuarioBL.GetSponsors(), "IdUsuario", "Alias");
            return View(new Proyecto_BE());
        }

        [HttpPost]
        public ActionResult Registrar(Proyecto_BE reg, IEnumerable<Tarea_BE> tareas)
        {
            
            TempData["mensaje"] = _proyectoBL.RegistrarProyecto(reg, tareas);
 
            return RedirectToAction("Index", "Proyectos");
        }

        public ActionResult Asignar(int idProyecto)
        {

            Proyecto_BE reg = _proyectoBL.BuscarProyecto(idProyecto);
            reg.AliasCoordinadores = _proyectoBL.GetMembers(idProyecto, 1);
            reg.AliasDesarrolladores = _proyectoBL.GetMembers(idProyecto, 2);
            ViewBag.teamMembers = _usuarioBL.GetTeamMembers();
            return View(reg);
        }

        [HttpPost]
        public ActionResult Asignar(Proyecto_BE reg, List<string> ListaQuitar = null)
        {
            TempData["mensaje"] = _proyectoBL.AsignarProyecto(reg);
            if (ListaQuitar != null)
                _proyectoBL.QuitarAsignacion(reg.IdProyecto, ListaQuitar);
            return RedirectToAction("Index", "Proyectos");
        }
    }
}