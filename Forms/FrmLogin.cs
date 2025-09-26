using System;
using System.Windows.Forms;
using project-jmc.Data;
using project-jmc.Security;

namespace project-jmc
{
    public partial class FrmLogin : Form
    {
        private readonly AuthRepository _repo = new AuthRepository();

        public FrmLogin()
        {
            InitializeComponent();

            // Configurar botones para Enter y Esc
            this.AcceptButton = btnIngresar;
            this.CancelButton = btnCancelar;

            // Crear botón Registrar dinámicamente
            var btnRegistrar = new Button();
            btnRegistrar.Text = "Registrar";
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Width = 100;
            btnRegistrar.Height = 30;

            // Ubicación relativa (debajo del botón Cancelar)
            btnRegistrar.Top = btnCancelar.Bottom + 10;
            btnRegistrar.Left = btnCancelar.Left;

            // Asignar evento Click
            btnRegistrar.Click += BtnRegistrar_Click;

            // Agregar al formulario
            this.Controls.Add(btnRegistrar);

            // Siembra opcional: usuario admin inicial
            try
            {
                if (!_repo.ExisteUsuario("admin"))
                    _repo.CrearUsuario("admin", "Admin123!");
            }
            catch { /* ignora errores si ya existe */ }
        }

        // Evento para abrir el registro
        private void BtnRegistrar_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmRegistro())
            {
                frm.ShowDialog();
            }
        }

        // Métodos vacíos para TextChanged y Click de Labels
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void FrmLogin_Load(object sender, EventArgs e) { }

        // Botón Ingresar
        private void btnIngresar_Click(object sender, EventArgs e)
        {
            var user = txtUsuario.Text.Trim();
            var pass = txtClave.Text;

            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Usuario y contraseña son obligatorios", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var cred = _repo.ObtenerCredenciales(user);
            if (cred is null)
            {
                MessageBox.Show("Usuario o contraseña incorrectos", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!cred.Value.Activo)
            {
                MessageBox.Show("El usuario está inactivo. Contacte al administrador.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var ok = PasswordHasher.Verify(pass, cred.Value.Salt, cred.Value.Iteraciones, cred.Value.Hash);
            if (!ok)
            {
                MessageBox.Show("Usuario o contraseña incorrectos", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Login exitoso: abrir ventana principal
            this.Hide();
            using (var frm = new FrmPrincipal())
            {
                frm.ShowDialog();
            }
            this.Close();
        }

        // Botón Cancelar
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Método alternativo por si el Designer tiene referencia antigua
        private void Ingresar_Click(object sender, EventArgs e)
        {
            btnIngresar_Click(sender, e);
        }
    }
}
