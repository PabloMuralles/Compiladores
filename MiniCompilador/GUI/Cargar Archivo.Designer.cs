namespace MiniCompilador.GUI
{
    partial class Cargar_Archivo
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
            this.components = new System.ComponentModel.Container();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textomostrar = new System.Windows.Forms.TextBox();
            this.button_Analizar = new System.Windows.Forms.Button();
            this.path = new System.Windows.Forms.TextBox();
            this.button_Cargar = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 101);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "Contenido";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 10;
            this.label1.Text = "Direción";
            // 
            // textomostrar
            // 
            this.textomostrar.Location = new System.Drawing.Point(16, 126);
            this.textomostrar.Margin = new System.Windows.Forms.Padding(4);
            this.textomostrar.Multiline = true;
            this.textomostrar.Name = "textomostrar";
            this.textomostrar.ReadOnly = true;
            this.textomostrar.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textomostrar.Size = new System.Drawing.Size(599, 350);
            this.textomostrar.TabIndex = 9;
            // 
            // button_Analizar
            // 
            this.button_Analizar.Location = new System.Drawing.Point(382, 483);
            this.button_Analizar.Margin = new System.Windows.Forms.Padding(4);
            this.button_Analizar.Name = "button_Analizar";
            this.button_Analizar.Size = new System.Drawing.Size(125, 35);
            this.button_Analizar.TabIndex = 8;
            this.button_Analizar.Text = "Analizar";
            this.button_Analizar.UseVisualStyleBackColor = true;
            this.button_Analizar.Click += new System.EventHandler(this.button_Analizar_Click);
            // 
            // path
            // 
            this.path.Location = new System.Drawing.Point(13, 39);
            this.path.Margin = new System.Windows.Forms.Padding(4);
            this.path.Multiline = true;
            this.path.Name = "path";
            this.path.ReadOnly = true;
            this.path.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.path.Size = new System.Drawing.Size(599, 54);
            this.path.TabIndex = 7;
            this.path.WordWrap = false;
            // 
            // button_Cargar
            // 
            this.button_Cargar.Location = new System.Drawing.Point(113, 484);
            this.button_Cargar.Margin = new System.Windows.Forms.Padding(4);
            this.button_Cargar.Name = "button_Cargar";
            this.button_Cargar.Size = new System.Drawing.Size(125, 34);
            this.button_Cargar.TabIndex = 12;
            this.button_Cargar.Text = "Cargar";
            this.button_Cargar.UseVisualStyleBackColor = true;
            this.button_Cargar.Click += new System.EventHandler(this.button_Cargar_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // Cargar_Archivo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 540);
            this.Controls.Add(this.button_Cargar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textomostrar);
            this.Controls.Add(this.button_Analizar);
            this.Controls.Add(this.path);
            this.Name = "Cargar_Archivo";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textomostrar;
        private System.Windows.Forms.Button button_Analizar;
        private System.Windows.Forms.TextBox path;
        private System.Windows.Forms.Button button_Cargar;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}