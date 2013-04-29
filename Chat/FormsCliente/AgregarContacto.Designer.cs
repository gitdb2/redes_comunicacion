namespace Chat
{
    partial class AgregarContacto
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnBuscar = new System.Windows.Forms.Button();
            this.txtBuscarContacto = new System.Windows.Forms.TextBox();
            this.btnAgregarContacto = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.listaContactos = new System.Windows.Forms.ListView();
            this.columnUsuario = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnEstado = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(181, 12);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(75, 23);
            this.btnBuscar.TabIndex = 0;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // txtBuscarContacto
            // 
            this.txtBuscarContacto.Location = new System.Drawing.Point(9, 14);
            this.txtBuscarContacto.MaxLength = 8;
            this.txtBuscarContacto.Name = "txtBuscarContacto";
            this.txtBuscarContacto.Size = new System.Drawing.Size(164, 20);
            this.txtBuscarContacto.TabIndex = 1;
            // 
            // btnAgregarContacto
            // 
            this.btnAgregarContacto.Location = new System.Drawing.Point(32, 292);
            this.btnAgregarContacto.Name = "btnAgregarContacto";
            this.btnAgregarContacto.Size = new System.Drawing.Size(75, 23);
            this.btnAgregarContacto.TabIndex = 3;
            this.btnAgregarContacto.Text = "Agregar";
            this.btnAgregarContacto.UseVisualStyleBackColor = true;
            this.btnAgregarContacto.Click += new System.EventHandler(this.btnAgregarContacto_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.Location = new System.Drawing.Point(140, 292);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(75, 23);
            this.btnCerrar.TabIndex = 4;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // listaContactos
            // 
            this.listaContactos.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnUsuario,
            this.columnEstado});
            this.listaContactos.FullRowSelect = true;
            this.listaContactos.Location = new System.Drawing.Point(9, 41);
            this.listaContactos.MultiSelect = false;
            this.listaContactos.Name = "listaContactos";
            this.listaContactos.Size = new System.Drawing.Size(247, 242);
            this.listaContactos.TabIndex = 5;
            this.listaContactos.UseCompatibleStateImageBehavior = false;
            this.listaContactos.View = System.Windows.Forms.View.Details;
            // 
            // columnUsuario
            // 
            this.columnUsuario.Text = "Nombre";
            this.columnUsuario.Width = 70;
            // 
            // columnEstado
            // 
            this.columnEstado.Text = "Estado";
            this.columnEstado.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // AgregarContacto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(265, 327);
            this.Controls.Add(this.listaContactos);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.btnAgregarContacto);
            this.Controls.Add(this.txtBuscarContacto);
            this.Controls.Add(this.btnBuscar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.Name = "AgregarContacto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Agregar Contacto";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.TextBox txtBuscarContacto;
        private System.Windows.Forms.Button btnAgregarContacto;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.ListView listaContactos;
        private System.Windows.Forms.ColumnHeader columnUsuario;
        private System.Windows.Forms.ColumnHeader columnEstado;
    }
}