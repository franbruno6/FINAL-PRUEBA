using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CN_Permiso
    {
        private CD_Permiso objetoCD = new CD_Permiso();

        public List<Permiso> Listar(int idUsuario)
        {
            return objetoCD.Listar(idUsuario);
        }
    }
}
