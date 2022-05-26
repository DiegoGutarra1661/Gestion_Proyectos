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
    public class Usuario_DA :Conexion
    {
        public Usuario_BE Login(string correo)
        {
            Usuario_BE usuario = null;

            try
            {
                using (var con = GetSqlConnGestionProyectos())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("usp_validarUsuario", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@correo", correo);

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        usuario = new Usuario_BE()
                        {
                            IdUsuario = dr.GetInt32(0),
                            Correo = dr.GetString(1),
                            Alias = dr.GetString(2),
                            IdArea = dr.GetInt32(3)
                        };
                    }

                    dr.Close();
                    con.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return usuario;
        }

        public IEnumerable<string> ListarPermisosLectura(int idUsuario, int idRol)
        {
            List<string> lista = new List<string>();
            try
            {
                using (var con = GetSqlConnGestionProyectos())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("usp_listar_permisos_Lectura", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@idRol", idRol);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        lista.Add(dr.GetString(0));
                    }

                    dr.Close(); con.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return lista;
        }

        public IEnumerable<string> ListarPermisosEscritura(int idUsuario, int idRol)
        {
            List<string> lista = new List<string>();

            using (var con = GetSqlConnGestionProyectos())
            {
                con.Open();
                
                SqlCommand cmd = new SqlCommand("usp_listar_permisos_Escritura", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                cmd.Parameters.AddWithValue("@idRol", idRol);
                cmd.CommandType = CommandType.StoredProcedure;

                
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(dr.GetString(0));
                }

                dr.Close(); con.Close();
            }

            return lista;
        }

        public IEnumerable<Usuario_BE> ListarSponsors()
        {
            List<Usuario_BE> lista = new List<Usuario_BE>();

            using (var con = GetSqlConnGestionProyectos())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_listarUsuariosSponsor", con);
                cmd.CommandType = CommandType.StoredProcedure;
                
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Usuario_BE reg = new Usuario_BE()
                    {
                        IdUsuario = dr.GetInt32(0),
                        Alias = dr.GetString(1)
                    };

                    lista.Add(reg);
                }

                dr.Close(); con.Close();
            }

            return lista;
        }

        public IEnumerable<Usuario_BE> ListarTeamMembers()
        {
            List<Usuario_BE> lista = new List<Usuario_BE>();

            using (var con = GetSqlConnGestionProyectos())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_team_member", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Usuario_BE reg = new Usuario_BE()
                    {
                        IdUsuario = dr.GetInt32(0),
                        Alias = dr.GetString(1)
                    };

                    lista.Add(reg);
                }

                dr.Close(); con.Close();
            }

            return lista;
        }

        public IEnumerable<Usuario_BE> ListarUsuarios()
        {
            List<Usuario_BE> lista = new List<Usuario_BE>();

            using (var con = GetSqlConnGestionProyectos())
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("usp_listarUsuario", con);
                cmd.CommandType = CommandType.StoredProcedure;

               
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Usuario_BE reg = new Usuario_BE();

                    reg.IdUsuario = dr.GetInt32(0);
                    reg.Correo = dr.GetString(1);
                    reg.Alias = dr.GetString(2);
                    reg.NombreArea = dr.GetString(3);
                    reg.IdArea = dr.GetInt32(4);
                    if (!dr.IsDBNull(5))
                        reg.Iniciales = dr.GetString(5);


                    lista.Add(reg);
                }

                dr.Close(); con.Close();
            }

            return lista;
        }    
        public int ValidarRol(int id)
        {

            using (var con = GetSqlConnGestionProyectos())
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("validacion", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idusuario", id);
                cmd.Parameters.Add("@respuesta", SqlDbType.Int);
                cmd.Parameters["@respuesta"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                con.Close();
                return (int)cmd.Parameters["@respuesta"].Value;

            }
        }

        public Usuario_BE BuscarUsuario(int idUsuario)
        {
            return ListarUsuarios().Where(usu => usu.IdUsuario == idUsuario).FirstOrDefault();
        }

        public void EditarUsuario(Usuario_BE reg)
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
                    SqlCommand cmd = new SqlCommand("usp_editarUsuario", con, tr);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idUsuario", reg.IdUsuario);
                    cmd.Parameters.AddWithValue("@correo", reg.Correo);
                    cmd.Parameters.AddWithValue("@alias", reg.Alias);
                    cmd.Parameters.AddWithValue("@idArea", reg.IdArea);
                    if (reg.Iniciales != null)
                        cmd.Parameters.AddWithValue("@iniciales", reg.Iniciales.ToUpper());
                    else
                        cmd.Parameters.AddWithValue("@iniciales", DBNull.Value);
                    cmd.Parameters.AddWithValue("@idUsuarioModificacion", usu.IdUsuario);
                    cmd.Parameters.AddWithValue("@fechaModificacion", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    rs = cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("usp_resetearRoles", con, tr);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idUsuario", reg.IdUsuario);
                    rs = cmd.ExecuteNonQuery();

                    foreach (var item in reg.ListaRoles)
                    {
                        cmd = new SqlCommand("usp_registrarRolUsuario", con, tr);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idUsuario", reg.IdUsuario);
                        cmd.Parameters.AddWithValue("@idRol", item);
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

        public int ObtenerNuevoID()
        {
            int nuevoId = 0;
            using (var con = GetSqlConnGestionProyectos())
            {
                SqlCommand cmd = new SqlCommand("select max(idusuario) from Usuario", con);
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

        public void RegistrarUsuario(Usuario_BE reg)
        {
            Usuario_BE usu = HttpContext.Current.Session["usuario"] != null ? HttpContext.Current.Session["usuario"] as Usuario_BE : new Usuario_BE();

            int rs = 0;
            int nuevoId = ObtenerNuevoID();
            using (var con = GetSqlConnGestionProyectos())
            {
                con.Open();
                
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    // Definir el SqlCommand y el tipo de instruccion
                    SqlCommand cmd = new SqlCommand("usp_registrarUsuario", con, tr);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@correo", reg.Correo);
                    cmd.Parameters.AddWithValue("@alias", reg.Alias);
                    cmd.Parameters.AddWithValue("@idArea", reg.IdArea);
                    if (reg.Iniciales != null)
                        cmd.Parameters.AddWithValue("@iniciales", reg.Iniciales.ToUpper());
                    else
                        cmd.Parameters.AddWithValue("@iniciales", DBNull.Value);
                    cmd.Parameters.AddWithValue("@idUsuarioCreacion", usu.IdUsuario);
                    cmd.Parameters.AddWithValue("@fechaCreacion", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    rs = cmd.ExecuteNonQuery();

                    foreach (var item in reg.ListaRoles)
                    {
                        cmd = new SqlCommand("usp_registrarRolUsuario", con, tr);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idUsuario", nuevoId);
                        cmd.Parameters.AddWithValue("@idRol", item);
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

        public List<int> ListarRolesUsuario(int idUsuario)
        {
            List<int> lista = new List<int>();

            using (var con = GetSqlConnGestionProyectos())
            {
                SqlCommand cmd = new SqlCommand("usp_listarRolesUsuario", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    int idRol = dr.GetInt32(0);

                    lista.Add(idRol);
                }

                dr.Close(); con.Close();
            }

            return lista;
        }
    }
}
