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

namespace CapaPresentacion
{
    public partial class CP_Proveedor : Form
    {
        public CP_Proveedor()
        {
            InitializeComponent();
        }

        private void CP_Proveedor_Load(object sender, EventArgs e)
        {
            //CARGAR COMBO ESTADO
            cboestado.Items.Add(new OpcionCombo() { Valor = 1, Texto = "Activo" });
            cboestado.Items.Add(new OpcionCombo() { Valor = 0, Texto = "Inactivo" });
            cboestado.DisplayMember = "Texto";
            cboestado.ValueMember = "Valor";
            cboestado.SelectedIndex = 0;

            //CARGAR COMBO BUSQUEDA
            foreach (DataGridViewColumn columna in dgvdata.Columns)
            {
                if (columna.Visible && columna.Name != "btnseleccionar")
                {
                    cbobusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                }
            }
            cbobusqueda.DisplayMember = "Texto";
            cbobusqueda.ValueMember = "Valor";
            cbobusqueda.SelectedIndex = 0;

            //MOSTRAR TODOS LOS PROVEEDORES
            List<Proveedor> listaProveedor = new CN_Proveedor().Listar();

            //MOSTRAR FILAS
            foreach (Proveedor item in listaProveedor)
            {
                dgvdata.Rows.Add(new object[] {
                    "",
                    item.IdProveedor,
                    item.Documento,
                    item.RazonSocial,
                    item.Correo,
                    item.Telefono,
                    item.Estado == true ? 1 : 0,
                    item.Estado == true ? "Activo" : "Inactivo"
                });
            }
        }
        private void btnguardar_Click(object sender, EventArgs e)
        {
            string Mensaje = string.Empty;

            Proveedor objProveedor = new Proveedor()
            {
                IdProveedor = Convert.ToInt32(txtid.Text),
                Documento = txtdocumento.Text,
                RazonSocial = txtrazonsocial.Text,
                Correo = txtcorreo.Text,
                Telefono = txttelefono.Text,
                Estado = Convert.ToInt32(((OpcionCombo)cboestado.SelectedItem).Valor) == 1 ? true : false
            };

            if (objProveedor.IdProveedor == 0)
            {
                //REGISTRAR
                int idProveedorRegistrado = new CN_Proveedor().Registrar(objProveedor, out Mensaje);

                if (idProveedorRegistrado != 0)
                {
                    MessageBox.Show("Proveedor registrado correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvdata.Rows.Add(new object[] {
                        "",
                        idProveedorRegistrado,
                        objProveedor.Documento,
                        objProveedor.RazonSocial,
                        objProveedor.Correo,
                        objProveedor.Telefono,
                        ((OpcionCombo)cboestado.SelectedItem).Valor.ToString(),
                        ((OpcionCombo)cboestado.SelectedItem).Texto.ToString()
                    });
                    Limpiar();
                }
                else
                {
                    MessageBox.Show(Mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                //EDITAR
                bool resultado = new CN_Proveedor().Editar(objProveedor, out Mensaje);

                if (resultado)
                {
                    MessageBox.Show("Proveedor actualizado correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DataGridViewRow fila = dgvdata.Rows[Convert.ToInt32(txtindice.Text)];
                    fila.Cells["Id"].Value = objProveedor.IdProveedor;
                    fila.Cells["Documento"].Value = objProveedor.Documento;
                    fila.Cells["RazonSocial"].Value = objProveedor.RazonSocial;
                    fila.Cells["Correo"].Value = objProveedor.Correo;
                    fila.Cells["Telefono"].Value = objProveedor.Telefono;
                    fila.Cells["EstadoValor"].Value = ((OpcionCombo)cboestado.SelectedItem).Valor.ToString();
                    fila.Cells["Estado"].Value = ((OpcionCombo)cboestado.SelectedItem).Texto.ToString();

                    Limpiar();
                }
                else
                {
                    MessageBox.Show(Mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnlimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }
        private void btneliminar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtid.Text) != 0)
            {
                if (MessageBox.Show("¿Está seguro de eliminar el Proveedor?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string Mensaje = string.Empty;

                    Proveedor objProveedor = new Proveedor()
                    {
                        IdProveedor = Convert.ToInt32(txtid.Text),
                    };

                    bool resultado = new CN_Proveedor().Eliminar(objProveedor, out Mensaje);

                    if (resultado)
                    {
                        MessageBox.Show("Proveedor eliminado correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        dgvdata.Rows.RemoveAt(Convert.ToInt32(txtindice.Text));

                        Limpiar();
                    }
                    else
                    {
                        MessageBox.Show(Mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void Limpiar()
        {
            txtindice.Text = "-1";
            txtid.Text = "0";
            txtdocumento.Text = "";
            txtrazonsocial.Text = "";
            txtcorreo.Text = "";
            txttelefono.Text = "";
            cboestado.SelectedIndex = 0;

            txtdocumento.Select();
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
        private void dgvdata_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (e.ColumnIndex == 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.check.Width;
                var h = Properties.Resources.check.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.check, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }
        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvdata.Columns[e.ColumnIndex].Name == "btnseleccionar")
            {
                int indice = e.RowIndex;

                if (indice >= 0)
                {
                    txtindice.Text = indice.ToString();
                    txtid.Text = dgvdata.Rows[indice].Cells["Id"].Value.ToString();
                    txtdocumento.Text = dgvdata.Rows[indice].Cells["Documento"].Value.ToString();
                    txtrazonsocial.Text = dgvdata.Rows[indice].Cells["RazonSocial"].Value.ToString();
                    txtcorreo.Text = dgvdata.Rows[indice].Cells["Correo"].Value.ToString();
                    txttelefono.Text = dgvdata.Rows[indice].Cells["Telefono"].Value.ToString();

                    foreach (OpcionCombo oc in cboestado.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvdata.Rows[indice].Cells["EstadoValor"].Value))
                        {
                            int indice_combo = cboestado.Items.IndexOf(oc);
                            cboestado.SelectedIndex = indice_combo;
                            break;
                        }
                    }
                }
            }
        }

    }
}
