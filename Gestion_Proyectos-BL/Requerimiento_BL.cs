using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Gestion_Proyectos_BE;
using Gestion_Proyectos_DA;


namespace Gestion_Proyectos_BL
{
    public class Requerimiento_BL
    {
        private readonly Requerimiento_DA _requerimientoDA;
        private readonly Proyecto_DA _proyectoDA;

        public Requerimiento_BL()
        {
            _requerimientoDA = new Requerimiento_DA();
            _proyectoDA = new Proyecto_DA();
        }

        public string RegistrarRequerimiento(Requerimiento_BE reg, HttpPostedFileBase[] archivos)
        {
            string mensaje = "";
            try
            {
                _requerimientoDA.RegistrarRequerimiento(reg, archivos);
                mensaje = "Se envió su Solicitud de Requerimiento satisfactoriamente";
            }
            catch (Exception ex)
            {
                mensaje = "Error en registrar requerimiento: " + ex.Message;

            }
            return mensaje;
        }

        public IEnumerable<Requerimiento_BE> GetRequerimientos()
        {
            try
            {
                var requerimientos = _requerimientoDA.ListarRequerimientos();
                return requerimientos;         
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar requerimientos ==> {ex.Message}");
            }
        }

        public IEnumerable<Adjunto_BE> GetAdjuntos(string id)
        {
            try
            {
                var adjuntos = _requerimientoDA.ListarAdjuntosRequerimiento(id);


                return adjuntos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en el método Busqueda adjuntos ==> {ex.Message}");
            }
        }

        public string RegistrarRequerimientoProyecto(string codigo, int estado,string cuerpo)
        {
            string mensaje = "";
            try
            {

                
                if (_requerimientoDA.validarRegistroRequerimiento(codigo) == 1)
                {
                    mensaje = "El requerimiento ha sido aprobado y se encuetra registrado en los proyectos";
                }
                else
                {
                    _requerimientoDA.RegistrarRequerimientoProyecto(codigo, estado);
                    if (estado == 1)
                    {
                        mensaje = "Se actualizó el estado del requerimiento a Aprobado";
                    }
                    else if(estado==2)
                    {
                        _proyectoDA.EnviarCorreo(_requerimientoDA.BuscarRequerimiento(codigo).CorreoUsuario,"Requerimiento rechazado",cuerpo);
                    }

                }

                

            }
            catch
            {
                mensaje = "Error en registrar";

            }
            return mensaje;
        }

        public Requerimiento_BE BuscarRequerimiento(string id)
        {
            try
            {
                var requerimiento = _requerimientoDA.BuscarRequerimiento(id);

                if (requerimiento == null)
                {
                    throw new Exception("Error ==> No se encontro requerimiento");
                }

                return requerimiento;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en el método Buscar requerimiento ==> {ex.Message}");
            }
        }

        
    }
}
