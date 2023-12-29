using CapaEntidad;
using CapaNegocio;
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
    public partial class CP_Negocio : Form
    {
        public CP_Negocio()
        {
            InitializeComponent();
        }
        public Image ByteToImage(byte[] logo)
        {
            MemoryStream ms = new MemoryStream(logo);
            ms.Write(logo, 0, logo.Length);
            Image img = new Bitmap(ms);

            return img;
        }
        private void CP_Negocio_Load(object sender, EventArgs e)
        {
            bool obtenido = true;
            byte[] logo = new CN_Negocio().ObtenerLogo(out obtenido);

            if (obtenido)
            {
                //picLogo.Image = Utilidades.ConvertirImagen(logo); BUENA IDEA HACERLO EN UTILIDADES Y DESPUES LLAMARLO
                picLogo.Image = ByteToImage(logo);
            }

            Negocio obj = new CN_Negocio().ObtenerDatos();

            txtnegocio.Text = obj.Nombre;
            txtruc.Text = obj.Ruc;
            txtdireccion.Text = obj.Direccion;
        }

        private void btnsubir_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Archivo de Imagen (*.jpg, *.png, *.jpeg) | *.jpg; *.png; *.jpeg";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                byte[] logo = File.ReadAllBytes(dialog.FileName);
                bool respuesta = new CN_Negocio().ActualizarLogo(logo, out mensaje);

                if(respuesta)
                {
                    picLogo.Image = ByteToImage(logo);
                }
                else
                {
                    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnguardarcambios_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            Negocio obj = new Negocio()
            {
                Nombre = txtnegocio.Text,
                Ruc = txtruc.Text,
                Direccion = txtdireccion.Text
            };

            bool respuesta = new CN_Negocio().Registrar(obj, out mensaje);

            if (respuesta)
            {
                MessageBox.Show("Datos actualizados correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
