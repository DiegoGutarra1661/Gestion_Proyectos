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
    public class Area_DA: Conexion
    {
        public IEnumerable<Area_BE> ListarAreas()
        {
            List<Area_BE> lista = new List<Area_BE>();

            using (var con = GetSqlConnGestionProyectos())
            {
                SqlCommand cmd = new SqlCommand("usp_ListarAreas", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Area_BE reg = new Area_BE()
                    {
                        IdArea = dr.GetInt32(0),
                        Descripcion = dr.GetString(1)
                    };

                    lista.Add(reg);
                }

                dr.Close(); con.Close();
            }

            return lista;
        }
    }
}
