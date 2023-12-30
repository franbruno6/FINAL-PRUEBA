using CapaEntidad;
using CapaNegocio;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;


namespace CapaPresentacion
{
    public partial class CP_DetalleCompra : Form
    {
        public CP_DetalleCompra()
        {
            InitializeComponent();
        }
        private void btnbuscar_Click(object sender, EventArgs e)
        {
            Compra obj = new CN_Compra().ObtenerCompra(txtbusqueda.Text);

            if( obj.IdCompra != 0)
            {
                txtnrodocumento.Text = obj.NumeroDocumento;
                txtfecha.Text = obj.FechaCreacion;
                txttipodocumento.Text = obj.TipoDocumento;
                txtusuario.Text = obj.oUsuario.NombreCompleto;
                txtnrodocproveedor.Text = obj.oProveedor.Documento;
                txtrazonsocialproveedor.Text = obj.oProveedor.RazonSocial;

                //LIMPIAR FILAS
                dgvdata.Rows.Clear();
                //AGREGAR FILAS
                foreach (Detalle_Compra item in obj.oListaDetalleCompra)
                {
                    dgvdata.Rows.Add(new object[]
                    {
                        item.oProducto.Nombre,
                        item.PrecioCompra,
                        item.Cantidad,
                        item.MontoTotal
                    });
                    txtmontototal.Text = obj.MontoTotal.ToString("0,00");
                }
            }
        }
        private void btnlimpiar_Click(object sender, EventArgs e)
        {
            txtfecha.Text = "";
            txttipodocumento.Text = "";
            txtusuario.Text = "";
            txtnrodocproveedor.Text = "";
            txtrazonsocialproveedor.Text = "";

            //LIMPIAR FILAS
            dgvdata.Rows.Clear();
            txtmontototal.Text = "0,00";
        }
        private void btndescargar_Click(object sender, EventArgs e)
        {
            if (txttipodocumento.Text == "")
            {
                MessageBox.Show("No hay datos para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string Texto_Html = Properties.Resources.PlantillaCompra.ToString();
            Negocio negocio = new CN_Negocio().ObtenerDatos();

            //INSERTAR VALORES DEL NEGOCIO
            Texto_Html = Texto_Html.Replace("@nombrenegocio", negocio.Nombre.ToUpper());
            Texto_Html = Texto_Html.Replace("@docnegocio", negocio.Ruc);
            Texto_Html = Texto_Html.Replace("@direcnegocio", negocio.Direccion);
                
            //INSERTAR VALORES DEL DOCUMENTO
            Texto_Html = Texto_Html.Replace("@tipodocumento", txttipodocumento.Text);
            Texto_Html = Texto_Html.Replace("@numerodocumento", txtnrodocumento.Text);
                
            //INSERTAR VALORES DEL PROVEEDOR
            Texto_Html = Texto_Html.Replace("@docproveedor", txtnrodocproveedor.Text);
            Texto_Html = Texto_Html.Replace("@nombreproveedor", txtrazonsocialproveedor.Text);
            Texto_Html = Texto_Html.Replace("@fecharegistro", txtfecha.Text);
            Texto_Html = Texto_Html.Replace("@usuarioregistro", txtusuario.Text);

            //INSERTAR VALORES DEL DETALLE
            string filas = "";
            foreach (DataGridViewRow item in dgvdata.Rows)
            {
                filas += "<tr>";
                filas += "<td>" + item.Cells["Producto"].Value.ToString() + "</td>";
                filas += "<td>" + item.Cells["PrecioCompra"].Value.ToString() + "</td>";
                filas += "<td>" + item.Cells["Cantidad"].Value.ToString() + "</td>";
                filas += "<td>" + item.Cells["SubTotal"].Value.ToString() + "</td>";
                filas += "</tr>";
            }
            Texto_Html = Texto_Html.Replace("@filas", filas);
            Texto_Html = Texto_Html.Replace("@montototal", txtmontototal.Text);

            //GENERAR PDF
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = string.Format("DetalleCompra_{0}.pdf", txtnrodocumento.Text);
            savefile.Filter = "PDF Files (*.pdf)|*.pdf";

            if(savefile.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(savefile.FileName, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 25);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    bool obtenido = true;
                    byte[] byteimagen = new CN_Negocio().ObtenerLogo(out obtenido);

                    if (obtenido)
                    {
                        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(byteimagen);
                        logo.ScaleToFit(60, 60);
                        logo.Alignment = iTextSharp.text.Image.UNDERLYING;
                        logo.SetAbsolutePosition(pdfDoc.Left, pdfDoc.GetTop(51));
                        pdfDoc.Add(logo);
                    }

                    using (StringReader sr = new StringReader(Texto_Html))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);

                    }

                    pdfDoc.Close();
                    stream.Close();
                    MessageBox.Show("Se generó el PDF correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
