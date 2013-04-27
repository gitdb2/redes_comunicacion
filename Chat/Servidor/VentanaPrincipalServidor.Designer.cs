namespace Servidor
{
    partial class VentanaPrincipalServidor
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
            this.txtBoxNombre = new System.Windows.Forms.TextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.lblDireccionIP = new System.Windows.Forms.Label();
            this.btnIniciarServidor = new System.Windows.Forms.Button();
            this.btnDetenerServidor = new System.Windows.Forms.Button();
            this.lblCarpetaDatos = new System.Windows.Forms.Label();
            this.txtBoxCarpetaDatos = new System.Windows.Forms.TextBox();
            this.listaClientes = new System.Windows.Forms.ListView();
            this.columnUsuario = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnIP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnaEstado = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtBoxDireccionIP = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtBoxNombre
            // 
            this.txtBoxNombre.Location = new System.Drawing.Point(113, 12);
            this.txtBoxNombre.Name = "txtBoxNombre";
            this.txtBoxNombre.Size = new System.Drawing.Size(241, 20);
            this.txtBoxNombre.TabIndex = 1;
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(60, 15);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(47, 13);
            this.lblNombre.TabIndex = 0;
            this.lblNombre.Text = "Nombre:";
            // 
            // lblDireccionIP
            // 
            this.lblDireccionIP.AutoSize = true;
            this.lblDireccionIP.Location = new System.Drawing.Point(39, 41);
            this.lblDireccionIP.Name = "lblDireccionIP";
            this.lblDireccionIP.Size = new System.Drawing.Size(68, 13);
            this.lblDireccionIP.TabIndex = 2;
            this.lblDireccionIP.Text = "Direccion IP:";
            // 
            // btnIniciarServidor
            // 
            this.btnIniciarServidor.Location = new System.Drawing.Point(28, 102);
            this.btnIniciarServidor.Name = "btnIniciarServidor";
            this.btnIniciarServidor.Size = new System.Drawing.Size(147, 23);
            this.btnIniciarServidor.TabIndex = 6;
            this.btnIniciarServidor.Text = "Iniciar Servidor";
            this.btnIniciarServidor.UseVisualStyleBackColor = true;
            this.btnIniciarServidor.Click += new System.EventHandler(this.btnIniciarServidor_Click);
            // 
            // btnDetenerServidor
            // 
            this.btnDetenerServidor.Location = new System.Drawing.Point(193, 102);
            this.btnDetenerServidor.Name = "btnDetenerServidor";
            this.btnDetenerServidor.Size = new System.Drawing.Size(147, 23);
            this.btnDetenerServidor.TabIndex = 7;
            this.btnDetenerServidor.Text = "Detener Servidor";
            this.btnDetenerServidor.UseVisualStyleBackColor = true;
            // 
            // lblCarpetaDatos
            // 
            this.lblCarpetaDatos.AutoSize = true;
            this.lblCarpetaDatos.Location = new System.Drawing.Point(14, 67);
            this.lblCarpetaDatos.Name = "lblCarpetaDatos";
            this.lblCarpetaDatos.Size = new System.Drawing.Size(93, 13);
            this.lblCarpetaDatos.TabIndex = 4;
            this.lblCarpetaDatos.Text = "Carpeta de Datos:";
            // 
            // txtBoxCarpetaDatos
            // 
            this.txtBoxCarpetaDatos.Location = new System.Drawing.Point(113, 64);
            this.txtBoxCarpetaDatos.Name = "txtBoxCarpetaDatos";
            this.txtBoxCarpetaDatos.Size = new System.Drawing.Size(241, 20);
            this.txtBoxCarpetaDatos.TabIndex = 5;
            // 
            // listaClientes
            // 
            this.listaClientes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnUsuario,
            this.columnIP,
            this.columnaEstado});
            this.listaClientes.FullRowSelect = true;
            this.listaClientes.Location = new System.Drawing.Point(17, 143);
            this.listaClientes.MultiSelect = false;
            this.listaClientes.Name = "listaClientes";
            this.listaClientes.Size = new System.Drawing.Size(337, 208);
            this.listaClientes.TabIndex = 8;
            this.listaClientes.UseCompatibleStateImageBehavior = false;
            this.listaClientes.View = System.Windows.Forms.View.Details;
            // 
            // columnUsuario
            // 
            this.columnUsuario.Text = "Usuario";
            this.columnUsuario.Width = 74;
            // 
            // columnIP
            // 
            this.columnIP.Text = "Direccion IP";
            this.columnIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnIP.Width = 70;
            // 
            // columnaEstado
            // 
            this.columnaEstado.Text = "Estado";
            this.columnaEstado.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnaEstado.Width = 81;
            // 
            // txtBoxDireccionIP
            // 
            this.txtBoxDireccionIP.Location = new System.Drawing.Point(113, 38);
            this.txtBoxDireccionIP.Name = "txtBoxDireccionIP";
            this.txtBoxDireccionIP.Size = new System.Drawing.Size(241, 20);
            this.txtBoxDireccionIP.TabIndex = 9;
            // 
            // VentanaPrincipalServidor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 363);
            this.Controls.Add(this.txtBoxDireccionIP);
            this.Controls.Add(this.listaClientes);
            this.Controls.Add(this.txtBoxCarpetaDatos);
            this.Controls.Add(this.lblCarpetaDatos);
            this.Controls.Add(this.btnDetenerServidor);
            this.Controls.Add(this.btnIniciarServidor);
            this.Controls.Add(this.lblDireccionIP);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.txtBoxNombre);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "VentanaPrincipalServidor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ventana Principal Servidor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBoxNombre;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.Label lblDireccionIP;
        private System.Windows.Forms.Button btnIniciarServidor;
        private System.Windows.Forms.Button btnDetenerServidor;
        private System.Windows.Forms.Label lblCarpetaDatos;
        private System.Windows.Forms.TextBox txtBoxCarpetaDatos;
        private System.Windows.Forms.ListView listaClientes;
        private System.Windows.Forms.ColumnHeader columnUsuario;
        private System.Windows.Forms.ColumnHeader columnIP;
        private System.Windows.Forms.ColumnHeader columnaEstado;
        private System.Windows.Forms.TextBox txtBoxDireccionIP;
    }
}

