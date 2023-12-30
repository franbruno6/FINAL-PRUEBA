using CapaEntidad;
using CapaNegocio;
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

namespace CapaPresentacion.Modales
{
    public partial class mdProducto : Form
    {
        public Producto ProductoSeleccionado { get; set; }
        public mdProducto()
        {
            InitializeComponent();
        }
        //EL MODAL DE PROVEEDOR ESTA MEJOR CONFIGURADO EL DATAGRIDVIEW. MIRAR ESA CONFIGURACION
        private void mdProducto_Load(object sender, EventArgs e)
        {
            //CARGAR COMBO BUSQUEDA
            foreach (DataGridViewColumn columna in dgvdata.Columns)
            {
                if (columna.Visible)
                {
                    cbobusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                }
            }
            cbobusqueda.DisplayMember = "Texto";
            cbobusqueda.ValueMember = "Valor";
            cbobusqueda.SelectedIndex = 0;

            //MOSTRAR TODOS LOS PRODUCTOS
            List<Producto> listaProducto = new CN_Producto().Listar();

            //MOSTRAR FILAS
            foreach (Producto item in listaProducto)
            {
                dgvdata.Rows.Add(new object[] {
                    item.IdProducto,
                    item.Codigo,
                    item.Nombre,
                    item.oCategoria.Descripcion,
                    item.Stock,
                    item.PrecioCompra,
                    item.PrecioVenta,
                });
            }
        }
        private void dgvdata_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int indiceFila = e.RowIndex;
            int indiceColumna = e.ColumnIndex;

            if (indiceFila >= 0 && indiceColumna > 0)
            {
                ProductoSeleccionado = new Producto()
                {
                    IdProducto = Convert.ToInt32(dgvdata.Rows[indiceFila].Cells["Id"].Value),
                    Codigo = dgvdata.Rows[indiceFila].Cells["Codigo"].Value.ToString(),
                    Nombre = dgvdata.Rows[indiceFila].Cells["Nombre"].Value.ToString(),
                    Stock = Convert.ToInt32(dgvdata.Rows[indiceFila].Cells["Stock"].Value),
                    PrecioCompra = Convert.ToDecimal(dgvdata.Rows[indiceFila].Cells["PrecioCompra"].Value),
                    PrecioVenta = Convert.ToDecimal(dgvdata.Rows[indiceFila].Cells["PrecioVenta"].Value),
                };
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        private void btnbuscar_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)cbobusqueda.SelectedItem).Valor.ToString();

            if (dgvdata.Rows.Count > 0)
            {
                foreach (DataGridViewRow fila in dgvdata.Rows)
                {
                    if (fila.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtbusqueda.Text.Trim().ToUpper()))
                    {
                        fila.Visible = true;
                    }
                    else
                    {
                        fila.Visible = false;
                    }
                }
            }
        }
        private void btnlimpiarbuscador_Click(object sender, EventArgs e)
        {
            txtbusqueda.Text = "";

            foreach (DataGridViewRow fila in dgvdata.Rows)
            {
                fila.Visible = true;
            }
        }
    }
}
