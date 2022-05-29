using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gestion_Proyectos_BE;
using Gestion_Proyectos_DA;

namespace Gestion_Proyectos_BL
{
    public class Permiso_BL
    {
        private readonly Permiso_DA _permisoDA;
        public Permiso_BL()
        {
            _permisoDA = new Permiso_DA();
        }


        public IEnumerable<Permiso_BE> GetPermisos()
        {
            try
            {
                var permisos = _permisoDA.ListarPermisos();

                return permisos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar permisos ==> {ex.Message}");
            }
        }

    }
}
