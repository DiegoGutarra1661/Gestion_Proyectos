using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gestion_Proyectos_BE;
using Gestion_Proyectos_DA;

namespace Gestion_Proyectos_BL
{
    public class Area_BL
    {
        private readonly Area_DA _areaDA;
        public Area_BL()
        {
            _areaDA = new Area_DA();
        }

        public IEnumerable<Area_BE> GetAreas()
        {
            try
            {
                var areas = _areaDA.ListarAreas();

                return areas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar areas ==> {ex.Message}");
            }
        }
    }
}
