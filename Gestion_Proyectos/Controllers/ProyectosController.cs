using System;
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
        private readonly Requerimiento_BL _requerimientoBL;
        public ProyectosController()
        {
            _proyectoBL = new Proyecto_BL();
            _usuarioBL = new Usuario_BL();
            _gerenciaBL = new Gerencia_BL();
            _tareBL = new Tarea_BL();
            _requerimientoBL = new Requerimiento_BL();
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

        public ActionResult Requerimientos()
        {

            var listaRequerimientos = _requerimientoBL.GetRequerimientos();

            return View(listaRequerimientos);
        }

        public ActionResult MisProyectos()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ActualizarRequerimiento(string codigo = "", int estado = 0)
        {
            string mensaje;

            try
            {

                mensaje = _requerimientoBL.RegistrarRequerimientoProyecto(codigo,estado);

                if (mensaje.StartsWith("Error") || mensaje.StartsWith("No"))
                {
                    return Json(new { success = false, mensaje = mensaje });
                }
                else
                {
                    return Json(new { success = true, mensaje = mensaje });
                }

            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return Json(new { success = false, mensaje = mensaje });
            }
        }

        public JsonResult GetAdjuntosRequerimiento(string Id)
        {
            try
            {
                var adjuntos = _requerimientoBL.GetAdjuntos(Id);
                return Json(new { data = adjuntos, mensaje = "", success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new Adjunto_BE(), mensaje = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTareasPorProyecto(int Id)
        {
            try
            {
                var tareas = _tareBL.GetTareas(Id);
                return Json(new { data = tareas, mensaje = "", success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new Tarea_BE(), mensaje = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult BuscarRequerimiento(string Id)
        {
            try
            {
                var requerimiento = _requerimientoBL.BuscarRequerimiento(Id);
                return Json(new { data = requerimiento, mensaje = "", success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new Requerimiento_BE(), mensaje = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetProyectosPorUsuario(int Id)
        {
            try
            {
                var proyectos = _proyectoBL.GetProyectosUsuario(Id);
                return Json(new { data = proyectos, mensaje = "", success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new Adjunto_BE(), mensaje = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult BuscarProyectoTablero(int Id)
        {
            try
            {
                var proyecto = _proyectoBL.BuscarProyectoTablero(Id);
                return Json(new { data = proyecto, mensaje = "", success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new Proyecto_BE(), mensaje = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ActualizarProyectoEstado(int codigo = 0, string estado = "")
        {
            string mensaje;

            try
            {

                mensaje = _proyectoBL.ActualizarEstadoProyecto(codigo, estado);

                if (mensaje.StartsWith("Error") || mensaje.StartsWith("No"))
                {
                    return Json(new { success = false, mensaje = mensaje });
                }
                else
                {
                    return Json(new { success = true, mensaje = mensaje });
                }

            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return Json(new { success = false, mensaje = mensaje });
            }
        }
    }
}