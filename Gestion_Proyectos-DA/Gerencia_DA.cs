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
    public class Gerencia_DA: Conexion
    {
        public IEnumerable<Gerencia_BE> ListarGerencias()
        {
            List<Gerencia_BE> lista = new List<Gerencia_BE>();

            using (var con = GetSqlConnGestionProyectos())
            {
                SqlCommand cmd = new SqlCommand("select * from Gerencia", con);
                // cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Gerencia_BE reg = new Gerencia_BE()
                    {
                        IdGerencia = dr.GetInt32(0),
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
