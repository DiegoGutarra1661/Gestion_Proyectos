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
                    "#95598e" },
                    hoverOffset = 4
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
                var reportes = _proyectoDA.ListarProyectosUsuarioEstado();
                Bar _bar = new Bar();
                var nombresUsuario = reportes.Select(x => x.AliasUsuario).ToArray();
                _bar.labels = (string[])nombresUsuario.Distinct();
                _bar.datasets = new List<DatasetsBar>();
                List<DatasetsBar> _dataSet = new List<DatasetsBar>();
                foreach (var reporte in reportes)
                {
                    foreach(var datas in _dataSet)
                    {
                        if(reporte.EstadoProyecto == "No iniciado")
                        {
                            datas.label = "Por hacer";
                            datas.data = reportes.Select(x => x.CantidadProyectosUsuario).ToArray();
                            datas.backgroundColor = "#30445a";
                            datas.borderColor = "#000000";
                            
                        }else if(reporte.EstadoProyecto == "Detenido")
                        {
                            //datas.label = "Por hacer";
                            //datas.data = reportes.Where(x => x.EstadoProyecto == "Detenido").FirstOrDefault();
                            datas.backgroundColor = "#402a4a";
                            datas.borderColor = "#000000";

                        }
                        else if (reporte.EstadoProyecto == "En proceso")
                        {
                            datas.label = "En proceso";
                            datas.data = reportes.Select(x => x.CantidadProyectosUsuario).ToArray();
                            datas.backgroundColor = "#383a95";
                            datas.borderColor = "#000000";

                        }
                        else if (reporte.EstadoProyecto == "Concluido")
                        {
                            datas.label = "Concluido";
                            datas.data = reportes.Select(x => x.CantidadProyectosUsuario).ToArray();
                            datas.backgroundColor = "#10769e";
                            datas.borderColor = "#000000";

                        }
                        _dataSet.Add(datas);
                    }
                }

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
