using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gestion_Proyectos_BE;
using Gestion_Proyectos_DA;

namespace Gestion_Proyectos_BL
{
    public class Encuesta_BL
    {
        private readonly Encuesta_DA _encuestaDA;

        public Encuesta_BL()
        {
            _encuestaDA = new Encuesta_DA();
        }

        public Encuesta_BE BuscarEncuesta(int idProyecto)
        {
            try
            {
                var encuesta = _encuestaDA.BuscaEncuesta(idProyecto);

                if (encuesta == null)
                {
                    throw new Exception("Error ==> No se encontro encuesta");
                }

                return encuesta;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en el método Buscar encuesta por codigo ==> {ex.Message}");
            }
        }

        public string RegistrarEncuesta(Encuesta_BE reg)
        {
            string mensaje = "";
            try
            {
                _encuestaDA.registrarEncuesta(reg);

                mensaje = "Encuesta registrada correctamente";
            }
            catch (Exception ex)
            {
                mensaje = "Error en registrar encuesta: " + ex.Message;

            }
            return mensaje;
        }
        public IEnumerable<Encuesta_BE> GetEncuestas()
        {
            try
            {
                var encuestas = _encuestaDA.ListarEncuestas();

                return encuestas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar encuestas ==> {ex.Message}");
            }
        }

    }
}
