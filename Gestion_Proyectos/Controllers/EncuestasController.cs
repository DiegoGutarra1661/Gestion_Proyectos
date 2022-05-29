using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gestion_Proyectos_BL;
using Gestion_Proyectos_BE;

namespace Gestion_Proyectos.Controllers
{
    public class EncuestasController : Controller
    {
        private readonly Proyecto_BL _proyectoBL;
        private readonly Encuesta_BL _encuestaBL;
        public EncuestasController()
        {
            _proyectoBL = new Proyecto_BL();
            _encuestaBL = new Encuesta_BL();
        }
        // GET: Encuesta
        public ActionResult Index()
        {
            var encuestas = _encuestaBL.GetEncuestas();
            return View(encuestas);
        }

        public ActionResult Registrar(int id)
        {
            ViewBag.proyecto = _proyectoBL.BuscarProyecto(id);

            // Valida la existencia de una encuesta para el proyecto
            Encuesta_BE encuesta = _encuestaBL.BuscarEncuesta(id);

            if (encuesta == null)
                return View(encuesta);
            else
                return View("EncuestaExiste");
        }

        [HttpPost]
        public ActionResult Registrar(Encuesta_BE reg)
        {
            TempData["mensaje"] = _encuestaBL.RegistrarEncuesta(reg);
            return RedirectToAction("Gracias", "Encuestas");
        }

        public ActionResult Gracias()
        {
            return View();
        }

        public ActionResult EncuestaExiste()
        {
            Session["url"] = Request.Url.ToString();
            return View();
        }

        public JsonResult DetalleEncuesta(int idEncuesta)
        {
            return Json(_encuestaBL.GetEncuestas().Where(e => e.IdEncuesta == idEncuesta).FirstOrDefault(), JsonRequestBehavior.AllowGet);
        }
    }
}