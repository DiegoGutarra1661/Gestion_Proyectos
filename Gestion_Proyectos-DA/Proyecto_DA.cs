using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gestion_Proyectos_BE;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Net.Mail;
using System.Web.Mvc;

namespace Gestion_Proyectos_DA
{
    public class Proyecto_DA : Conexion
    {
        public Proyecto_BE BuscarProyecto(int idProyecto)
        {
            Proyecto_BE reg = null;

            using (var con = GetSqlConnGestionProyectos())
            {
                SqlCommand cmd = new SqlCommand("usp_buscarProyecto", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idProyecto", idProyecto);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    reg = new Proyecto_BE();
                    reg.IdProyecto = dr.GetInt32(0);
                    reg.Nombre = dr.GetString(1);
                    if (!dr.IsDBNull(2))
                        reg.Descripcion = dr.GetString(2);
                    reg.EstadoProyecto = dr.GetString(3);
                    reg.Avance = dr.GetDecimal(4);
                    if (!dr.IsDBNull(5))
                        reg.IdUsuarioSponsor = dr.GetInt32(5);
                    reg.Prioridad = dr.GetInt32(6);
                    if (!dr.IsDBNull(7))
                        reg.FecharRequerimiento = dr.GetDateTime(7);
                    if (!dr.IsDBNull(8))
                        reg.FechaInicioEstimada = dr.GetDateTime(8);
                    if (!dr.IsDBNull(9))
                        reg.FechaInicio = dr.GetDateTime(9);
                    if (!dr.IsDBNull(10))
                        reg.FechaConcluidoEstimada = dr.GetDateTime(10);
                    if (!dr.IsDBNull(11))
                        reg.FechaConcluido = dr.GetDateTime(11);
                    reg.AreaProyecto = dr.GetString(12);
                    if (!dr.IsDBNull(13))
                        reg.Proveedor = dr.GetString(13);
                    if (!dr.IsDBNull(14))
                        reg.ResponsableLiberacion = dr.GetString(14);
                    if (!dr.IsDBNull(15))
                        reg.ETALiberacion = dr.GetDateTime(15);
                    if (!dr.IsDBNull(16))
                        reg.Comentario = dr.GetString(16);
                    /*if (!dr.IsDBNull(12))
                        reg.Estado = dr.GetInt32(12);
                    reg.IdUsuarioCreacion = dr.GetInt32(13);
                    if (!dr.IsDBNull(14))
                        reg.FechaCreacion = dr.GetDateTime(14);
                    if (!dr.IsDBNull(15))
                        reg.IdUsuarioModificacion = dr.GetInt32(15);
                    if (!dr.IsDBNull(15))
                        reg.FechaModificacion = dr.GetDateTime(16);*/


                }
                dr.Close(); con.Close();
            }

            return reg;
        }

