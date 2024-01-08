using CapaEntidad;
using CapaNegocio;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
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

namespace CapaPresentacion
{
    public partial class CP_DetalleVenta : Form
    {
        public CP_DetalleVenta()
        {
            InitializeComponent();
        }
        private void CP_DetalleVenta_Load(object sender, EventArgs e)
        {
            txtbusqueda.Focus();
        }
        private void btnbuscar_Click(object sender, EventArgs e)
        {
            Venta oVenta = new CN_Venta().ObtenerVenta(txtbusqueda.Text);

            if (oVenta != null)
            {
                txtnrodocumento.Text = oVenta.NumeroDocumento;
                //GROUP BOX INFORMACION VENTA
                txtfecha.Text = oVenta.FechaCreacion;
                txttipodocumento.Text = oVenta.TipoDocumento;
                txtusuario.Text = oVenta.oUsuario.NombreCompleto;
                //GROUOP BOX INFORMACION CLIENTE
                txtnrodoccliente.Text = oVenta.DocumentoCliente;
                txtnombrecliente.Text = oVenta.NombreCliente;

                dgvdata.Rows.Clear();
                foreach (Detalle_Venta item in oVenta.oDetalle_Venta)
                {
                    dgvdata.Rows.Add(new object[]
                    {
                        item.oProducto.Nombre,
                        item.PrecioVenta,
                        item.Cantidad,
                        item.SubTotal
                    });
                }

                txtmontototal.Text = oVenta.MontoTotal.ToString("0.00");
                txtmontopago.Text = oVenta.MontoPago.ToString("0.00");
                txtmontocambio.Text = oVenta.MontoCambio.ToString("0.00");


            }
            else
            {
                MessageBox.Show("No existe la venta");
            }

        }
        private void btnlimpiar_Click(object sender, EventArgs e)
        {
            txtfecha.Text = "";
            txttipodocumento.Text = "";

            txtusuario.Text = "";
            txtnrodoccliente.Text = "";
            txtnombrecliente.Text = "";

            dgvdata.Rows.Clear();

            txtmontototal.Text = "";
            txtmontopago.Text = "";
            txtmontocambio.Text = "";
        }
        private void btndescargar_Click(object sender, EventArgs e)
        {
            if (txttipodocumento.Text == "")
            {
                MessageBox.Show("No hay datos para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string Texto_Html = Properties.Resources.PlantillaVenta.ToString();
            Negocio negocio = new CN_Negocio().ObtenerDatos();

            //INSERTAR VALORES DEL NEGOCIO
            Texto_Html = Texto_Html.Replace("@nombrenegocio", negocio.Nombre.ToUpper());
            Texto_Html = Texto_Html.Replace("@docnegocio", negocio.Ruc);
            Texto_Html = Texto_Html.Replace("@direcnegocio", negocio.Direccion);

            //INSERTAR VALORES DEL DOCUMENTO
            Texto_Html = Texto_Html.Replace("@tipodocumento", txttipodocumento.Text);
            Texto_Html = Texto_Html.Replace("@numerodocumento", txtnrodocumento.Text);

            //INSERTAR VALORES DEL CLIENTE
            Texto_Html = Texto_Html.Replace("@doccliente", txtnrodoccliente.Text);
            Texto_Html = Texto_Html.Replace("@nombrecliente", txtnombrecliente.Text);
            Texto_Html = Texto_Html.Replace("@fecharegistro", txtfecha.Text);
            Texto_Html = Texto_Html.Replace("@usuarioregistro", txtusuario.Text);

            //INSERTAR VALORES DEL DETALLE
            string filas = "";
            foreach (DataGridViewRow item in dgvdata.Rows)
            {
                filas += "<tr>";
                filas += "<td>" + item.Cells["Producto"].Value.ToString() + "</td>";
                filas += "<td>" + item.Cells["PrecioVenta"].Value.ToString() + "</td>";
                filas += "<td>" + item.Cells["Cantidad"].Value.ToString() + "</td>";
                filas += "<td>" + item.Cells["SubTotal"].Value.ToString() + "</td>";
                filas += "</tr>";
            }
            Texto_Html = Texto_Html.Replace("@filas", filas);
            Texto_Html = Texto_Html.Replace("@montototal", txtmontototal.Text);
            Texto_Html = Texto_Html.Replace("@pagocon", txtmontopago.Text);
            Texto_Html = Texto_Html.Replace("@cambio", txtmontocambio.Text);

            //GENERAR PDF
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = string.Format("DetalleVenta_{0}.pdf", txtnrodocumento.Text);
            savefile.Filter = "PDF Files (*.pdf)|*.pdf";

            if (savefile.ShowDialog() == DialogResult.OK)
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
