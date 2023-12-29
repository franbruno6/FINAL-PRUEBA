using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Negocio
    {
        private CD_Negocio objetoCD = new CD_Negocio();

        public Negocio ObtenerDatos()
        {
            return objetoCD.ObtenerDatos();
        }
        public bool Registrar(Negocio obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.Nombre == string.Empty)
            {
                Mensaje += "Ingrese el nombre del Negocio\n";
            }
            if (obj.Ruc == string.Empty)
            {
                Mensaje += "Ingrese el RUC del Negocio\n";
            }
            if (obj.Direccion == string.Empty)
            {
                Mensaje += "Ingrese la direccion del Negocio\n";
            }

            if (Mensaje == string.Empty)
            {
                return objetoCD.GuardarDatos(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }
        public byte[] ObtenerLogo(out bool obtenido)
        {
            return objetoCD.ObtenerLogo(out obtenido);
        }
        public bool ActualizarLogo(byte[] logo, out string mensaje)
        {
            return objetoCD.ActualizarLogo(logo, out mensaje);
        }
    }
}