        public Proyecto_BE BuscarProyectoTablero(int idProyecto)
        {
            Proyecto_BE reg = null;

            using (var con = GetSqlConnGestionProyectos())
            {
                SqlCommand cmd = new SqlCommand("usp_buscarProyectoTablero", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idProyecto", idProyecto);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    reg = new Proyecto_BE();
                    reg.IdProyecto = dr.GetInt32(0);
                    reg.Nombre = dr.GetString(1);
                    reg.Descripcion = dr.GetString(2);
                    reg.Avance = dr.GetDecimal(3);
                    reg.DescripcionSponsor = dr.GetString(4);
                    reg.Prioridad = dr.GetInt32(5);
                    reg.SFechaRequerimiento = dr.GetString(6);
                    reg.SFechaInicioEstimada = dr.GetString(7);
                    reg.SFechaInicio = dr.GetString(8); 
                    reg.SFechaConclusionEstimada = dr.GetString(9);
                    reg.SFechaConclusion = dr.GetString(10);
                    reg.Proveedor = dr.GetString(11);
                    reg.ResponsableLiberacion = dr.GetString(12);
                    reg.DescripcionETALiberacion = dr.GetString(13);
                    reg.Comentario = dr.GetString(14);
                }
                dr.Close(); con.Close();
            }

            return reg;
        }
        public void EditarProyecto(Proyecto_BE reg, IEnumerable<Tarea_BE> tareas)
        {
            int rs = 0;
            Usuario_BE usu = HttpContext.Current.Session["usuario"] != null ? HttpContext.Current.Session["usuario"] as Usuario_BE : new Usuario_BE();

            using (var con = GetSqlConnGestionProyectos())
            {

                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);

                try
                {
                    // Definir el SqlCommand y el tipo de instruccion
                    SqlCommand cmd = new SqlCommand("usp_editarProyecto", con, tr);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idProyecto", reg.IdProyecto);
                    cmd.Parameters.AddWithValue("@nombre", reg.Nombre);
                    if (string.IsNullOrEmpty(reg.Descripcion))
                        cmd.Parameters.AddWithValue("@descripcion", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@descripcion", reg.Descripcion);
                    cmd.Parameters.AddWithValue("@estadoProyecto", reg.EstadoProyecto);
                    cmd.Parameters.AddWithValue("@idUsuarioSponsor", reg.IdUsuarioSponsor);
                    cmd.Parameters.AddWithValue("@prioridad", reg.Prioridad);
                    if (reg.FecharRequerimiento.ToString("dd/MM/yyyy") == "01/01/0001")
                        cmd.Parameters.AddWithValue("@fechaRequerimiento", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@fechaRequerimiento", reg.FecharRequerimiento.ToString("dd/MM/yyyy"));
                    if (reg.FechaInicioEstimada.ToString("dd/MM/yyyy") == "01/01/0001")
                        cmd.Parameters.AddWithValue("@fechaInicioEstimada", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@fechaInicioEstimada", reg.FechaInicioEstimada.ToString("dd/MM/yyyy"));
                    if (reg.FechaInicio.ToString("dd/MM/yyyy") == "01/01/0001")
                        cmd.Parameters.AddWithValue("@fechaInicio", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@fechaInicio", reg.FechaInicio.ToString("dd/MM/yyyy"));
                    if (reg.FechaConcluidoEstimada.ToString("dd/MM/yyyy") == "01/01/0001")
                        cmd.Parameters.AddWithValue("@fechaConcluidoEstimada", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@fechaConcluidoEstimada", reg.FechaConcluidoEstimada.ToString("dd/MM/yyyy"));
                    if (reg.FechaConcluido.ToString("dd/MM/yyyy") == "01/01/0001")
                        cmd.Parameters.AddWithValue("@fechaConcluido", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@fechaConcluido", reg.FechaConcluido.ToString("dd/MM/yyyy"));
                    cmd.Parameters.AddWithValue("@idUsuarioModificacion", usu.IdUsuario);
                    cmd.Parameters.AddWithValue("@fechaModificacion", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                    if (string.IsNullOrEmpty(reg.Proveedor))
                        cmd.Parameters.AddWithValue("@proveedor", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@proveedor", reg.Proveedor);
                    if (string.IsNullOrEmpty(reg.ResponsableLiberacion))
                        cmd.Parameters.AddWithValue("@responsableliberacion", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@responsableliberacion", reg.ResponsableLiberacion);
                    if (reg.ETALiberacion.ToString("dd/MM/yyyy") == "01/01/0001")
                        cmd.Parameters.AddWithValue("@etaliberacion", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@etaliberacion", reg.ETALiberacion.ToString("dd/MM/yyyy"));
                    if (string.IsNullOrEmpty(reg.Comentario))
                        cmd.Parameters.AddWithValue("@comentario", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@comentario", reg.Comentario);

                    rs = cmd.ExecuteNonQuery();


                    foreach (var tarea in tareas)
                    {
                        cmd = new SqlCommand("usp_Registrar_Actulizar_Tareas", con, tr);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idProyecto", reg.IdProyecto);
                        cmd.Parameters.AddWithValue("@idTarea", tarea.IdTarea);
                        cmd.Parameters.AddWithValue("@nombreTarea", tarea.NombreTarea);
                        cmd.Parameters.AddWithValue("@porcentaje", tarea.PorcentajeTarea);
                        cmd.Parameters.AddWithValue("@estado", tarea.EstadoTarea);
                        rs = cmd.ExecuteNonQuery();
                    }

                    tr.Commit();

                    // Valida si es que el estado del proyecto es 'concluido'
                    if (reg.EstadoProyecto == "Concluido")
                        EnviarEncuesta(reg);


                }
                catch (SqlException ex)
                {

                    tr.Rollback();
                    con.Close();
                }
                finally
                {
                    con.Close();
                }


            }
        }

        public void ActualizarEstadoProyecto(int idProyecto, string estado)
        {
            int rs = 0;
           
            using (var con = GetSqlConnGestionProyectos())
            {

                con.Open();

                try
                {
                    // Definir el SqlCommand y el tipo de instruccion
                    SqlCommand cmd = new SqlCommand("usp_actualizarEstadoProyecto", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idProyecto", idProyecto);
                    cmd.Parameters.AddWithValue("@estadoProyecto", estado);
                    rs = cmd.ExecuteNonQuery();
                    
                

                }
                catch (SqlException ex)
                {
                   
                    con.Close();
                }
                finally
                {
                    con.Close();
                }


            }
        }

        public IEnumerable<Proyecto_BE> ListarProyectos()
        {
            List<Proyecto_BE> lista = new List<Proyecto_BE>();

            using (var con = GetSqlConnGestionProyectos())
            {
                SqlCommand cmd = new SqlCommand("usp_listarProyectos", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Proyecto_BE reg = new Proyecto_BE();
                    reg.IdProyecto = dr.GetInt32(0);
                    reg.Nombre = dr.GetString(1);
                    if (!dr.IsDBNull(2))
                        reg.Descripcion = dr.GetString(2);
                    reg.EstadoProyecto = dr.GetString(3);
                    if (!dr.IsDBNull(4))
                        reg.Avance = dr.GetDecimal(4);
                    if (!dr.IsDBNull(5))
                        reg.IdUsuarioSponsor = dr.GetInt32(5);
                    reg.Prioridad = dr.GetInt32(6);
                    if (!dr.IsDBNull(7))
                        reg.FecharRequerimiento = dr.GetDateTime(7);
                    if (!dr.IsDBNull(8))
                        reg.FechaInicioEstimada = dr.GetDateTime(8);
                    if (!dr.IsDBNull(9))
                        reg.FechaInicio = dr.GetDateTime(9);
                    if (!dr.IsDBNull(10))
                        reg.FechaConcluidoEstimada = dr.GetDateTime(10);
                    if (!dr.IsDBNull(11))
                        reg.FechaConcluido = dr.GetDateTime(11);
                    reg.AreaProyecto = dr.GetString(12);
                    /*if (!dr.IsDBNull(12))
                        reg.Estado = dr.GetInt32(12);
                    reg.IdUsuarioCreacion = dr.GetInt32(13);
                    if (!dr.IsDBNull(14))
                        reg.FechaCreacion = dr.GetDateTime(14);
                    if (!dr.IsDBNull(15))
                        reg.IdUsuarioModificacion = dr.GetInt32(15);
                    if (!dr.IsDBNull(15))
                        reg.FechaModificacion = dr.GetDateTime(16);*/
                    lista.Add(reg);

                }
                dr.Close(); con.Close();
            }

            return lista;
        }

        public IEnumerable<Proyecto_BE> ListarProyectosPorUsuario(int idUsuario)
        {
            List<Proyecto_BE> lista = new List<Proyecto_BE>();
           

            using (var con = GetSqlConnGestionProyectos())
            {
                SqlCommand cmd = new SqlCommand("usp_buscarProyectosUsuario", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Proyecto_BE reg = new Proyecto_BE();
                    reg.IdProyecto = dr.GetInt32(0);
                    reg.Nombre = dr.GetString(1);
                    reg.EstadoProyecto = dr.GetString(2);
                    lista.Add(reg);

                }
                dr.Close(); con.Close();
            }

            return lista;
        }

        public IEnumerable<Proyecto_BE> ListarProyectos(List<int> lstGerencia, List<string> lstEstados, List<int> lstMembers)
        {
            List<Proyecto_BE> lista = new List<Proyecto_BE>();
            string condicionEstado = "";

            if (lstEstados != null)
            {
                //for (int i = 0; i < lstEstados.Count(); i++)
                //{
                //    condicionEstado.Append("," + i);
                //}

                foreach (var estado in lstEstados)
                {
                    condicionEstado = condicionEstado += (estado + ",");
                }

            }

            string condicionGerencia = "";
            if (lstGerencia != null)
            {
                //for (int i = 0; i < lstGerencia.Count(); i++)
                //{
                //    if (i > 0)
                //        condicionGerencia.Append("and a.idgerencia = '" + lstGerencia[i] + "' ").ToString();
                //    else
                //        condicionGerencia.Append("and a.idgerencia = '" + lstGerencia[i] + "' ").ToString();
                //}

                foreach (var idGerencia in lstGerencia)
                {
                    condicionGerencia = condicionGerencia += (idGerencia + ",");
                }
            }

            string condicionMember = "";
            if (lstMembers != null)
            {
                //for (int i = 0; i < lstMembers.Count(); i++)
                //{
                //    if (i > 0)
                //        condicionGerencia.Append("and fp.idusuario = '" + lstMembers[i] + "' ").ToString();
                //    else
                //        condicionGerencia.Append("and fp.idusuario = '" + lstMembers[i] + "' ").ToString();
                //}

                foreach (var idTeamMember in lstMembers)
                {
                    condicionMember = condicionMember += (idTeamMember + ",");
                }
            }

            using (var con = GetSqlConnGestionProyectos())
            {
                //SqlCommand cmd = new SqlCommand("select	distinct p.idproyecto, nombre, P.descripcion, estadoproyecto, avance, idusuariosponsor, prioridad, fecharequerimiento, fechainicioestimada, fechainicio, fechaconcluidoestimada, fechaconcluido, a.descripcion " +
                //                                "from Proyecto p inner join Usuario u on p.idusuariosponsor = u.idusuario inner join Area a on u.idarea = a.idarea " +
                //                                " left join FuncionProyecto fp on fp.idproyecto = p.idproyecto " +
                //                                "where u.idusuario != 0 " + condicionEstado + condicionGerencia + condicionMember + "order by prioridad asc", con);

                con.Open();
                SqlCommand cmd = new SqlCommand("usp_Filtrar_Proyectos", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@estados_proyecto", condicionEstado);
                cmd.Parameters.AddWithValue("@gerencias_proyecto", condicionGerencia);
                cmd.Parameters.AddWithValue("@team_members", condicionMember);


                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Proyecto_BE reg = new Proyecto_BE();
                    reg.IdProyecto = dr.GetInt32(0);
                    reg.Nombre = dr.GetString(1);
                    if (!dr.IsDBNull(2))
                        reg.Descripcion = dr.GetString(2);
                    reg.EstadoProyecto = dr.GetString(3);
                    if (!dr.IsDBNull(4))
                        reg.Avance = dr.GetDecimal(4);
                    if (!dr.IsDBNull(5))
                        reg.IdUsuarioSponsor = dr.GetInt32(5);
                    reg.Prioridad = dr.GetInt32(6);
                    if (!dr.IsDBNull(7))
                        reg.FecharRequerimiento = dr.GetDateTime(7);
                    if (!dr.IsDBNull(8))
                        reg.FechaInicioEstimada = dr.GetDateTime(8);
                    if (!dr.IsDBNull(9))
                        reg.FechaInicio = dr.GetDateTime(9);
                    if (!dr.IsDBNull(10))
                        reg.FechaConcluidoEstimada = dr.GetDateTime(10);
                    if (!dr.IsDBNull(11))
                        reg.FechaConcluido = dr.GetDateTime(11);
                    reg.AreaProyecto = dr.GetString(12);
                    reg.DiasRestantes = dr.GetInt32(13);
                    if (dr.GetInt32(13) < 0)
                    {
                        reg.DiasRestantes = 0;
                    }
                    else
                    {
                        reg.DiasRestantes = dr.GetInt32(13);
                    }
                    reg.AvancePorcentaje = dr.GetDecimal(14);
                    reg.AvanceAutomaticoPlanificado = dr.GetDecimal(15);
                    reg.AvancePlanificadoFinal = dr.GetDecimal(16);
                    reg.AvanceReal = dr.GetDecimal(17);
                    reg.Desviacion = dr.GetDecimal(18);
                    reg.FechaFinProyectada = dr.GetString(19);
                    reg.Proveedor = dr.GetString(20);
                    reg.ResponsableLiberacion = dr.GetString(21);
                    reg.DescripcionETALiberacion = dr.GetString(22);
                    reg.Comentario = dr.GetString(23);
                    reg.DescripcionSolicitante = dr.GetString(24);
                    reg.ListaMemberes = ListarMembers(reg.IdProyecto);
                    /*if (!dr.IsDBNull(12))
                        reg.Estado = dr.GetInt32(12);
                    reg.IdUsuarioCreacion = dr.GetInt32(13);
                    if (!dr.IsDBNull(14))
                        reg.FechaCreacion = dr.GetDateTime(14);
                    if (!dr.IsDBNull(15))
                        reg.IdUsuarioModificacion = dr.GetInt32(15);
                    if (!dr.IsDBNull(15))
                        reg.FechaModificacion = dr.GetDateTime(16);*/
                    lista.Add(reg);

                }
                dr.Close(); con.Close();
            }

            return lista;
        }

        public void QuitarAsignacion(int idProyecto, List<String> listaQuitar)
        {
            int rs = 0;
            using (var con = GetSqlConnGestionProyectos())
            {
                con.Open();

                try
                {
                    // Definir el SqlCommand y el tipo de instruccion
                    foreach (var item in listaQuitar)
                    {
                        SqlCommand cmd = new SqlCommand("usp_quitarAsignacion", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idproyecto", idProyecto);
                        cmd.Parameters.AddWithValue("@alias", item.ToString());
                        rs = cmd.ExecuteNonQuery();
                    }

                }
                catch (SqlException ex)
                {

                    con.Close();
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public void RegistrarProyecto(Proyecto_BE reg, IEnumerable<Tarea_BE> tareas)
        {
            Usuario_BE usu = HttpContext.Current.Session["usuario"] != null ? HttpContext.Current.Session["usuario"] as Usuario_BE : new Usuario_BE();

            int rs = 0;
            using (var con = GetSqlConnGestionProyectos())
            {

                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);

                try
                {
                    // Definir el SqlCommand y el tipo de instruccion
                    SqlCommand cmd = new SqlCommand("usp_registrarProyecto", con, tr);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", reg.Nombre);
                    if (string.IsNullOrEmpty(reg.Descripcion))
                        cmd.Parameters.AddWithValue("@descripcion", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@descripcion", reg.Descripcion);
                    cmd.Parameters.AddWithValue("@estadoProyecto", reg.EstadoProyecto);
                    cmd.Parameters.AddWithValue("@avance", reg.Avance);
                    cmd.Parameters.AddWithValue("@idUsuarioSponsor", reg.IdUsuarioSponsor);
                    cmd.Parameters.AddWithValue("@prioridad", reg.Prioridad);
                    if (reg.FecharRequerimiento.ToString("dd/MM/yyyy") == "01/01/0001")
                        cmd.Parameters.AddWithValue("@fechaRequerimiento", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@fechaRequerimiento", reg.FecharRequerimiento.ToString("dd/MM/yyyy"));
                    if (reg.FechaInicioEstimada.ToString("dd/MM/yyyy") == "01/01/0001")
                        cmd.Parameters.AddWithValue("@fechaInicioEstimada", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@fechaInicioEstimada", reg.FechaInicioEstimada.ToString("dd/MM/yyyy"));
                    if (reg.FechaInicio.ToString("dd/MM/yyyy") == "01/01/0001")
                        cmd.Parameters.AddWithValue("@fechaInicio", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@fechaInicio", reg.FechaInicio.ToString("dd/MM/yyyy"));
                    if (reg.FechaConcluidoEstimada.ToString("dd/MM/yyyy") == "01/01/0001")
                        cmd.Parameters.AddWithValue("@fechaConcluidoEstimada", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@fechaConcluidoEstimada", reg.FechaConcluidoEstimada.ToString("dd/MM/yyyy"));
                    if (reg.FechaConcluido.ToString("dd/MM/yyyy") == "01/01/0001")
                        cmd.Parameters.AddWithValue("@fechaConcluido", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@fechaConcluido", reg.FechaConcluido.ToString("dd/MM/yyyy"));
                    cmd.Parameters.AddWithValue("@idUsuarioCreacion", usu.IdUsuario);
                    cmd.Parameters.AddWithValue("@fechaCreacion", DateTime.Now);

                    cmd.Parameters.AddWithValue("@proveedor", reg.Proveedor);
                    cmd.Parameters.AddWithValue("@comentario", reg.Comentario);

                    rs = cmd.ExecuteNonQuery();


                    foreach (var tarea in tareas)
                    {
                        cmd = new SqlCommand("usp_registrarTarea", con, tr);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idProyecto", 0);
                        cmd.Parameters.AddWithValue("@nombreTarea", tarea.NombreTarea);
                        cmd.Parameters.AddWithValue("@porcentaje", tarea.PorcentajeTarea);
                        rs = cmd.ExecuteNonQuery();
                    }

                    tr.Commit();

                }
                catch (SqlException ex)
                {

                    tr.Rollback();
                    con.Close();
                }
                finally
                {
                    con.Close();
                }


            }
        }

        public void AsignarProyecto(Proyecto_BE reg)
        {
            int rs = 0;
            using (var con = GetSqlConnGestionProyectos())
            {
                con.Open();
                try
                {
                    // Definir el SqlCommand y el tipo de instruccion

                    if (reg.ListaCoordinadores != null)
                    {
                        foreach (var item in reg.ListaCoordinadores)
                        {
                            SqlCommand cmd = new SqlCommand("usp_asignar_proyecto", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idproyecto", reg.IdProyecto);
                            cmd.Parameters.AddWithValue("@idusuario", item.ToString());
                            cmd.Parameters.AddWithValue("@idfuncion", 1);
                            rs = cmd.ExecuteNonQuery();
                        }
                    }

                    if (reg.ListaDesarrolladores != null)
                    {
                        foreach (var item in reg.ListaDesarrolladores)
                        {
                            SqlCommand cmd = new SqlCommand("usp_asignar_proyecto", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idproyecto", reg.IdProyecto);
                            cmd.Parameters.AddWithValue("@idusuario", item.ToString());
                            cmd.Parameters.AddWithValue("@idfuncion", 2);
                            rs = cmd.ExecuteNonQuery();
                        }
                    }

                 
                }
                catch (SqlException ex)
                {
                 
                    con.Close();
                }
                finally
                {
                    con.Close();
                }

               
            }
        }

        public List<string> ListarMembers(int idProyecto, int idFuncion)
        {
            List<string> lista = new List<string>();

            using (var con = GetSqlConnGestionProyectos())
            {
                SqlCommand cmd = new SqlCommand("usp_listarAlias", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idproyecto", idProyecto);
                cmd.Parameters.AddWithValue("@idfuncion", idFuncion);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    string alias = dr.GetString(0);

                    lista.Add(alias);
                }

                dr.Close(); con.Close();
            }

            return lista;
        }

        public List<Usuario_BE> ListarMembers(int idProyecto)
        {
            List<Usuario_BE> lista = new List<Usuario_BE>();

            using (var con = GetSqlConnGestionProyectos())
            {
                SqlCommand cmd = new SqlCommand("usp_listarIniciales", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idproyecto", idProyecto);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();


                while (dr.Read())
                {
                    Usuario_BE reg = new Usuario_BE();
                    reg.Iniciales = dr.GetString(0);
                    reg.Alias = dr.GetString(1);

                    lista.Add(reg);
                }

                dr.Close(); con.Close();
            }

            return lista;
        }

        public void EnviarEncuesta(Proyecto_BE proyecto)
        {
            // Creación y configuración del cliente para el envío de las encuestas
            SmtpClient client = new SmtpClient("smtp.office365.com", 587);
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("servicedesk.ti@adexperu.org.pe", "@dex2021");

            // Define el enlace a enviar
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            string dominio = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + urlHelper.Content("~");


            string enlace = (dominio + "Encuestas/Registrar/" + proyecto.IdProyecto);

            // Obtiene el correo del sponsor
            string correoSponsor = GetCorreoSponsor(proyecto.IdProyecto);

            // Creación de los atributos del correo
            MailAddress from = new MailAddress("servicedesk.ti@adexperu.org.pe", String.Empty, System.Text.Encoding.UTF8);
            MailAddress to = new MailAddress(correoSponsor);
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Encuesta satisfacción proyecto: " + proyecto.Nombre;
            message.IsBodyHtml = true;
            message.Body = "Por favor responda esta encuesta de satisfacción de proyecto <br><br> " +
                            "<a href='" + enlace + "'>" + enlace + "</a> <br> <br>" +
                            "¡Vamos por más! <br>" +
                            "Soluciones TI";
            message.BodyEncoding = System.Text.Encoding.UTF8;

            message.SubjectEncoding = System.Text.Encoding.UTF8;

            client.Send(message);
        }

        public string GetCorreoSponsor(int idProyecto)
        {
            string correo = "";

            using (var con = GetSqlConnGestionProyectos())
            {
                SqlCommand cmd = new SqlCommand("USP_CORREO_SPONSOR", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idProyecto", idProyecto);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    correo = dr.GetString(0);
                }

                dr.Close();
                con.Close();
            }

            return correo;
        }

        public decimal GetAvanceAutomatico(DateTime fInicio, DateTime fFinal)
        {
            decimal porcentaje = 0;



            return porcentaje;
        }
        
    }
}
