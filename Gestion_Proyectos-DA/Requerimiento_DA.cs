using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gestion_Proyectos_BE;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Gestion_Proyectos_DA
{
    public class Requerimiento_DA : Conexion
    {
        Usuario_BE usu = HttpContext.Current.Session["usuario"] != null ? HttpContext.Current.Session["usuario"] as Usuario_BE : new Usuario_BE();

        public string ObtenerIDRequerimiento()
        {
            string id = "";
            using (var con = GetSqlConnGestionProyectos())
            {
                SqlCommand cmd = new SqlCommand("SELECT [dbo].[AutogeneraCodigoRequerimiento]()", con);
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

        public int validarRegistroRequerimiento(string idRequerimiento)
        {
            int validar = 0;
            using (var con = GetSqlConnGestionProyectos())
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("usp_validarRegistroRequerimiento", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idRequerimiento", idRequerimiento);
            

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    validar = dr.GetInt32(0);
                }

                dr.Close(); con.Close();
            }

            return validar;
        }

       

        public void RegistrarRequerimiento(Requerimiento_BE reg, HttpPostedFileBase[] archivos)
        {

            var idRequerimiento = ObtenerIDRequerimiento().Trim();

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
                        if(archivos[0] != null)
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
                    }


                    EnviarCorreo("shirley.cordova@adexperu.org.pe","angello.gordillo@adexperu.org.pe", "ADEX - Nuevo Requerimiento", "Estimados," + "<br/>" + "<br/>" +
                    "Se recibió una nueva Solictud de Requerimiento " + idRequerimiento + " con título " + reg.Titulo + " por parte de " + BuscarUsuarioCorreo(reg.Solicitante).Alias + " - " + BuscarUsuarioCorreo(reg.Solicitante).NombreArea + ".");
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
        public Usuario_BE BuscarUsuarioCorreo(int idUsuario)
        {
            Usuario_BE reg = null;

            using (var con = GetSqlConnGestionProyectos())
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("usp_buscarUsuario", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    reg = new Usuario_BE();

                    reg.Alias = dr.GetString(0);
                    reg.NombreArea = dr.GetString(1);

                }
                dr.Close(); con.Close();
            }

            return reg;
        }


      

        public IEnumerable<Requerimiento_BE> ListarRequerimientos()
        {
            List<Requerimiento_BE> lista = new List<Requerimiento_BE>();

            using (var con = GetSqlConnGestionProyectos())
            {
                SqlCommand cmd = new SqlCommand("usp_listarRequerimiento", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Requerimiento_BE reg = new Requerimiento_BE();
                    reg.IdRequerimiento = dr.GetString(0);
                    reg.Solicitante = dr.GetInt32(1);
                    reg.DescripcionUsuario = dr.GetString(2);
                    reg.DescripcionGerencia = dr.GetString(3);
                    reg.DescripcionPresupuesto = dr.GetString(4);
                    reg.Titulo = dr.GetString(5);
                    reg.Alcance = dr.GetString(6);
                    reg.Prioridad = dr.GetString(7);
                    reg.Resumen = dr.GetString(8);
                    reg.FechaFormateada = dr.GetString(9);
                    reg.DescripcionEstado = dr.GetString(10);
                    reg.CorreoUsuario = dr.GetString(11);

                    lista.Add(reg);

                }
                dr.Close(); con.Close();
            }

            return lista;
        }


        public IEnumerable<Adjunto_BE> ListarAdjuntosRequerimiento(string id)
        {
            List<Adjunto_BE> lista = new List<Adjunto_BE>();

            using (var con = GetSqlConnGestionProyectos())
            {
                SqlCommand cmd = new SqlCommand("usp_buscarAdjuntosRequerimiento", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idRequerimiento", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Adjunto_BE reg = new Adjunto_BE();
                    reg.IdAdjunto = dr.GetInt32(1);
                    reg.RutaAdjunto = dr.GetString(2);
                    reg.NombreAdjunto = dr.GetString(3);

                    lista.Add(reg);

                }
                dr.Close(); con.Close();
            }

            return lista;
        }

        public Requerimiento_BE BuscarRequerimiento(string idRequerimiento)
        {
            return ListarRequerimientos().Where(req => req.IdRequerimiento == idRequerimiento).FirstOrDefault();
        }

        public void RegistrarRequerimientoProyecto(string idRequerimiento, int estado)
        {
            var reg = BuscarRequerimiento(idRequerimiento);

            using (var con = GetSqlConnGestionProyectos())
            {

                con.Open();
                SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    if (estado == 1)
                    {
                        cmd = new SqlCommand("usp_registrarRequerimientoProyecto", con, tr);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@nombre", reg.Titulo);
                        cmd.Parameters.AddWithValue("@descripcion", reg.Alcance);
                        cmd.Parameters.AddWithValue("@idUsuarioSponsor", reg.Solicitante);
                        cmd.Parameters.AddWithValue("@idUsuarioCreacion", usu.IdUsuario);
                        cmd.Parameters.AddWithValue("@idRequerimiento", reg.IdRequerimiento);

                        cmd.ExecuteNonQuery();
                    }

                    cmd = new SqlCommand("usp_actualizarEstadoRequerimiento", con, tr);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idRequerimiento",idRequerimiento);
                    cmd.Parameters.AddWithValue("@estado",estado);
                    cmd.ExecuteNonQuery();



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

        public void EnviarCorreo(string tocorreo1,string tocorreo2, string subject, string cuerpo)
        {

            // Creación y configuración del cliente para el envío de las encuestas
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            using (SmtpClient client = new SmtpClient("smtp.office365.com", 587))
            {
                client.EnableSsl = true;
                //Cambiar por otro correo :S
                client.Credentials = new System.Net.NetworkCredential("servicedesk.ti@adexperu.org.pe", "@dex2021");

                // Creación de los atributos del correo

                MailMessage message = new MailMessage();
                message.From = new MailAddress("servicedesk.ti@adexperu.org.pe", String.Empty, System.Text.Encoding.UTF8);
                message.To.Add(tocorreo1);
                message.To.Add(tocorreo2);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = cuerpo + "<br><br> " +
                                        "¡Vamos por más! <br>" +
                                        "Soluciones TI";
                message.BodyEncoding = System.Text.Encoding.UTF8;

                message.SubjectEncoding = System.Text.Encoding.UTF8;

                client.Send(message);
                client.Dispose();
                Console.WriteLine("Se envió un correo");
            }

        }
    }
}
