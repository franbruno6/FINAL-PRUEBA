using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuario
    {
        private CD_Usuario objetoCD = new CD_Usuario();

        public List<Usuario> Listar()
        {
            return objetoCD.Listar();
        }

        public int Registrar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if(obj.Documento == string.Empty)
            {
                Mensaje += "Ingrese el documento del usuario\n";
            }
            if(obj.NombreCompleto == string.Empty)
            {
                Mensaje += "Ingrese el nombre del usuario\n";
            }
            if(obj.Clave == string.Empty)
            {
                Mensaje += "Ingrese la clave del usuario\n";
            }

            if(Mensaje == string.Empty)
            {
                return objetoCD.Registrar(obj, out Mensaje);
            }
            else
            {
                return 0;
            }
        }

        public bool Editar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.Documento == string.Empty)
            {
                Mensaje += "Ingrese el documento del usuario\n";
            }
            if (obj.NombreCompleto == string.Empty)
            {
                Mensaje += "Ingrese el nombre del usuario\n";
            }
            if (obj.Clave == string.Empty)
            {
                Mensaje += "Ingrese la clave del usuario\n";
            }

            if (Mensaje == string.Empty)
            {
                return objetoCD.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(Usuario obj, out string Mensaje)
        {
            return objetoCD.Eliminar(obj, out Mensaje);
        }
    }
}
