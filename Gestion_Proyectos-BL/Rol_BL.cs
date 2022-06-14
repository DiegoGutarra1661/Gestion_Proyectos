using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gestion_Proyectos_BE;
using Gestion_Proyectos_DA;

namespace Gestion_Proyectos_BL
{
    public class Rol_BL
    {
        private readonly Rol_DA _rolDA;

        public Rol_BL()
        {
            _rolDA = new Rol_DA();
        }

        public IEnumerable<Rol_BE> GetRoles()
        {
            try
            {
                var roles = _rolDA.ListarRoles();

                return roles;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar roles ==> {ex.Message}");
            }
        }

        public List<int> GetPermisoRoles(int idUsuario)
        {
            try
            {
                var permisoRoles = _rolDA.ListarPermisosRol(idUsuario);

                return permisoRoles;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar roles ==> {ex.Message}");
            }
        }

        public Rol_BE BuscarRol(int id)
        {
            try
            {
                var rol = _rolDA.BuscarRol(id);

                if (rol == null)
                {
                    throw new Exception("Error ==> No se encontro roles");
                }

                return rol;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en el método Buscar rol por codigo ==> {ex.Message}");
            }
        }

        public string EditarRol(Rol_BE reg)
        {
            string mensaje = "";
            try
            {
                _rolDA.EditarRol(reg);

                mensaje = "Rol '" + reg.Nombre + "' actualizado correctamente";
            }
            catch (Exception ex)
            {
                mensaje = "Error en editar rol: " + ex.Message;

            }
            return mensaje;
        }

        public string RegistrarRol(Rol_BE reg)
        {
            string mensaje = "";
            try
            {
                _rolDA.RegistrarRol(reg);

                mensaje = "Rol '" + reg.Nombre + "' registrado correctamente";
            }
            catch (Exception ex)
            {
                mensaje = "Error en registrar rol: " + ex.Message;

            }
            return mensaje;
        }

    }
}
