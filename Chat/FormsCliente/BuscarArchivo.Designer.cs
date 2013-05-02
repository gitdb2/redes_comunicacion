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
            this.columnSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnaServidor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnOwner = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnMD5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnCerrar = new System.Windows.Forms.Button();
            this.btnDescargar = new System.Windows.Forms.Button();
            this.txtBuscarArchivo = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listaArchivos
            // 
            this.listaArchivos.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnNombreArchivo,
            this.columnSize,
            this.columnaServidor,
            this.columnOwner,
            this.columnMD5});
            this.listaArchivos.FullRowSelect = true;
            this.listaArchivos.Location = new System.Drawing.Point(12, 42);
            this.listaArchivos.MultiSelect = false;
            this.listaArchivos.Name = "listaArchivos";
            this.listaArchivos.Size = new System.Drawing.Size(324, 242);
            this.listaArchivos.TabIndex = 10;
            this.listaArchivos.UseCompatibleStateImageBehavior = false;
            this.listaArchivos.View = System.Windows.Forms.View.Details;
            this.listaArchivos.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listaArchivos_ColumnClick);
            this.listaArchivos.DoubleClick += new System.EventHandler(this.btnDescargar_Click);
            // 
            // columnNombreArchivo
            // 
            this.columnNombreArchivo.Text = "Nombre Archivo";
            this.columnNombreArchivo.Width = 107;
            // 
            // columnSize
            // 
            this.columnSize.Text = "Tamaño";
            // 
            // columnaServidor
            // 
            this.columnaServidor.Text = "Servidor";
            this.columnaServidor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnaServidor.Width = 117;
            // 
            // columnOwner
            // 
            this.columnOwner.Text = "Dueño";
            // 
            // columnMD5
            // 
            this.columnMD5.Text = "MD5";
            // 
            // btnCerrar
            // 
            this.btnCerrar.Location = new System.Drawing.Point(191, 292);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(75, 23);
            this.btnCerrar.TabIndex = 9;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // btnDescargar
            // 
            this.btnDescargar.Enabled = false;
            this.btnDescargar.Location = new System.Drawing.Point(82, 292);
            this.btnDescargar.Name = "btnDescargar";
            this.btnDescargar.Size = new System.Drawing.Size(75, 23);
            this.btnDescargar.TabIndex = 8;
            this.btnDescargar.Text = "Descargar";
            this.btnDescargar.UseVisualStyleBackColor = true;
            this.btnDescargar.Click += new System.EventHandler(this.btnDescargar_Click);
            // 
            // txtBuscarArchivo
            // 
            this.txtBuscarArchivo.Location = new System.Drawing.Point(12, 15);
            this.txtBuscarArchivo.Name = "txtBuscarArchivo";
            this.txtBuscarArchivo.Size = new System.Drawing.Size(243, 20);
            this.txtBuscarArchivo.TabIndex = 7;
            this.txtBuscarArchivo.Text = "*";
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(261, 12);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(75, 23);
            this.btnBuscar.TabIndex = 6;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // BuscarArchivo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 327);
            this.Controls.Add(this.listaArchivos);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.btnDescargar);
            this.Controls.Add(this.txtBuscarArchivo);
            this.Controls.Add(this.btnBuscar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
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
        private System.Windows.Forms.ColumnHeader columnMD5;

    }
}