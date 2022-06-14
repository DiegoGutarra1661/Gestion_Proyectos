using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_Proyectos_BE
{
    public class Tarea_BE
    {
        public int IdTarea { get; set; }
        public int IdProyecto { get; set; }
        public string NombreTarea { get; set; }
        public Decimal PorcentajeTarea { get; set; }
        public int EstadoTarea { get; set; }
        //
        public string EstadoProyecto { get; set; }
        public string getColorTareas()
        {
            if (EstadoTarea == 2)
                return "bg-warning";
            else if (EstadoTarea == 1)
                return "bg-secondary";
            else
                return "bg-success";
        }

        public string getTextoTarea()
        {
            if (EstadoTarea == 2)
                return "En proceso";
            else if (EstadoTarea == 1)
                return "No iniciado";
            else
                return "Concluido";
        }
    }
}
