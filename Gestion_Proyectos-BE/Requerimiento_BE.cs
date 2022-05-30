using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_Proyectos_BE
{
    public class Requerimiento_BE
    {
        public string IdRequerimiento { get; set; }
        public int Solicitante { get; set; }
        public int Gerencia { get; set; }
        public int Presupuesto { get; set; }
        public string Titulo { get; set; }
        public string Alcance { get; set; }
        public string Prioridad { get; set; }
        public string Resumen { get; set; }
        public DateTime FechaReunion { get; set; }


    }
}
