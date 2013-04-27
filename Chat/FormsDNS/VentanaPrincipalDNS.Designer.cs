namespace FormsDNS
{
    partial class VentanaPrincipalDNS
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
            this.txtBoxDireccionIP = new System.Windows.Forms.TextBox();
            this.lblPuerto = new System.Windows.Forms.Label();
            this.btnDetenerServidor = new System.Windows.Forms.Button();
            this.btnIniciarServidor = new System.Windows.Forms.Button();
            this.lblDireccionIP = new System.Windows.Forms.Label();
            this.txtBoxPuerto = new System.Windows.Forms.MaskedTextBox();
            this.txtBoxMensajes = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtBoxDireccionIP
            // 
            this.txtBoxDireccionIP.Location = new System.Drawing.Point(143, 21);
            this.txtBoxDireccionIP.Name = "txtBoxDireccionIP";
            this.txtBoxDireccionIP.Size = new System.Drawing.Size(147, 20);
            this.txtBoxDireccionIP.TabIndex = 1;
            // 
            // lblPuerto
            // 
            this.lblPuerto.AutoSize = true;
            this.lblPuerto.Location = new System.Drawing.Point(98, 62);
            this.lblPuerto.Name = "lblPuerto";
            this.lblPuerto.Size = new System.Drawing.Size(41, 13);
            this.lblPuerto.TabIndex = 2;
            this.lblPuerto.Text = "Puerto:";
            // 
            // btnDetenerServidor
            // 
            this.btnDetenerServidor.Location = new System.Drawing.Point(209, 97);
            this.btnDetenerServidor.Name = "btnDetenerServidor";
            this.btnDetenerServidor.Size = new System.Drawing.Size(99, 23);
            this.btnDetenerServidor.TabIndex = 5;
            this.btnDetenerServidor.Text = "Detener Servidor";
            this.btnDetenerServidor.UseVisualStyleBackColor = true;
            // 
            // btnIniciarServidor
            // 
            this.btnIniciarServidor.Location = new System.Drawing.Point(50, 97);
            this.btnIniciarServidor.Name = "btnIniciarServidor";
            this.btnIniciarServidor.Size = new System.Drawing.Size(97, 23);
            this.btnIniciarServidor.TabIndex = 4;
            this.btnIniciarServidor.Text = "Iniciar Servidor";
            this.btnIniciarServidor.UseVisualStyleBackColor = true;
            this.btnIniciarServidor.Click += new System.EventHandler(this.btnIniciarServidor_Click);
            // 
            // lblDireccionIP
            // 
            this.lblDireccionIP.AutoSize = true;
            this.lblDireccionIP.Location = new System.Drawing.Point(69, 24);
            this.lblDireccionIP.Name = "lblDireccionIP";
            this.lblDireccionIP.Size = new System.Drawing.Size(68, 13);
            this.lblDireccionIP.TabIndex = 0;
            this.lblDireccionIP.Text = "Direccion IP:";
            // 
            // txtBoxPuerto
            // 
            this.txtBoxPuerto.Location = new System.Drawing.Point(145, 59);
            this.txtBoxPuerto.Mask = "00000";
            this.txtBoxPuerto.Name = "txtBoxPuerto";
            this.txtBoxPuerto.PromptChar = ' ';
            this.txtBoxPuerto.Size = new System.Drawing.Size(145, 20);
            this.txtBoxPuerto.TabIndex = 3;
            this.txtBoxPuerto.Text = "2500";
            this.txtBoxPuerto.ValidatingType = typeof(int);
            // 
            // txtBoxMensajes
            // 
            this.txtBoxMensajes.Location = new System.Drawing.Point(27, 140);
            this.txtBoxMensajes.Multiline = true;
            this.txtBoxMensajes.Name = "txtBoxMensajes";
            this.txtBoxMensajes.Size = new System.Drawing.Size(299, 201);
            this.txtBoxMensajes.TabIndex = 6;
            // 
            // VentanaPrincipalDNS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 353);
            this.Controls.Add(this.txtBoxMensajes);
            this.Controls.Add(this.txtBoxPuerto);
            this.Controls.Add(this.txtBoxDireccionIP);
            this.Controls.Add(this.lblPuerto);
            this.Controls.Add(this.btnDetenerServidor);
            this.Controls.Add(this.btnIniciarServidor);
            this.Controls.Add(this.lblDireccionIP);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "VentanaPrincipalDNS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ventana Principal DNS";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBoxDireccionIP;
        private System.Windows.Forms.Label lblPuerto;
        private System.Windows.Forms.Button btnDetenerServidor;
        private System.Windows.Forms.Button btnIniciarServidor;
        private System.Windows.Forms.Label lblDireccionIP;
        private System.Windows.Forms.MaskedTextBox txtBoxPuerto;
        private System.Windows.Forms.TextBox txtBoxMensajes;

    }
}