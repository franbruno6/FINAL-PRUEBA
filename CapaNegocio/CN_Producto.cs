using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Producto
    {
        private CD_Producto objetoCD = new CD_Producto();

        public List<Producto> Listar()
        {
            return objetoCD.Listar();
        }
        public int Registrar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.Codigo == string.Empty)
            {
                Mensaje += "Ingrese el codigo del producto\n";
            }
            if (obj.Nombre == string.Empty)
            {
                Mensaje += "Ingrese el nombre del producto\n";
            }
            if (obj.Descripcion == string.Empty)
            {
                Mensaje += "Ingrese la descripcion del producto\n";
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
        public bool Editar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.Codigo == string.Empty)
            {
                Mensaje += "Ingrese el codigo del producto\n";
            }
            if (obj.Nombre == string.Empty)
            {
                Mensaje += "Ingrese el nombre del producto\n";
            }
            if (obj.Descripcion == string.Empty)
            {
                Mensaje += "Ingrese la descripcion del producto\n";
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
        public bool Eliminar(Producto obj, out string Mensaje)
        {
            return objetoCD.Eliminar(obj, out Mensaje);
        }
    }
}
