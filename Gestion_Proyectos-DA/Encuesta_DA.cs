using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gestion_Proyectos_BE;
using System.Data.SqlClient;
using System.Data;
using System.Web;

namespace Gestion_Proyectos_DA
{
    public class Encuesta_DA:Conexion
    {
        Usuario_BE usu = HttpContext.Current.Session["usuario"] != null ? HttpContext.Current.Session["usuario"] as Usuario_BE : new Usuario_BE();

        public void registrarEncuesta(Encuesta_BE reg)
        {
            int rs = 0;
            using (var con = GetSqlConnGestionProyectos())
            {
                con.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_registrarEncuesta", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idProyecto", reg.IdProyecto);
                    cmd.Parameters.AddWithValue("@fechaCreacion", DateTime.Now);
                    cmd.Parameters.AddWithValue("@idUsuarioSponsor", usu.IdUsuario);
                    cmd.Parameters.AddWithValue("@satisfaccion", reg.Satisfaccion);
                    if (!string.IsNullOrEmpty(reg.Comentario))
                        cmd.Parameters.AddWithValue("@comentario", reg.Comentario);
                    else
                        cmd.Parameters.AddWithValue("@comentario", DBNull.Value);

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

        public Encuesta_BE BuscaEncuesta(int idProyecto)
        {
            using (var con = GetSqlConnGestionProyectos())
            {
                Encuesta_BE encuesta = null;

                SqlCommand cmd = new SqlCommand("USP_EXISTE_ECNUESTA", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idProyecto", idProyecto);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    encuesta = new Encuesta_BE()
                    {
                        IdEncuesta = dr.GetInt32(0),
                        FechaCreacion = dr.GetDateTime(2)
                    };
                }

                dr.Close();
                con.Close();

                return encuesta;
            }
        }

        public IEnumerable<Encuesta_BE> ListarEncuestas()
        {
            List<Encuesta_BE> listaEncuestas = new List<Encuesta_BE>();

            using (var con = GetSqlConnGestionProyectos())
            {
                SqlCommand cmd = new SqlCommand("USP_LISTAR_ENCUESTAS", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Encuesta_BE reg = new Encuesta_BE();
                    reg.IdEncuesta = dr.GetInt32(0);
                    reg.IdProyecto = dr.GetInt32(1);
                    reg.FechaCreacion = dr.GetDateTime(2);
                    reg.IdUsuarioSponsor = dr.GetInt32(3);
                    reg.Satisfaccion = dr.GetInt32(4);
                    reg.Comentario = (dr.IsDBNull(5) ? "" : dr.GetString(5));
                    reg.NombreProyecto = dr.GetString(6);
                    reg.NombreSponsor = dr.GetString(7);
                    listaEncuestas.Add(reg);
                }

                dr.Close();
                con.Close();
            }

            return listaEncuestas;
        }
    }
}
