﻿namespace Chat
{
    partial class SubirArchivo
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
            this.txtBoxArchivo = new System.Windows.Forms.TextBox();
            this.btnElegirArchivo = new System.Windows.Forms.Button();
            this.btnSubir = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtBoxArchivo
            // 
            this.txtBoxArchivo.Location = new System.Drawing.Point(11, 15);
            this.txtBoxArchivo.Name = "txtBoxArchivo";
            this.txtBoxArchivo.Size = new System.Drawing.Size(235, 20);
            this.txtBoxArchivo.TabIndex = 0;
            // 
            // btnElegirArchivo
            // 
            this.btnElegirArchivo.Location = new System.Drawing.Point(262, 13);
            this.btnElegirArchivo.Name = "btnElegirArchivo";
            this.btnElegirArchivo.Size = new System.Drawing.Size(97, 23);
            this.btnElegirArchivo.TabIndex = 1;
            this.btnElegirArchivo.Text = "Elegir Archivo ...";
            this.btnElegirArchivo.UseVisualStyleBackColor = true;
            this.btnElegirArchivo.Click += new System.EventHandler(this.btnElegirArchivo_Click);
            // 
            // btnSubir
            // 
            this.btnSubir.Location = new System.Drawing.Point(262, 56);
            this.btnSubir.Name = "btnSubir";
            this.btnSubir.Size = new System.Drawing.Size(97, 23);
            this.btnSubir.TabIndex = 2;
            this.btnSubir.Text = "Subir";
            this.btnSubir.UseVisualStyleBackColor = true;
            this.btnSubir.Click += new System.EventHandler(this.btnSubir_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.Location = new System.Drawing.Point(149, 100);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(75, 23);
            this.btnCerrar.TabIndex = 3;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(13, 56);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(233, 23);
            this.progressBar.TabIndex = 4;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(259, 105);
            this.lblStatus.MaximumSize = new System.Drawing.Size(97, 13);
            this.lblStatus.MinimumSize = new System.Drawing.Size(97, 13);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(97, 13);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SubirArchivo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 137);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.btnSubir);
            this.Controls.Add(this.btnElegirArchivo);
            this.Controls.Add(this.txtBoxArchivo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SubirArchivo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Subir Archivo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SubirArchivo_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBoxArchivo;
        private System.Windows.Forms.Button btnElegirArchivo;
        private System.Windows.Forms.Button btnSubir;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblStatus;
    }
}