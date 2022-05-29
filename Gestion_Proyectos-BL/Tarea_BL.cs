using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gestion_Proyectos_BE;
using Gestion_Proyectos_DA;

namespace Gestion_Proyectos_BL
{
    public class Tarea_BL
    {
        private readonly Tarea_DA _tareaDA;

        public Tarea_BL()
        {
            _tareaDA = new Tarea_DA();
        }

        public IEnumerable<Tarea_BE> GetTareas(int idProyecto)
        {
            try
            {
                var tareas = _tareaDA.ListarTareas(idProyecto);

                return tareas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar tareas ==> {ex.Message}");
            }
        }

        public IEnumerable<Tarea_BE> GetTareasPorProyecto(int idProyecto)
        {
            try
            {
                var tareas = _tareaDA.ListarTareasPorProyecto(idProyecto);

                return tareas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar tareas por proyecto ==> {ex.Message}");
            }
        }

        public void QuitarTarea(List<int> listaQuitar)
        {
            try
            {
                _tareaDA.QuitarTarea(listaQuitar);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al quitar tareas ==> {ex.Message}");
            }
        }

    }
}
