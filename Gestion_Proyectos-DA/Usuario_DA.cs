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

    }
}
