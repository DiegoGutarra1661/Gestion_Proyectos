using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_Proyectos_BE
{
    public class Rol
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; }
        public List<int> ListaPermisos { get; set; }

    }
}
