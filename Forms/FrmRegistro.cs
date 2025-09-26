using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using project-jmc.Data;
using project-jmc.Security;

namespace project-jmc
{
    public partial class FrmRegistro : Form
    {
        private readonly AuthRepository _repo = new AuthRepository();

        public FrmRegistro()
        {
            InitializeComponent();
            AplicarEstilosModernos();

            // Configurar botones para Enter y Esc
            this.AcceptButton = btnGuardar;
            this.CancelButton = btnCancelar;
        }

        private void AplicarEstilosModernos()
        {
            // Configuración del formulario
            this.BackColor = Color.FromArgb(240, 244, 247);
            this.ForeColor = Color.FromArgb(52, 73, 94);
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Text = "Registro de Usuario";
            this.Size = new Size(400, 400);
            this.Padding = new Padding(20);

            // Estilos para controles
            EstilizarLabel(lblUsuario);
            EstilizarLabel(lblClave);
            EstilizarLabel(lblEstado, isEstado: true);

            EstilizarTextBox(txtUsuario);
            EstilizarTextBox(txtClave);
            EstilizarBoton(btnGuardar, Color.FromArgb(46, 204, 113));
            EstilizarBoton(btnCancelar, Color.FromArgb(231, 76, 60));
        }

        private void EstilizarTextBox(TextBox textBox)
        {
            textBox.BackColor = Color.White;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.ForeColor = Color.FromArgb(52, 73, 94);
            textBox.Font = new Font("Segoe UI", 10F);
            textBox.Size = new Size(300, 35);

            // Efectos de focus
            textBox.Enter += (s, e) =>
            {
                textBox.BackColor = Color.FromArgb(236, 240, 241);
                textBox.BorderStyle = BorderStyle.FixedSingle;
            };

            textBox.Leave += (s, e) =>
            {
                textBox.BackColor = Color.White;
                textBox.BorderStyle = BorderStyle.FixedSingle;
            };
        }

        private void EstilizarBoton(Button boton, Color colorFondo)
        {
            boton.BackColor = colorFondo;
            boton.ForeColor = Color.White;
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = 0;
            boton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            boton.Size = new Size(120, 35);
            boton.Cursor = Cursors.Hand;

            // Efecto hover
            boton.MouseEnter += (s, e) =>
            {
                boton.BackColor = ControlPaint.Light(colorFondo, 0.1f);
            };

            boton.MouseLeave += (s, e) =>
            {
                boton.BackColor = colorFondo;
            };
        }

        private void EstilizarLabel(Label label, bool isEstado = false)
        {
            if (isEstado)
            {
                label.ForeColor = Color.FromArgb(231, 76, 60);
                label.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                label.TextAlign = ContentAlignment.MiddleLeft;
                label.AutoSize = false;
                label.Size = new Size(300, 30);
                label.BackColor = Color.Transparent;
            }
            else
            {
                label.ForeColor = Color.FromArgb(52, 73, 94);
                label.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                label.AutoSize = true;
                label.BackColor = Color.Transparent;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string password = txtClave.Text;

            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
            {
                lblEstado.Text = "Usuario y contraseña son obligatorios.";
                return;
            }

            // Longitud mínima
            if (usuario.Length < 3)
            {
                lblEstado.Text = "El usuario debe tener al menos 3 caracteres.";
                return;
            }

            if (password.Length < 6)
            {
                lblEstado.Text = "La contraseña debe tener al menos 6 caracteres.";
                return;
            }

            // Complejidad de contraseña: al menos una mayúscula, minúscula y número
            if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$"))
            {
                lblEstado.Text = "La contraseña debe contener mayúscula, minúscula y número.";
                return;
            }

            try
            {
                if (_repo.ExisteUsuario(usuario))
                {
                    lblEstado.Text = "El usuario ya existe.";
                    return;
                }

                _repo.CrearUsuario(usuario, password);
                MessageBox.Show("Usuario creado exitosamente.", "Registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                lblEstado.Text = "Error: " + ex.Message;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtUsuario_TextChanged(object sender, EventArgs e)
        {
            lblEstado.Text = ""; // Limpiar mensaje mientras escribe
        }

        private void txtClave_TextChanged(object sender, EventArgs e)
        {
            lblEstado.Text = ""; // Limpiar mensaje mientras escribe
        }

        private void FrmRegistro_Load(object sender, EventArgs e)
        {
            // Centrar controles
            CentrarControles();
        }

        private void CentrarControles()
        {
            int centerX = (this.ClientSize.Width - 300) / 2;
            int currentY = 30;

            // Label Usuario
            lblUsuario.Location = new Point(centerX, currentY);
            currentY += lblUsuario.Height + 5;

            // TextBox Usuario
            txtUsuario.Location = new Point(centerX, currentY);
            currentY += txtUsuario.Height + 15;

            // Label Contraseña
            lblClave.Location = new Point(centerX, currentY);
            currentY += lblClave.Height + 5;

            // TextBox Contraseña
            txtClave.Location = new Point(centerX, currentY);
            currentY += txtClave.Height + 20;

            // Label Estado
            lblEstado.Location = new Point(centerX, currentY);
            currentY += lblEstado.Height + 20;

            // Botones
            int buttonSpacing = 20;
            int totalButtonWidth = btnGuardar.Width + btnCancelar.Width + buttonSpacing;
            int buttonStartX = (this.ClientSize.Width - totalButtonWidth) / 2;

            btnGuardar.Location = new Point(buttonStartX, currentY);
            btnCancelar.Location = new Point(buttonStartX + btnGuardar.Width + buttonSpacing, currentY);
        }
    }
}
