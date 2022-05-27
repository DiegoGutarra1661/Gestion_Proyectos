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
    public class Permiso_DA: Conexion
    {
        public IEnumerable<Permiso_BE> ListarPermisos()
        {
            List<Permiso_BE> lista = new List<Permiso_BE>();

            using (var con = GetSqlConnGestionProyectos())
            {
                SqlCommand cmd = new SqlCommand("usp_listarPermisos", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Permiso_BE reg = new Permiso_BE()
                    {
                        IdPermiso = dr.GetInt32(0),
                        Nombre = dr.GetString(1)
                    };

                    lista.Add(reg);
                }

                dr.Close(); con.Close();
            }

            return lista;
        }
    }
}
