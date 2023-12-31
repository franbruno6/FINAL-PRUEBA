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
    public class CN_Venta
    {
        private CD_Venta objetocd = new CD_Venta();
        public int GenerarCorrelativo()
        {
            return objetocd.ObtenerCorrelativo();
        }
        public bool RestarStock(int idProducto, int cantidad)
        {
            return objetocd.RestarStock(idProducto, cantidad);
        }
        public bool SumarStock(int idProducto, int cantidad)
        {
            return objetocd.SumarStock(idProducto, cantidad);
        }
        public bool Registrar(Venta obj, DataTable DetalleVenta, out string mensaje)
        {
            return objetocd.Registrar(obj, DetalleVenta, out mensaje);
        }
    }
}
