namespace Chat
{
    partial class VentanaPrincipalCliente
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuPrincipalArchivo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuArchivoOpcionSalir = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPrincipalAcciones = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAccionesOpcionAgregarContacto = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAccionesOpcionBuscarArchivo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAccionesOpcionSubirArchivo = new System.Windows.Forms.ToolStripMenuItem();
            this.listaContactos = new System.Windows.Forms.ListView();
            this.columnUsuario = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnEstado = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusStripConnectedAs = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblConnectedUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            this.statusStripConnectedAs.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuPrincipalArchivo,
            this.menuPrincipalAcciones});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(243, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuPrincipal";
            // 
            // menuPrincipalArchivo
            // 
            this.menuPrincipalArchivo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuArchivoOpcionSalir});
            this.menuPrincipalArchivo.Name = "menuPrincipalArchivo";
            this.menuPrincipalArchivo.Size = new System.Drawing.Size(60, 20);
            this.menuPrincipalArchivo.Text = "Archivo";
            // 
            // menuArchivoOpcionSalir
            // 
            this.menuArchivoOpcionSalir.Name = "menuArchivoOpcionSalir";
            this.menuArchivoOpcionSalir.Size = new System.Drawing.Size(96, 22);
            this.menuArchivoOpcionSalir.Text = "Salir";
            this.menuArchivoOpcionSalir.Click += new System.EventHandler(this.menuArchivoOpcionSalir_Click);
            // 
            // menuPrincipalAcciones
            // 
            this.menuPrincipalAcciones.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAccionesOpcionAgregarContacto,
            this.menuAccionesOpcionBuscarArchivo,
            this.menuAccionesOpcionSubirArchivo});
            this.menuPrincipalAcciones.Name = "menuPrincipalAcciones";
            this.menuPrincipalAcciones.Size = new System.Drawing.Size(67, 20);
            this.menuPrincipalAcciones.Text = "Acciones";
            // 
            // menuAccionesOpcionAgregarContacto
            // 
            this.menuAccionesOpcionAgregarContacto.Name = "menuAccionesOpcionAgregarContacto";
            this.menuAccionesOpcionAgregarContacto.Size = new System.Drawing.Size(168, 22);
            this.menuAccionesOpcionAgregarContacto.Text = "Agregar Contacto";
            this.menuAccionesOpcionAgregarContacto.Click += new System.EventHandler(this.menuAccionesOpcionAgregarContacto_Click);
            // 
            // menuAccionesOpcionBuscarArchivo
            // 
            this.menuAccionesOpcionBuscarArchivo.Name = "menuAccionesOpcionBuscarArchivo";
            this.menuAccionesOpcionBuscarArchivo.Size = new System.Drawing.Size(168, 22);
            this.menuAccionesOpcionBuscarArchivo.Text = "Buscar Archivo";
            this.menuAccionesOpcionBuscarArchivo.Click += new System.EventHandler(this.menuAccionesOpcionBuscarArchivo_Click);
            // 
            // menuAccionesOpcionSubirArchivo
            // 
            this.menuAccionesOpcionSubirArchivo.Name = "menuAccionesOpcionSubirArchivo";
            this.menuAccionesOpcionSubirArchivo.Size = new System.Drawing.Size(168, 22);
            this.menuAccionesOpcionSubirArchivo.Text = "Subir Archivo";
            this.menuAccionesOpcionSubirArchivo.Click += new System.EventHandler(this.menuAccionesOpcionSubirArchivo_Click);
            // 
            // listaContactos
            // 
            this.listaContactos.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnUsuario,
            this.columnEstado});
            this.listaContactos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listaContactos.FullRowSelect = true;
            this.listaContactos.Location = new System.Drawing.Point(0, 24);
            this.listaContactos.MultiSelect = false;
            this.listaContactos.Name = "listaContactos";
            this.listaContactos.Size = new System.Drawing.Size(243, 303);
            this.listaContactos.TabIndex = 1;
            this.listaContactos.UseCompatibleStateImageBehavior = false;
            this.listaContactos.View = System.Windows.Forms.View.Details;
            this.listaContactos.DoubleClick += new System.EventHandler(this.listaContactos_DoubleClick);
            // 
            // columnUsuario
            // 
            this.columnUsuario.Text = "Nombre";
            this.columnUsuario.Width = 74;
            // 
            // columnEstado
            // 
            this.columnEstado.Text = "Estado";
            this.columnEstado.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // statusStripConnectedAs
            // 
            this.statusStripConnectedAs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.toolStripStatusLabel,
            this.lblConnectedUser});
            this.statusStripConnectedAs.Location = new System.Drawing.Point(0, 305);
            this.statusStripConnectedAs.Name = "statusStripConnectedAs";
            this.statusStripConnectedAs.Size = new System.Drawing.Size(243, 22);
            this.statusStripConnectedAs.TabIndex = 2;
            this.statusStripConnectedAs.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(102, 17);
            this.lblStatus.Text = "Conectado como:";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // lblConnectedUser
            // 
            this.lblConnectedUser.Name = "lblConnectedUser";
            this.lblConnectedUser.Size = new System.Drawing.Size(0, 17);
            // 
            // VentanaPrincipalCliente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(243, 327);
            this.Controls.Add(this.statusStripConnectedAs);
            this.Controls.Add(this.listaContactos);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "VentanaPrincipalCliente";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ventana Principal";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VentanaPrincipalCliente_FormClosing);
            this.Load += new System.EventHandler(this.VentanaPrincipalCliente_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStripConnectedAs.ResumeLayout(false);
            this.statusStripConnectedAs.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuPrincipalArchivo;
        private System.Windows.Forms.ToolStripMenuItem menuArchivoOpcionSalir;
        private System.Windows.Forms.ToolStripMenuItem menuPrincipalAcciones;
        private System.Windows.Forms.ToolStripMenuItem menuAccionesOpcionAgregarContacto;
        private System.Windows.Forms.ToolStripMenuItem menuAccionesOpcionBuscarArchivo;
        private System.Windows.Forms.ToolStripMenuItem menuAccionesOpcionSubirArchivo;
        private System.Windows.Forms.ListView listaContactos;
        private System.Windows.Forms.ColumnHeader columnUsuario;
        private System.Windows.Forms.ColumnHeader columnEstado;
        private System.Windows.Forms.StatusStrip statusStripConnectedAs;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel lblConnectedUser;
    }
}