using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Cliente
    {
        private CD_Cliente objetoCD = new CD_Cliente();

        public List<Cliente> Listar()
        {
            return objetoCD.Listar();
        }
        public int Registrar(Cliente obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.Documento == string.Empty)
            {
                Mensaje += "Ingrese el documento del Cliente\n";
            }
            if (obj.NombreCompleto == string.Empty)
            {
                Mensaje += "Ingrese el nombre del Cliente\n";
            }
            if(obj.Telefono == string.Empty)
            {
                Mensaje += "Ingrese el telefono del Cliente\n";
            }
            if (obj.Telefono == string.Empty)
            {
                Mensaje += "Ingrese la clave del Cliente\n";
            }

            if (Mensaje == string.Empty)
            {
                return objetoCD.Registrar(obj, out Mensaje);
            }
            else
            {
                return 0;
            }
        }
        public bool Editar(Cliente obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.Documento == string.Empty)
            {
                Mensaje += "Ingrese el documento del Cliente\n";
            }
            if (obj.NombreCompleto == string.Empty)
            {
                Mensaje += "Ingrese el nombre del Cliente\n";
            }
            if (obj.Telefono == string.Empty)
            {
                Mensaje += "Ingrese el telefono del Cliente\n";
            }
            if (obj.Telefono == string.Empty)
            {
                Mensaje += "Ingrese la clave del Cliente\n";
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
        public bool Eliminar(Cliente obj, out string Mensaje)
        {
            return objetoCD.Eliminar(obj, out Mensaje);
        }
    }
}
