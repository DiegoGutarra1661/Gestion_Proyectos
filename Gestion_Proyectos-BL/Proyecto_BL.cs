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
        private readonly Tarea_DA _tareaDA;
        public Proyecto_BL()
        {
            _proyectoDA = new Proyecto_DA();
            _tareaDA = new Tarea_DA();
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

        public Chart GetProyectoGerencia()
        {
            try
            {
                var reportes = _proyectoDA.ListarProyectosGerencia();

                Chart _chart = new Chart();
                _chart.labels = reportes.Select(x => x.NombreGerencia).ToArray();
                _chart.datasets = new List<DatasetsChart>();
                List<DatasetsChart> _dataSet = new List<DatasetsChart>();
                _dataSet.Add(new DatasetsChart() { 
                    label = "Proyectos por gerencia",
                    data = reportes.Select(x => x.CantidadGerenica).ToArray(),
                    backgroundColor = new string[] { "rgb(255, 99, 132)",
                    "rgb(54, 162, 235)",
                    "rgb(255, 205, 86)",
                    "#30445a",
                    "#402a4a",
                    "#383a95",
                    "#10769e",
                    "#95598e" }
                });
                _chart.datasets = _dataSet;

                return _chart;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar cantidad de proyectos por gerenica==> {ex.Message}");
            }
        }

        public Bar GetProyectoUsuarioEstado()
        {
            try
            {
                var reportes = _proyectoDA.ListarProyectosUsuarioEstado(1,"");
               
                Bar _bar = new Bar();
                _bar.labels = reportes.Select(x => x.AliasUsuario).ToArray();
                _bar.datasets = new List<DatasetsBar>();
                List<DatasetsBar> _dataSet = new List<DatasetsBar>();
                _dataSet.Add(new DatasetsBar()
                {
                    label = "Por hacer",
                    data = _proyectoDA.ListarProyectosUsuarioEstado(2, "No iniciado").Select(x => x.CantidadProyectosUsuario).ToArray(),
                    backgroundColor = "#6c757d",
                    borderColor = "#000000"
                });
                _dataSet.Add(new DatasetsBar()
                {
                    label = "En espera",
                    data = _proyectoDA.ListarProyectosUsuarioEstado(2, "Detenido").Select(x => x.CantidadProyectosUsuario).ToArray(),
                    backgroundColor = "#dc3545",
                    borderColor = "#000000"
                });
                _dataSet.Add(new DatasetsBar()
                {
                    label = "En proceso",
                    data = _proyectoDA.ListarProyectosUsuarioEstado(2, "En proceso").Select(x => x.CantidadProyectosUsuario).ToArray(),
                    backgroundColor = "#ffc107",
                    borderColor = "#000000",
                });
                _dataSet.Add(new DatasetsBar()
                {
                    label = "Concluido",
                    data = _proyectoDA.ListarProyectosUsuarioEstado(2, "Concluido").Select(x => x.CantidadProyectosUsuario).ToArray(),
                    backgroundColor = "#28a745",
                    borderColor = "#000000",
                });


                _bar.datasets = _dataSet;

                return _bar;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar cantidad de proyectos por gerenica==> {ex.Message}");
            }
        }

        public IEnumerable<Proyecto_BE> GetProyectosUsuario(int idUsuario)
        {
            try
            {
                var proyectos = _proyectoDA.ListarProyectosPorUsuario(idUsuario);
                foreach(var tareas in proyectos)
                {   
                    tareas.Tareas = _tareaDA.ListarTareasPorProyecto(tareas.IdProyecto);
                }

                return proyectos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar proyectos por usuario ==> {ex.Message}");
            }
        }

        public List<Usuario_BE> GetMembers(int idProyecto, int idFuncion)
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

        public Proyecto_BE BuscarProyectoTablero(int id)
        {
            try
            {
                var proyecto = _proyectoDA.BuscarProyectoTablero(id);

                if (proyecto == null)
                {
                    throw new Exception("Error ==> No se encontro proyecto");
                }

                return proyecto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en el método Buscar proyecto tablero por codigo ==> {ex.Message}");
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

        public string ActualizarEstadoProyecto(int codigo, string estado)
        {
            string mensaje = "";
            try
            {

                _proyectoDA.ActualizarEstadoProyecto(codigo, estado);
                mensaje = "Se actualizó el estado del proyecto";

            }
            catch
            {
                mensaje = "Error en actualizar el estado del proyecto";

            }
            return mensaje;
        }
    }
}
