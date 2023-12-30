using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Compra
    {
        private CD_Compra objetoCD = new CD_Compra();
        public int ObtenerCorrelativo() {
            return objetoCD.ObtenerCorrelativo();
        }
        public bool Registrar(Compra obj, DataTable DetalleCompra, out string Mensaje)
        {
            return objetoCD.Registrar(obj, DetalleCompra, out Mensaje);
        }
        public Compra ObtenerCompra(string numero)
        {
            Compra obj = objetoCD.ObtenerCompra(numero);

            if (obj != null)
            {
                List<Detalle_Compra> lista = objetoCD.ObtenerDetalleCompra(obj.IdCompra);

                obj.oListaDetalleCompra = lista;
            }
            return obj;
        }
    }
}
