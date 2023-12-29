using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Proveedor
    {
        private CD_Proveedor objetoCD = new CD_Proveedor();

        public List<Proveedor> Listar()
        {
            return objetoCD.Listar();
        }
        public int Registrar(Proveedor obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.Documento == string.Empty)
            {
                Mensaje += "Ingrese el documento del Proveedor\n";
            }
            if (obj.RazonSocial == string.Empty)
            {
                Mensaje += "Ingrese el nombre del Proveedor\n";
            }
            if (obj.Correo == string.Empty)
            {
                Mensaje += "Ingrese el correo del Proveedor\n";
            }
            if (obj.Telefono == string.Empty)
            {
                Mensaje += "Ingrese el telefono del Proveedor\n";
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
        public bool Editar(Proveedor obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.Documento == string.Empty)
            {
                Mensaje += "Ingrese el documento del Proveedor\n";
            }
            if (obj.RazonSocial == string.Empty)
            {
                Mensaje += "Ingrese el nombre del Proveedor\n";
            }
            if (obj.Correo == string.Empty)
            {
                Mensaje += "Ingrese el correo del Proveedor\n";
            }
            if (obj.Telefono == string.Empty)
            {
                Mensaje += "Ingrese el telefono del Proveedor\n";
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
        public bool Eliminar(Proveedor obj, out string Mensaje)
        {
            return objetoCD.Eliminar(obj, out Mensaje);
        }
    }
}
