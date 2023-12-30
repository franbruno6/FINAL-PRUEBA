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
    public class CD_Compra
    {
        public int ObtenerCorrelativo()
        {
            int idcorrelativo = 0;

            using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.Append("select count(*) + 1 from Compra");

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
        public bool Registrar(Compra obj, DataTable DetalleCompra, out string Mensaje)
        {
            bool Respuesta = false;
            Mensaje = string.Empty;
            
            using(SqlConnection oConexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarCompra", oConexion);
                    //PARAMETROS DE ENTRADA
                    cmd.Parameters.AddWithValue("IdUsuario", obj.oUsuario.IdUsuario);
                    cmd.Parameters.AddWithValue("IdProveedor", obj.oProveedor.IdProveedor);
                    cmd.Parameters.AddWithValue("TipoDocumento", obj.TipoDocumento);
                    cmd.Parameters.AddWithValue("NumeroDocumento", obj.NumeroDocumento);
                    cmd.Parameters.AddWithValue("MontoTotal", obj.MontoTotal);
                    cmd.Parameters.AddWithValue("DetalleCompra", DetalleCompra);
                    //PARAMETROS DE SALIDA
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;
                    //TIPO DE COMANDO
                    cmd.CommandType = CommandType.StoredProcedure;
                    //ABRIR CONEXION
                    oConexion.Open();

                    cmd.ExecuteNonQuery();

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
        public Compra ObtenerCompra(string numero)
        {
            Compra obj = null;

            using(SqlConnection oConexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.Append("select Compra.Id,");
                    query.Append("Usuario.NombreCompleto,");
                    query.Append("Proveedor.Documento,Proveedor.RazonSocial,");
                    query.Append("Compra.TipoDocumento,Compra.NroDocumento,Compra.MontoTotal,convert(char(10),Compra.FechaCreacion,103)[FechaCreacion]");
                    query.Append("from Compra ");
                    query.Append("inner join Usuario on Usuario.Id = Compra.IdUsuario ");
                    query.Append("inner join Proveedor on Proveedor.Id = Compra.IdProveedor ");
                    query.Append("where Compra.NroDocumento = @NumeroDocumento");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.Parameters.AddWithValue("NumeroDocumento", numero);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    using(SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            obj = new Compra()
                            {
                                IdCompra = Convert.ToInt32(dr["Id"]),
                                oUsuario = new Usuario()
                                {
                                    NombreCompleto = dr["NombreCompleto"].ToString()
                                },
                                oProveedor = new Proveedor()
                                {
                                    Documento = dr["Documento"].ToString(),
                                    RazonSocial = dr["RazonSocial"].ToString()
                                },
                                TipoDocumento = dr["TipoDocumento"].ToString(),
                                NumeroDocumento = dr["NroDocumento"].ToString(),
                                MontoTotal = Convert.ToDecimal(dr["MontoTotal"]),
                                FechaCreacion = dr["FechaCreacion"].ToString()
                            };
                        }
                    }
                }
                catch
                {
                    obj = new Compra();
                }
            }
            return obj;
        }
        public List<Detalle_Compra> ObtenerDetalleCompra(int idCompra)
        {
            List<Detalle_Compra> Lista = new List<Detalle_Compra>();

            using(SqlConnection oConexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.Append("select Producto.Nombre,");
                    query.Append("Detalle_Compra.PrecioCompra, Detalle_Compra.Cantidad, Detalle_Compra.MontoTotal ");
                    query.Append("from Detalle_Compra ");
                    query.Append("inner join Producto on Producto.Id = Detalle_Compra.IdProducto ");
                    query.Append("where Detalle_Compra.IdCompra = @IdCompra");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.Parameters.AddWithValue("IdCompra", idCompra);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    using(SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Lista.Add(new Detalle_Compra()
                            {
                                oProducto = new Producto()
                                {
                                    Nombre = dr["Nombre"].ToString()
                                },
                                PrecioCompra = Convert.ToDecimal(dr["PrecioCompra"]),
                                Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                MontoTotal = Convert.ToDecimal(dr["MontoTotal"])
                            });
                        }
                    }
                }
                catch
                {
                    Lista = new List<Detalle_Compra>();
                }
            }
            return Lista;
        }


    }
}
