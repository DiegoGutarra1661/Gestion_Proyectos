using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Gestion_Proyectos_BE;
using Gestion_Proyectos_DA;


namespace Gestion_Proyectos_BL
{
    public class Requerimiento_BL
    {
        private readonly Requerimiento_DA _requerimientoDA;

        public Requerimiento_BL()
        {
            _requerimientoDA = new Requerimiento_DA();
        }

        public string RegistrarRequerimiento(Requerimiento_BE reg, HttpPostedFileBase[] archivos)
        {
            string mensaje = "";
            try
            {
                _requerimientoDA.RegistrarRequerimiento(reg, archivos);
                mensaje = "Se envió su Solicitud de Requerimiento satisfactoriamente";
            }
            catch (Exception ex)
            {
                mensaje = "Error en registrar requerimiento: " + ex.Message;

            }
            return mensaje;
        }

        public IEnumerable<Requerimiento_BE> GetRequerimientos()
        {
            try
            {
                var requerimientos = _requerimientoDA.ListarRequerimientos();
                return requerimientos;         
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar requerimientos ==> {ex.Message}");
            }
        }
    }
}
