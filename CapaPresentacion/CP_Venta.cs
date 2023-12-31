using CapaEntidad;
using CapaPresentacion.Modales;
using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class CP_Ventas : Form
    {
        //EL FORM DE COMPRA TIENE TODOS LOS METODOS DE VERIFICACION DE DATOS
        //SEGUIR DE FORMA EJEMPLO ESE FORM, NO ESTE
        //EL METODO DE VERIFICAR CAMBIO ME VA A SERVIR PARA CALCULAR EL SALDO PENDIENTE DEL CLIENTE
        private Usuario _Usuario;
        public CP_Ventas(Usuario oUsuario = null)
        {
            _Usuario = oUsuario;
            InitializeComponent();
        }
        private void CP_Ventas_Load(object sender, EventArgs e)
        {
            cbotipodocumento.Items.Add(new OpcionCombo() { Valor = "Boleta", Texto = "Boleta" });
            cbotipodocumento.Items.Add(new OpcionCombo() { Valor = "Factura", Texto = "Factura" });
            cbotipodocumento.DisplayMember = "Texto";
            cbotipodocumento.ValueMember = "Valor";
            cbotipodocumento.SelectedIndex = 0;

            txtfecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtidproducto.Text = "0";

            txtpagocon.Text = "0,00";
            txttotal.Text = "0,00";
            txtcambio.Text = "0,00";
        }
        private void btnbuscarcliente_Click(object sender, EventArgs e)
        {
            using (var modal = new mdCliente())
            {
                var resultado = modal.ShowDialog();

                if (resultado == DialogResult.OK)
                {
                    txtdoccliente.Text = modal._Cliente.Documento;
                    txtnombrecliente.Text = modal._Cliente.NombreCompleto;
                    txtcodproducto.Focus();
                }
                else
                {
                    txtdoccliente.Text = string.Empty;
                    txtnombrecliente.Text = string.Empty;
                    txtdoccliente.Focus();
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
                    txtprecio.Text = modal.ProductoSeleccionado.PrecioVenta.ToString();
                    txtstock.Text = modal.ProductoSeleccionado.Stock.ToString();
                    numcantidad.Select();
                }
                else
                {
                    txtidproducto.Text = "0";
                    txtcodproducto.Text = string.Empty;
                    txtnombreproducto.Text = string.Empty;
                    txtprecio.Text = string.Empty;
                    txtstock.Text = string.Empty;
                    numcantidad.Value = 1;
                    txtcodproducto.Focus();
                }
            }
        }
        private void btnagregarproducto_Click(object sender, EventArgs e)
        {
            decimal precio = 0;
            bool productoExiste = false;

            if (int.Parse(txtidproducto.Text) == 0)
            {
                MessageBox.Show("Seleccione un producto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtcodproducto.Focus();
                return;
            }

            if (!decimal.TryParse(txtprecio.Text, out precio))
            {
                MessageBox.Show("Ingrese un precio válido", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtprecio.Select();
                return;
            }

            if (Convert.ToInt32(txtstock.Text) < Convert.ToInt32(numcantidad.Value.ToString()))
            {
                MessageBox.Show("No hay stock suficiente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            if (productoExiste)
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
                    precio.ToString("0.00"),
                    numcantidad.Value.ToString(),
                    (precio * numcantidad.Value).ToString("0.00"),
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
            txtprecio.Text = string.Empty;
            txtstock.Text = string.Empty;
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

            if (e.ColumnIndex == 5)
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
        private void calcularCambio()
        {
            if (txttotal.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Ingrese un total válido", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            decimal total = Convert.ToDecimal(txttotal.Text);
            decimal pagacon = 0;

            if (txtpagocon.Text.Trim() == "")
            {
                txtpagocon.Text = "0,00";
            }

            if (decimal.TryParse(txtpagocon.Text, out pagacon))
            {
                if (pagacon < total)
                {
                    MessageBox.Show("El monto a pagar debe ser mayor o igual al total", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtpagocon.Select();
                    return;
                }
                else
                {
                    decimal cambio = pagacon - total;
                    txtcambio.Text = cambio.ToString("0.00");
                }
            }
        }
        private void txtpagocon_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                calcularCambio();
            }
        }
    }
}
