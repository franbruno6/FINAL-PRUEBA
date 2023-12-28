using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Categoria
    {
        private CD_Categoria objetoCD = new CD_Categoria();

        public List<Categoria> Listar()
        {
            return objetoCD.Listar();
        }

        public int Registrar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.Descripcion == string.Empty)
            {
                Mensaje += "Ingrese la descripcion del Categoria\n";
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

        public bool Editar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.Descripcion == string.Empty)
            {
                Mensaje += "Ingrese la descripcion del Categoria\n";
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

        public bool Eliminar(Categoria obj, out string Mensaje)
        {
            return objetoCD.Eliminar(obj, out Mensaje);
        }
    }
}
