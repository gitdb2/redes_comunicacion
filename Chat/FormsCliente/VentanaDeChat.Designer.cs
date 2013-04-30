namespace Chat
{
    partial class VentanaDeChat
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
            this.txtBoxChat = new System.Windows.Forms.TextBox();
            this.txtBoxMensaje = new System.Windows.Forms.TextBox();
            this.btnEnviarMensaje = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtBoxChat
            // 
            this.txtBoxChat.BackColor = System.Drawing.SystemColors.Window;
            this.txtBoxChat.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtBoxChat.Location = new System.Drawing.Point(12, 5);
            this.txtBoxChat.Multiline = true;
            this.txtBoxChat.Name = "txtBoxChat";
            this.txtBoxChat.ReadOnly = true;
            this.txtBoxChat.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBoxChat.Size = new System.Drawing.Size(324, 283);
            this.txtBoxChat.TabIndex = 1;
            // 
            // txtBoxMensaje
            // 
            this.txtBoxMensaje.Location = new System.Drawing.Point(12, 297);
            this.txtBoxMensaje.Name = "txtBoxMensaje";
            this.txtBoxMensaje.Size = new System.Drawing.Size(226, 20);
            this.txtBoxMensaje.TabIndex = 0;
            this.txtBoxMensaje.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBoxMensaje_KeyPress);
            // 
            // btnEnviarMensaje
            // 
            this.btnEnviarMensaje.Location = new System.Drawing.Point(244, 294);
            this.btnEnviarMensaje.Name = "btnEnviarMensaje";
            this.btnEnviarMensaje.Size = new System.Drawing.Size(92, 25);
            this.btnEnviarMensaje.TabIndex = 2;
            this.btnEnviarMensaje.Text = "Enviar";
            this.btnEnviarMensaje.UseVisualStyleBackColor = true;
            this.btnEnviarMensaje.Click += new System.EventHandler(this.btnEnviarMensaje_Click);
            // 
            // VentanaDeChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 327);
            this.Controls.Add(this.btnEnviarMensaje);
            this.Controls.Add(this.txtBoxMensaje);
            this.Controls.Add(this.txtBoxChat);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "VentanaDeChat";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ventana De Chat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VentanaDeChat_FormClosing);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBoxMensaje_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBoxChat;
        private System.Windows.Forms.TextBox txtBoxMensaje;
        private System.Windows.Forms.Button btnEnviarMensaje;
    }
}