namespace Chat
{
    partial class BuscarArchivo
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
            this.listaArchivos = new System.Windows.Forms.ListView();
            this.columnNombreArchivo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnaServidor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnOwner = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnCerrar = new System.Windows.Forms.Button();
            this.btnDescargar = new System.Windows.Forms.Button();
            this.txtBuscarArchivo = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.columnSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listaArchivos
            // 
            this.listaArchivos.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnNombreArchivo,
            this.columnSize,
            this.columnaServidor,
            this.columnOwner});
            this.listaArchivos.FullRowSelect = true;
            this.listaArchivos.Location = new System.Drawing.Point(16, 52);
            this.listaArchivos.Margin = new System.Windows.Forms.Padding(4);
            this.listaArchivos.MultiSelect = false;
            this.listaArchivos.Name = "listaArchivos";
            this.listaArchivos.Size = new System.Drawing.Size(431, 297);
            this.listaArchivos.TabIndex = 10;
            this.listaArchivos.UseCompatibleStateImageBehavior = false;
            this.listaArchivos.View = System.Windows.Forms.View.Details;
            this.listaArchivos.DoubleClick += new System.EventHandler(this.btnDescargar_Click);
            // 
            // columnNombreArchivo
            // 
            this.columnNombreArchivo.Text = "Nombre Archivo";
            this.columnNombreArchivo.Width = 107;
            // 
            // columnaServidor
            // 
            this.columnaServidor.DisplayIndex = 1;
            this.columnaServidor.Text = "Servidor";
            this.columnaServidor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnaServidor.Width = 117;
            // 
            // columnOwner
            // 
            this.columnOwner.DisplayIndex = 2;
            this.columnOwner.Text = "Dueño";
            // 
            // btnCerrar
            // 
            this.btnCerrar.Location = new System.Drawing.Point(255, 359);
            this.btnCerrar.Margin = new System.Windows.Forms.Padding(4);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(100, 28);
            this.btnCerrar.TabIndex = 9;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // btnDescargar
            // 
            this.btnDescargar.Enabled = false;
            this.btnDescargar.Location = new System.Drawing.Point(109, 359);
            this.btnDescargar.Margin = new System.Windows.Forms.Padding(4);
            this.btnDescargar.Name = "btnDescargar";
            this.btnDescargar.Size = new System.Drawing.Size(100, 28);
            this.btnDescargar.TabIndex = 8;
            this.btnDescargar.Text = "Descargar";
            this.btnDescargar.UseVisualStyleBackColor = true;
            this.btnDescargar.Click += new System.EventHandler(this.btnDescargar_Click);
            // 
            // txtBuscarArchivo
            // 
            this.txtBuscarArchivo.Location = new System.Drawing.Point(16, 18);
            this.txtBuscarArchivo.Margin = new System.Windows.Forms.Padding(4);
            this.txtBuscarArchivo.Name = "txtBuscarArchivo";
            this.txtBuscarArchivo.Size = new System.Drawing.Size(323, 22);
            this.txtBuscarArchivo.TabIndex = 7;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(348, 15);
            this.btnBuscar.Margin = new System.Windows.Forms.Padding(4);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(100, 28);
            this.btnBuscar.TabIndex = 6;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // columnSize
            // 
            this.columnSize.Text = "Tamaño";
            // 
            // BuscarArchivo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 402);
            this.Controls.Add(this.listaArchivos);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.btnDescargar);
            this.Controls.Add(this.txtBuscarArchivo);
            this.Controls.Add(this.btnBuscar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "BuscarArchivo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Buscar Archivo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BuscarArchivo_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listaArchivos;
        private System.Windows.Forms.ColumnHeader columnNombreArchivo;
        private System.Windows.Forms.ColumnHeader columnaServidor;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Button btnDescargar;
        private System.Windows.Forms.TextBox txtBuscarArchivo;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.ColumnHeader columnOwner;
        private System.Windows.Forms.ColumnHeader columnSize;

    }
}