using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_Proyectos_BE
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Correo { get; set; }
        public string Alias { get; set; }
        public int IdArea { get; set; }
        public int IdRol { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdUsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public List<int> ListaRoles { get; set; }
        public string Iniciales { get; set; }

        public string NombreArea { get; set; }
        private string NombreRol { get; set; }
    }
}
