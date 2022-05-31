﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gestion_Proyectos_BE;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.IO;

namespace Gestion_Proyectos_DA
{
    public class Requerimiento_DA: Conexion
    {
        public string ObtenerIDRequerimiento()
        {
            string id = "";
            using (var con = GetSqlConnGestionProyectos())
            {
                SqlCommand cmd = new SqlCommand("select max(idRequerimiento) from Requerimiento", con);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    id = dr.GetString(0);
                }

                dr.Close(); con.Close();
            }

            return id;
        }

        public void RegistrarRequerimiento(Requerimiento_BE reg, HttpPostedFileBase[] archivos)
        {

            var idRequerimiento = ObtenerIDRequerimiento();

            using (var con = GetSqlConnGestionProyectos())
            {

                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                try
                {

                    SqlCommand cmd = new SqlCommand("usp_registrarRequerimiento", con, tr);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@solicitante", reg.Solicitante);
                    cmd.Parameters.AddWithValue("@gerencia", reg.Gerencia);
                    cmd.Parameters.AddWithValue("@presupuesto", reg.Presupuesto);
                    cmd.Parameters.AddWithValue("@titulo", reg.Titulo);
                    cmd.Parameters.AddWithValue("@alcance", reg.Alcance);
                    cmd.Parameters.AddWithValue("@prioridad", reg.Prioridad);
                    cmd.Parameters.AddWithValue("@resumen", reg.Resumen);
                    cmd.Parameters.AddWithValue("@fechaReunion", reg.FechaReunion);

                    cmd.ExecuteNonQuery();
                    string path = HttpContext.Current.Server.MapPath("~/Archivos/" + idRequerimiento + "/");
                    if (archivos != null)
                    {
                        
                        var contador = 0;
                        foreach (var archivo in archivos)
                        {
                            contador++;
                             var newFileName = string.Format("{0}-{1}-{2}{3}"
                            , "Requerimiento"
                            , idRequerimiento
                            , contador
                            , Path.GetExtension(archivo.FileName));

                            cmd = new SqlCommand("usp_registrarAdjuntoRequerimiento", con, tr);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idRequerimiento", idRequerimiento);
                            cmd.Parameters.AddWithValue("@rutaAdjunto", "~/Archivos/" + idRequerimiento + "/" + newFileName);
                            cmd.Parameters.AddWithValue("@nombreAdjunto", newFileName);
                            cmd.ExecuteNonQuery();
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            archivo.SaveAs(path + Path.GetFileName(newFileName));
                        }

                       
                    }


                    tr.Commit();
                }
                catch (SqlException ex)
                {

                    con.Close();
                    tr.Rollback();
                    throw ex;

                }

            }

        }
    }
}