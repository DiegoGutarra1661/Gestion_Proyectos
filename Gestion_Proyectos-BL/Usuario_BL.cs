using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using Gestion_Proyectos_BE;
using Gestion_Proyectos_DA;
using System.DirectoryServices.AccountManagement;
using System.Web;

namespace Gestion_Proyectos_BL
{
    public class Usuario_BL
    {
        private readonly Usuario_DA _usuarioDA;

        public Usuario_BL()
        {
            _usuarioDA = new Usuario_DA();
        }



        public Usuario_BE Login(string correo)
        {
            return _usuarioDA.Login(correo);
        }

        public bool EstaEnAD(string correo, string clave)
        {
            bool res = false;
            string dominio = ConfigurationManager.AppSettings["DomainActiveDirectory"];

            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, dominio))
            {
              
                res = pc.ValidateCredentials(correo, clave);
            }

            return res;
        }
        
        public void IniciarSesion(Usuario_BE usuario, IEnumerable<string> permisosLectura, IEnumerable<string> permisosEscritura)
        {
            HttpContext.Current.Session["usuario"] = usuario;
            HttpContext.Current.Session["permisosLectura"] = permisosLectura;
            HttpContext.Current.Session["permisosEscritura"] = permisosEscritura;
            HttpContext.Current.Session["filtrosGerencia"] = new List<int>();
            HttpContext.Current.Session["filtrosMember"] = new List<int>();
            HttpContext.Current.Session["filtrosEstado"] = new List<string>();
            HttpContext.Current.Session.Timeout = 480;
        }
        public void CerrarSesion()
        {
            HttpContext.Current.Session.Abandon();
        }

        public IEnumerable<string> GetPermisosLectura(int idUsuario, int idRol)
        {
            try
            {
                var permisos = _usuarioDA.ListarPermisosLectura(idUsuario,idRol);
                return permisos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener permisos de lectura ==> {ex.Message}");
            }
        }
        public IEnumerable<string> GetPermisosEscritura(int idUsuario, int idRol)
        {
            try
            {
                var permisos = _usuarioDA.ListarPermisosEscritura(idUsuario, idRol);
                return permisos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener permisos de escritura ==> {ex.Message}");
            }
        }

        public IEnumerable<Usuario_BE> GetMembers()
        {
            try
            {
                var members = _usuarioDA.ListarTeamMembers();
                return members;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener team members ==> {ex.Message}");
            }
        }

        public int GetRol(int id)
        {
            try
            {
                var rol = _usuarioDA.ValidarRol(id);
                return rol;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener rol ==> {ex.Message}");
            }
        }


    }
}
