using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Venta
    {
        public int ObtenerCorrelativo()
        {
            int idcorrelativo = 0;

            using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.Append("select count(*) + 1 from Venta");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.CommandType = System.Data.CommandType.Text;

                    oConexion.Open();

                    idcorrelativo = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch
                {
                    idcorrelativo = 0;
                }
            }
            return idcorrelativo;
        }
        public bool RestarStock(int idProducto, int cantidad)
        {
            bool respuesta = true;

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update Producto set Stock = Stock - @Cantidad where Id = @IdProducto");
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@IdProducto", idProducto);
                    cmd.CommandType = System.Data.CommandType.Text;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
        public bool SumarStock(int idProducto, int cantidad)
        {
            bool respuesta = true;

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update Producto set Stock = Stock + @Cantidad where Id = @IdProducto");
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@IdProducto", idProducto);
                    cmd.CommandType = System.Data.CommandType.Text;

                    oconexion.Open();

                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
                catch
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
        public bool Registrar(Venta obj, DataTable DetalleVenta, out string Mensaje)
        {
            bool Respuesta = false;
            Mensaje = string.Empty;

            using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarVenta", oConexion);
                    //PARAMETROS DE ENTRADA
                    cmd.Parameters.AddWithValue("IdUsuario", obj.oUsuario.IdUsuario);
                    cmd.Parameters.AddWithValue("TipoDocumento", obj.TipoDocumento);
                    cmd.Parameters.AddWithValue("NumeroDocumento", obj.NumeroDocumento);
                    cmd.Parameters.AddWithValue("DocumentoCliente", obj.DocumentoCliente);
                    cmd.Parameters.AddWithValue("NombreCliente", obj.NombreCliente);
                    cmd.Parameters.AddWithValue("MontoPago", obj.MontoPago);
                    cmd.Parameters.AddWithValue("MontoCambio", obj.MontoCambio);
                    cmd.Parameters.AddWithValue("MontoTotal", obj.MontoTotal);
                    cmd.Parameters.AddWithValue("DetalleVenta", DetalleVenta);
                    //PARAMETROS DE SALIDA
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;
                    //TIPO DE COMANDO
                    cmd.CommandType = CommandType.StoredProcedure;
                    //ABRIR CONEXION
                    oConexion.Open();
                    //EJECUTAR COMANDO
                    cmd.ExecuteNonQuery();
                    //OBTENER RESPUESTA
                    Respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
                catch (Exception ex)
                {
                    Respuesta = false;
                    Mensaje = ex.Message;
                }
            }
            return Respuesta;
        }
        public Venta ObtenerVenta(string numero)
        {
            Venta obj = new Venta();

            using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select Venta.Id,DocumentoCliente,NombreCliente,TipoDocumento,NroDocumento,MontoPago,MontoCambio,MontoTotal,");
                    query.AppendLine("Usuario.NombreCompleto,convert(char(10), Venta.FechaCreacion, 103)[FechaCreacion] ");
                    query.AppendLine("from Venta ");
                    query.AppendLine("inner join Usuario on Usuario.Id = Venta.IdUsuario ");
                    query.AppendLine("where Venta.NroDocumento = @NumeroDocumento");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.Parameters.AddWithValue("NumeroDocumento", numero);
                    cmd.CommandType = System.Data.CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            obj = new Venta()
                            {
                                IdVenta = Convert.ToInt32(dr["Id"]),
                                oUsuario = new Usuario()
                                {
                                    NombreCompleto = dr["NombreCompleto"].ToString(),
                                },
                                DocumentoCliente = dr["DocumentoCliente"].ToString(),
                                NombreCliente = dr["NombreCliente"].ToString(),
                                TipoDocumento = dr["TipoDocumento"].ToString(),
                                NumeroDocumento = dr["NroDocumento"].ToString(),
                                MontoPago = Convert.ToDecimal(dr["MontoPago"]),
                                MontoCambio = Convert.ToDecimal(dr["MontoCambio"]),
                                MontoTotal = Convert.ToDecimal(dr["MontoTotal"]),
                                FechaCreacion = dr["FechaCreacion"].ToString()

                                //IdVenta = dr["Id"] != null ? Convert.ToInt32(dr["Id"]) : 0,
                                //oUsuario = new Usuario()
                                //{
                                //    NombreCompleto = dr["NombreCompleto"] != null ? dr["NombreCompleto"].ToString() : string.Empty,
                                //},
                                //DocumentoCliente = dr["DocumentoCliente"] != null ? dr["DocumentoCliente"].ToString() : string.Empty,
                                //NombreCliente = dr["NombreCliente"] != null ? dr["NombreCliente"].ToString() : string.Empty,
                                //TipoDocumento = dr["TipoDocumento"] != null ? dr["TipoDocumento"].ToString() : string.Empty,
                                //NumeroDocumento = dr["NroDocumento"] != null ? dr["NroDocumento"].ToString() : string.Empty,
                                //MontoPago = dr["MontoPago"] != null ? Convert.ToDecimal(dr["MontoPago"]) : 0,
                                //MontoCambio = dr["MontoCambio"] != null ? Convert.ToDecimal(dr["MontoCambio"]) : 0,
                                //MontoTotal = dr["MontoTotal"] != null ? Convert.ToDecimal(dr["MontoTotal"]) : 0,
                                //FechaCreacion = dr["FechaCreacion"] != null ? dr["FechaCreacion"].ToString() : string.Empty,
                            };

                        }
                    }
                }
                catch
                {
                    obj = null;
                }
            }
            return obj;
        }
        public List<Detalle_Venta> ObtenerDetalleVenta(int idVenta)
        {
            List<Detalle_Venta> lista = new List<Detalle_Venta>();

            using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select Producto.Nombre,Detalle_Venta.Cantidad,Detalle_Venta.PrecioVenta,Detalle_Venta.SubTotal ");
                    query.AppendLine("from Detalle_Venta ");
                    query.AppendLine("inner join Producto on Producto.Id = Detalle_Venta.IdProducto ");
                    query.AppendLine("where Detalle_Venta.IdVenta = @IdVenta");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.Parameters.AddWithValue("IdVenta", idVenta);
                    cmd.CommandType = System.Data.CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Detalle_Venta()
                            {
                                oProducto = new Producto()
                                {
                                    Nombre = dr["Nombre"].ToString(),
                                },
                                Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]),
                                SubTotal = Convert.ToDecimal(dr["SubTotal"])
                                //oProducto = new Producto()
                                //{
                                //    Nombre = dr["Nombre"] != null ? dr["Nombre"].ToString() : string.Empty,
                                //},
                                //Cantidad = dr["Cantidad"] != null ? Convert.ToInt32(dr["Cantidad"]) : 0,
                                //PrecioVenta = dr["PrecioVenta"] != null ? Convert.ToDecimal(dr["PrecioVenta"]) : 0,
                                //SubTotal = dr["SubTotal"] != null ? Convert.ToDecimal(dr["SubTotal"]) : 0,
                            });
                        }
                    }
                }
                catch
                {
                    lista = new List<Detalle_Venta>();
                }
            }
            return lista;
        }
    }
}
