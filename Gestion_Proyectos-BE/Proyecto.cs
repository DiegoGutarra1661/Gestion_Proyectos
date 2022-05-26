using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_Proyectos_BE
{
    public class Proyecto
    {
        public int IdProyecto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string EstadoProyecto { get; set; }
        public Decimal Avance { get; set; }
        public int IdUsuarioSponsor { get; set; }
        public int Prioridad { get; set; }
        public DateTime FecharRequerimiento { get; set; }
        public DateTime FechaInicioEstimada { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaConcluidoEstimada { get; set; }
        public DateTime FechaConcluido { get; set; }
        public int Estado { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdUsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int DiasRestantes { get; set; }
        public Decimal AvancePorcentaje { get; set; }
        public string AreaProyecto { get; set; }
        public List<int> ListaCoordinadores { get; set; }
        public List<int> ListaDesarrolladores { get; set; }
        public List<string> AliasCoordinadores { get; set; }
        public List<string> AliasDesarrolladores { get; set; }
        //public List<Usuario> listaMemberes;

        //public IEnumerable<Tarea> tareas;

        public string getColorAvance()
        {
            if (EstadoProyecto == "En proceso" && FechaConcluidoEstimada > DateTime.Now)
                return "bg-warning";
            else if (EstadoProyecto == "En proceso" && FechaConcluidoEstimada < DateTime.Now)
                return "bg-danger";
            else if (EstadoProyecto == "Detenido" || EstadoProyecto == "No iniciado")
                return "bg-secondary";
            else
                return "bg-success";
        }



        public string getColorEstado()
        {
            if (EstadoProyecto == "En proceso")
                return "bg-warning";
            else if (EstadoProyecto == "No iniciado")
                return "bg-secondary";
            else if (EstadoProyecto == "Detenido")
                return "bg-danger";
            else
                return "bg-success";
        }
    }
}
