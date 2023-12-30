using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Modales;
using CapaPresentacion.Utilidades;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Color = System.Drawing.Color;

namespace CapaPresentacion
{
    public partial class CP_Compra : Form
    {
        private Usuario _Usuario;
        public CP_Compra(Usuario usuario = null)
        {
            _Usuario = usuario;
            InitializeComponent();
        }
        private void CP_Compra_Load(object sender, EventArgs e)
        {
            //MOSTRAR TIPO DE DOCUMENTO
            cbotipodocumento.Items.Add(new OpcionCombo() { Valor = "Boleta", Texto = "Boleta" });
            cbotipodocumento.Items.Add(new OpcionCombo() { Valor = "Factura", Texto = "Factura" });
            cbotipodocumento.DisplayMember = "Texto";
            cbotipodocumento.ValueMember = "Valor";
            cbotipodocumento.SelectedIndex = 0;

            txtfecha.Text = DateTime.Now.ToString("dd/MM/yyyy");

            txtidproveedor.Text = "0";
            txtidproducto.Text = "0";
        }
        private void btnbuscarproveedor_Click(object sender, EventArgs e)
        {
            using (var modal = new mdProveedor())
            {
                var resultado = modal.ShowDialog();

                if (resultado == DialogResult.OK)
                {
                    txtidproveedor.Text = modal.ProveedorSeleccionado.IdProveedor.ToString();
                    txtdocproveedor.Text = modal.ProveedorSeleccionado.Documento;
                    txtrazonsocialproveedor.Text = modal.ProveedorSeleccionado.RazonSocial;
                }
                else
                {
                    txtidproveedor.Text = "0";
                    txtdocproveedor.Text = string.Empty;
                    txtrazonsocialproveedor.Text = string.Empty;
                    txtdocproveedor.Focus();
                }
            }
        }
        private void btnbuscarproducto_Click(object sender, EventArgs e)
        {
            using (var modal = new mdProducto())
            {
                var resultado = modal.ShowDialog();

                if (resultado == DialogResult.OK)
                {
                    txtidproducto.Text = modal.ProductoSeleccionado.IdProducto.ToString();
                    txtcodproducto.Text = modal.ProductoSeleccionado.Codigo;
                    txtnombreproducto.Text = modal.ProductoSeleccionado.Nombre;
                    txtpreciocompra.Select();
                }
                else
                {
                    txtidproducto.Text = "0";
                    txtcodproducto.Text = string.Empty;
                    txtnombreproducto.Text = string.Empty;
                    txtpreciocompra.Text = string.Empty;
                    txtprecioventa.Text = string.Empty;
                    numcantidad.Value = 1;
                    txtidproducto.Focus();
                }
            }
        }
        private void txtcodproducto_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter) /*CHEQUEAR COMO HACER PARA QUE TAMBIEN SE PUEDA CON EL TAB*/
            {
                Producto producto = new CN_Producto().Listar().Where(x => x.Codigo == txtcodproducto.Text).FirstOrDefault();

                if (producto != null)
                {
                    txtcodproducto.BackColor = Color.Honeydew;
                    txtidproducto.Text = producto.IdProducto.ToString();
                    txtnombreproducto.Text = producto.Nombre;
                    txtpreciocompra.Select();
                }
                else
                {
                    MessageBox.Show("No se encontró el producto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtidproducto.Text = "0";
                    txtcodproducto.BackColor = Color.MistyRose;
                    txtcodproducto.Text = string.Empty;
                    txtnombreproducto.Text = string.Empty;
                    txtpreciocompra.Text = string.Empty;
                    txtprecioventa.Text = string.Empty;
                    numcantidad.Value = 1;
                    txtidproducto.Focus();
                }
            }
        }
        private void btnagregarproducto_Click(object sender, EventArgs e)
        {
            decimal precioCompra = 0;
            decimal precioVenta = 0;
            bool productoExiste = false;

            if (int.Parse(txtidproducto.Text) == 0)
            {
                MessageBox.Show("Seleccione un producto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtcodproducto.Select();
                return;
            }

            if (!decimal.TryParse(txtpreciocompra.Text, out precioCompra))
            {
                MessageBox.Show("Ingrese un precio de compra válido", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtpreciocompra.Select();
                return;
            }

            if (!decimal.TryParse(txtprecioventa.Text, out precioVenta))
            {
                MessageBox.Show("Ingrese un precio de venta válido", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtprecioventa.Select();
                return;
            }


            foreach (DataGridViewRow fila in dgvdata.Rows)
            {
                if (fila.Cells["Id"].Value.ToString() == txtidproducto.Text)
                {
                    productoExiste = true;
                    break;
                }
            }
            

            if(productoExiste)
            {
                MessageBox.Show("El producto ya existe en la lista", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                limpiarProducto();
            }
            else
            {
                dgvdata.Rows.Add(new object[]
                {
                    txtidproducto.Text,
                    txtnombreproducto.Text,
                    precioCompra.ToString("0.00"),
                    precioVenta.ToString("0.00"),
                    numcantidad.Value.ToString(),
                    (precioCompra * numcantidad.Value).ToString("0.00"),
                });
                calcularTotal();
                limpiarProducto();
            }
        }
        private void limpiarProducto()
        {
            txtidproducto.Text = "0";
            txtcodproducto.Text = string.Empty;
            txtcodproducto.BackColor = Color.White;
            txtnombreproducto.Text = string.Empty;
            txtpreciocompra.Text = string.Empty;
            txtprecioventa.Text = string.Empty;
            numcantidad.Value = 1;
            txtcodproducto.Focus();
        }   
        private void calcularTotal()
        {
            decimal total = 0;

            if (dgvdata.Rows.Count > 0)
            {
                foreach (DataGridViewRow fila in dgvdata.Rows)
                {
                    total += decimal.Parse(fila.Cells["SubTotal"].Value.ToString());
                }
            }

            txttotal.Text = total.ToString("0.00");
        }
        private void dgvdata_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (e.ColumnIndex == 6)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.trash.Width;
                var h = Properties.Resources.trash.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.trash, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }
        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvdata.Columns[e.ColumnIndex].Name == "btneliminar")
            {
                int indice = e.RowIndex;

                if (indice >= 0)
                {
                    dgvdata.Rows.RemoveAt(indice);
                    calcularTotal();
                }
            }
        }
        private void txtpreciocompra_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if(txtpreciocompra.Text.Trim().Length == 0 && e.KeyChar.ToString() == ",")
            {
                e.Handled = true;
            }
            else if(Char.IsControl(e.KeyChar) || e.KeyChar.ToString() == ",")
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        private void txtprecioventa_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (txtprecioventa.Text.Trim().Length == 0 && e.KeyChar.ToString() == ",")
            {
                e.Handled = true;
            }
            else if (Char.IsControl(e.KeyChar) || e.KeyChar.ToString() == ",")
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
