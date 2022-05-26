using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_Proyectos_BE
{
    public class Encuesta
    {
        public int IddEncuesta { get; set; }
        public int IdProyecto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdUsuarioSponsor { get; set; }
        public int Satisfaccion { get; set; }
        public string Comentario { get; set; }
        public string NombreProyecto { get; set; }
        public string NombreSponsor { get; set; }

        public string GetSatisfaccion
        {
            get
            {
                switch (Satisfaccion)
                {
                    case 1: return "Nada Satisfecho";
                    case 2: return "Insatisfecho";
                    case 3: return "Satisfecho";
                    case 4: return "Muy Satisfecho";
                    default: return "Totalmente Satisfecho";
                };
            }
        }
    }
}
