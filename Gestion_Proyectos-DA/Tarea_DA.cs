using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gestion_Proyectos_BE;
using System.Data.SqlClient;
using System.Data;


namespace Gestion_Proyectos_DA
{
    public class Tarea_DA: Conexion
    {
        public IEnumerable<Tarea_BE> ListarTareas(int idProyecto)
        {
            List<Tarea_BE> lista = new List<Tarea_BE>();

            using (var con = GetSqlConnGestionProyectos())
            {
                SqlCommand cmd = new SqlCommand("usp_listarTareas", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idproyecto", idProyecto);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();


                while (dr.Read())
                {
                    Tarea_BE reg = new Tarea_BE();
                    reg.NombreTarea = dr.GetString(2);
                    reg.PorcentajeTarea = dr.GetDecimal(3);
                    reg.EstadoTarea = dr.GetInt32(4);

                    lista.Add(reg);
                }

                dr.Close(); con.Close();
            }

            return lista;
        }

        public IEnumerable<Tarea_BE> ListarTareasPorProyecto(int idProyecto)
        {
            List<Tarea_BE> lista = new List<Tarea_BE>();

            using (var con = GetSqlConnGestionProyectos())
            {
                SqlCommand cmd = new SqlCommand("usp_listarTareasPorProyecto", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idproyecto", idProyecto);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();


                while (dr.Read())
                {
                    Tarea_BE reg = new Tarea_BE();
                    reg.IdTarea = dr.GetInt32(0);
                    reg.IdProyecto = dr.GetInt32(1);
                    reg.NombreTarea = dr.GetString(2);
                    reg.PorcentajeTarea = dr.GetDecimal(3);
                    reg.EstadoTarea = dr.GetInt32(4);
                    reg.EstadoProyecto = dr.GetString(5);
                    lista.Add(reg);
                }

                dr.Close(); con.Close();
            }

            return lista;
        }

        public void QuitarTarea(List<int> listaQuitar)
        {
            int rs = 0;
            using (var con = GetSqlConnGestionProyectos())
            {
                con.Open();
                string resultado = "";
                try
                {
                    // Definir el SqlCommand y el tipo de instruccion
                    foreach (var item in listaQuitar)
                    {
                        SqlCommand cmd = new SqlCommand("usp_EliminarTarea", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idTarea", item);
                        rs = cmd.ExecuteNonQuery();
                    }

                }
                catch (SqlException ex)
                {
                    resultado = ex.Message;
                    con.Close();
                }
                finally
                {
                    con.Close();
                }
            }
                
        }
        public void RegistrarTarea(Tarea_BE reg)
        {
            int rs = 0;
            using (var con = GetSqlConnGestionProyectos())
            {
                con.Open();

                try
                {
                    // Definir el SqlCommand y el tipo de instruccion
                    SqlCommand cmd = new SqlCommand("usp_registrarTarea", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idProyecto", reg.EstadoTarea);
                    cmd.Parameters.AddWithValue("@nombreTarea", reg.NombreTarea);
                    cmd.Parameters.AddWithValue("@porcentaje", reg.PorcentajeTarea);
                    cmd.Parameters.AddWithValue("@estado", reg.EstadoTarea);

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
    }
}
