using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Gestion_Proyectos_DA
{
    public class Conexion
    {
        private readonly string _cadenaGestionProyectos;
        public Conexion()
        {
            
            _cadenaGestionProyectos = ConfigurationManager.ConnectionStrings["conexGestionProyecto"].ConnectionString;
        }

        public SqlConnection GetSqlConnGestionProyectos()
        {
            return new SqlConnection(_cadenaGestionProyectos);
        }
    }
}
