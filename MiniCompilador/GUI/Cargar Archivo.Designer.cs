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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textomostrar = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.path = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 101);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 21);
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(332, 484);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 35);
            this.button1.TabIndex = 8;
            this.button1.Text = "Analizar";
            this.button1.UseVisualStyleBackColor = true;
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
            // Cargar_Archivo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 532);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textomostrar);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.path);
            this.Name = "Cargar_Archivo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textomostrar;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox path;
    }
}