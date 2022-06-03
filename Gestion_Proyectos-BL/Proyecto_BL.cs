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

        public IEnumerable<Proyecto_BE> GetProyectosUsuario(int idUsuario)
        {
            try
            {
                var proyectos = _proyectoDA.ListarProyectosPorUsuario(idUsuario);
                return proyectos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar proyectos por usuario ==> {ex.Message}");
            }
        }

        public List<string> GetMembers(int idProyecto, int idFuncion)
        {
            try
            {
                var members = _proyectoDA.ListarMembers(idProyecto, idFuncion);

                return members;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar members ==> {ex.Message}");
            }
        }

        public Proyecto_BE BuscarProyecto(int id)
        {
            try
            {
                var proyecto = _proyectoDA.BuscarProyecto(id);

                if (proyecto == null)
                {
                    throw new Exception("Error ==> No se encontro proyecto");
                }

                return proyecto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en el método Buscar proyecto por codigo ==> {ex.Message}");
            }
        }

        public string EditarProyecto(Proyecto_BE reg,IEnumerable<Tarea_BE> tareas)
        {
            string mensaje = "";
            try
            {
                if (tareas != null)
                {
                    if (tareas.Sum(t => t.PorcentajeTarea) > 100)
                    {
                        mensaje = "Error las tareas suman mas de 100%";
                    }
                    else
                    {
                        _proyectoDA.EditarProyecto(reg, tareas);
                    }
                }
                else
                {
                    tareas = new List<Tarea_BE>();
                    _proyectoDA.EditarProyecto(reg, tareas);
                }

                mensaje = "Proyecto '" + reg.Nombre + "' actualizado correctamente";
            }
            catch (Exception ex)
            {
                mensaje = "Error en editar proyecto: " + ex.Message;

            }
            return mensaje;
        }

        public string RegistrarProyecto(Proyecto_BE reg, IEnumerable<Tarea_BE> tareas)
        {
            string mensaje = "";
            try
            {

                if (tareas != null)
                {
                    if (tareas.Sum(t => t.PorcentajeTarea) > 100)
                    {
                        mensaje = "Error las tareas suman mas de 100%";
                    }
                    else
                    {
                        _proyectoDA.RegistrarProyecto(reg, tareas);
                    }
                }
                else
                {
                    tareas = new List<Tarea_BE>();
                    _proyectoDA.RegistrarProyecto(reg, tareas);
                }

                mensaje = "Proyecto '" + reg.Nombre + "' registrado correctamente";
            }
            catch (Exception ex)
            {
                mensaje = "Error en registrar proyecto: " + ex.Message;

            }
            return mensaje;
        }

        public string AsignarProyecto(Proyecto_BE reg)
        {
            string mensaje = "";
            try
            {
                _proyectoDA.AsignarProyecto(reg);

                mensaje = "Proyecto asignado correctamente";
            }
            catch (Exception ex)
            {
                mensaje = "Error en asignar proyecto: " + ex.Message;

            }
            return mensaje;
        }

        public void QuitarAsignacion(int idProyecto, List<String> listaQuitar)
        {
            try
            {
                _proyectoDA.QuitarAsignacion(idProyecto,listaQuitar);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al quitar asignacion ==> {ex.Message}");
            }
        }
    }
}
