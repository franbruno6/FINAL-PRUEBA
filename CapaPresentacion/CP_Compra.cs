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
    }
}
