﻿using Minic.Análisis_sintactico;
using System;
using System.IO;
using System.Windows.Forms;

namespace MiniCompilador.GUI
{
    public partial class Cargar_Archivo : Form
    {
        private string Direccion = string.Empty;

        //private bool ExtencionValidar = false;
        public Cargar_Archivo()
        {
            InitializeComponent();

        }

        /// <summary>
        /// action del boton para cargar archivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Cargar_Click(object sender, EventArgs e)
        {
            // variable para poder abrir el dialog
            OpenFileDialog Abrir = new OpenFileDialog();

            // abre el explorador de archivos   
            if (Abrir.ShowDialog() == DialogResult.OK)
            {
                // da la direccion del archivo que se abrio
                Direccion = Abrir.FileName;

                /*valido la extencion del archivo y si es txt lo leo para posteriomente 
                 * guardarlo y sino es un txt se muestra un messaje*/

                try
                {
                    errorProvider1.Clear();
                    var ArchivoEnseñar = new StreamReader(Direccion);
                    if (ArchivoEnseñar.BaseStream.Length != 0)
                    {
                        //ExtencionValidar = true;
                        path.Text = Direccion;
                        textomostrar.Text = ArchivoEnseñar.ReadToEnd();
                        ArchivoEnseñar.Close();
                    }
                    else
                    {
                        throw new Exception("Archivo vacio");
                    }

                }
                catch (Exception p)
                {
                    errorProvider1.Clear();
                    errorProvider1.SetError(path, p.Message);
                }
            }
        }

        private void button_Analizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(path.Text))
                {
                    errorProvider1.Clear();
                    Análisis_Léxico.Analisis analisis = new Análisis_Léxico.Analisis();
                    analisis.LecturaArchivo(path.Text);
                }
                else
                {
                    throw new Exception("No se a cargado archivo");
                }

            }
            catch (Exception p)
            {
                errorProvider1.Clear();
                errorProvider1.SetError(path, p.Message);
            }
        }
        public void msg_Analyze_lexicon(string mensaje, string direccion)
        {
            MessageBox.Show(mensaje + " \n " + $"Mas informacion en {direccion}.out");
        }
        public void msg_Analyze_syntactic(string msg)
        {
            MessageBox.Show(msg + " \n ");
        }
        public void msg_Analyze_semantic(string msg)
        {
            MessageBox.Show(msg + " \n ");
        }
        private void Cargar_Archivo_Load(object sender, EventArgs e)
        {

        }
    }
}
