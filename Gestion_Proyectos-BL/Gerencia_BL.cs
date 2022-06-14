using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gestion_Proyectos_BE;
using Gestion_Proyectos_DA;

namespace Gestion_Proyectos_BL
{
    public class Gerencia_BL
    {
        private readonly Gerencia_DA _gerenciaDA;
        public Gerencia_BL()
        {
            _gerenciaDA = new Gerencia_DA();
        }

        public IEnumerable<Gerencia_BE> GetGerencias()
        {
            try
            {
                var gerencias = _gerenciaDA.ListarGerencias();

                return gerencias;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar gerencias ==> {ex.Message}");
            }
        }

    }
}
