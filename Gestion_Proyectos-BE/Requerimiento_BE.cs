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
        public string FechaFormateada { get; set; }
        //
        public string DescripcionUsuario { get; set; }
        public string CorreoUsuario { get; set; }
        public string DescripcionGerencia { get; set; }
        public string DescripcionPresupuesto { get; set; }

        public string DescripcionEstado { get; set; }
    
        public string getColorEstado()
        {
            if (DescripcionEstado == "Pendiente")
                return "bg-warning";
            else if (DescripcionEstado == "Aprobado")
                return "bg-success";
            else if (DescripcionEstado == "Rechazado")
                return "bg-danger";
            else
                return "bg-danger";
        }
    }
}
