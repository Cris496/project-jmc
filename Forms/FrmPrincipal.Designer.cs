namespace project-jmc
{
    partial class FrmPrincipal
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label txtBienvenido;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.txtBienvenido = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtBienvenido
            // 
            this.txtBienvenido.AutoSize = true;
            this.txtBienvenido.Location = new System.Drawing.Point(300, 60);
            this.txtBienvenido.Name = "txtBienvenido";
            this.txtBienvenido.Size = new System.Drawing.Size(60, 13);
            this.txtBienvenido.TabIndex = 0;
            this.txtBienvenido.Text = "Bienvenido";
            // Eliminamos el evento Click
            // this.txtBienvenido.Click += new System.EventHandler(this.label1_Click);
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtBienvenido);
            this.Name = "FrmPrincipal";
            this.Text = "Principal";
            this.Load += new System.EventHandler(this.FrmPrincipal_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
