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
    public class Rol_DA: Conexion
    {
        public Rol_BE BuscarRol(int idRol)
        {
            return ListarRoles().Where(rol => rol.IdRol == idRol).FirstOrDefault();
        }

        public List<int> ListarPermisosRol(int idUsuario)
        {
            List<int> lista = new List<int>();

            using (var con = GetSqlConnGestionProyectos())
            {
                SqlCommand cmd = new SqlCommand("usp_listarPermisosRol", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idRol", idUsuario);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    int idPermiso = dr.GetInt32(0);

                    lista.Add(idPermiso);
                }

                dr.Close(); con.Close();
            }

            return lista;
        }

        public IEnumerable<Rol_BE> ListarRoles()
        {
            List<Rol_BE> lista = new List<Rol_BE>();

            using (var con = GetSqlConnGestionProyectos())
            {
                SqlCommand cmd = new SqlCommand("usp_ListarRoles", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Rol_BE reg = new Rol_BE()
                    {
                        IdRol = dr.GetInt32(0),
                        Nombre = dr.GetString(1)
                    };

                    lista.Add(reg);
                }

                dr.Close(); con.Close();
            }

            return lista;
        }

        public void EditarRol(Rol_BE reg)
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
                    SqlCommand cmd = new SqlCommand("usp_editarRol", con, tr);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idRol", reg.IdRol);
                    cmd.Parameters.AddWithValue("@nombre", reg.Nombre);
                    rs = cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("usp_resetearPermisos", con, tr);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idRol", reg.IdRol);
                    rs = cmd.ExecuteNonQuery();

                    foreach (var item in reg.ListaPermisos)
                    {
                        cmd = new SqlCommand("usp_registrarPermisoRol", con, tr);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idRol", reg.IdRol);
                        cmd.Parameters.AddWithValue("@idPermiso", item);
                        cmd.Parameters.AddWithValue("@idUsuarioCreacion", usu.IdUsuario);
                        cmd.Parameters.AddWithValue("@fechaCreacion", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
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

        public int obtenerNuevoID()
        {
            int nuevoId = 0;
            using (var con = GetSqlConnGestionProyectos())
            {
                SqlCommand cmd = new SqlCommand("select max(idRol) from Rol", con);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    nuevoId = dr.GetInt32(0);
                }

                dr.Close(); con.Close();
            }

            return (nuevoId + 1);
        }

        public void RegistrarRol(Rol_BE reg)
        {
            int rs = 0;
            Usuario_BE usu = HttpContext.Current.Session["usuario"] != null ? HttpContext.Current.Session["usuario"] as Usuario_BE : new Usuario_BE();
            int nuevoId = obtenerNuevoID();
            using (var con = GetSqlConnGestionProyectos())
            {
                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    // Definir el SqlCommand y el tipo de instruccion
                    SqlCommand cmd = new SqlCommand("usp_registrarRol", con, tr);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", reg.Nombre);
                    rs = cmd.ExecuteNonQuery();

                    foreach (var item in reg.ListaPermisos)
                    {
                        cmd = new SqlCommand("usp_registrarPermisoRol", con, tr);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idRol", nuevoId);
                        cmd.Parameters.AddWithValue("@idPermiso", item);
                        cmd.Parameters.AddWithValue("@idUsuarioCreacion", usu.IdUsuario);
                        cmd.Parameters.AddWithValue("@fechaCreacion", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
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
    }
}
