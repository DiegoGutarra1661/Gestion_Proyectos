using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gestion_Proyectos_BE;
using Gestion_Proyectos_DA;

namespace Gestion_Proyectos_BL
{
    public class Proyecto_BL
    {
        private readonly Proyecto_DA _proyectoDA;

        public Proyecto_BL()
        {
            _proyectoDA = new Proyecto_DA();
        }

        public IEnumerable<Proyecto_BE> GetProyectos(List<int> lstGerencia, List<string> lstEstados, List<int> lstMembers)
        {
            try
            {
                var proyectos = _proyectoDA.ListarProyectos(lstGerencia,lstEstados,lstMembers);

                return proyectos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar proyectos ==> {ex.Message}");
            }
        }
    }
}
