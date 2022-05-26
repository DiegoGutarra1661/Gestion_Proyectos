﻿using System;
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
        
        public void IniciarSesion(Usuario_BE usuario)
        {
            HttpContext.Current.Session["usuario"] = usuario;
            HttpContext.Current.Session.Timeout = 20;
        }
        public void CerrarSesion()
        {
            HttpContext.Current.Session.Abandon();
        }
    }
}
