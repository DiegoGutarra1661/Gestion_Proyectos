using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gestion_Proyectos_BL;
using Gestion_Proyectos_BE;

namespace Gestion_Proyectos.Controllers
{
    public class RequerimientoController : Controller
    {
        private readonly Requerimiento_BL _requerimientoBL;
        private readonly Usuario_BL _usuarioBL;
        private readonly Gerencia_BL _gerenciaBL;

        public RequerimientoController()
        {
            _requerimientoBL = new Requerimiento_BL();
            _usuarioBL = new Usuario_BL();
            _gerenciaBL = new Gerencia_BL();
        }
        // GET: Requerimiento
        public ActionResult Solicitud()
        {
            ViewBag.solicitantes = _usuarioBL.GetUsuarios();
            ViewBag.gerencias = _gerenciaBL.GetGerencias();

            return View(new Requerimiento_BE());
        }

        [HttpPost]
        public ActionResult Solicitud(Requerimiento_BE reg, HttpPostedFileBase[] archivos)
        {
            TempData["mensaje"] = _requerimientoBL.RegistrarRequerimiento(reg, archivos);

            return RedirectToAction("Solicitud", "Requerimiento");
        }


    }
}