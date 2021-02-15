using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proyecto1.Analisis;

namespace Proyecto1
{
    public partial class Interfaz : Form
    {
        public Interfaz()
        {
            InitializeComponent();
        }

        public int getWidth()
        {
            int w = 25;
            // get total lines of richTextBox1    
            int line = richTextBox1.Lines.Length;

            if (line <= 99)
            {
                w = 20 + (int)richTextBox1.Font.Size;
            }
            else if (line <= 999)
            {
                w = 30 + (int)richTextBox1.Font.Size;
            }
            else
            {
                w = 50 + (int)richTextBox1.Font.Size;
            }

            return w;
        }

        public void AddLineNumbers()
        {
            Point pt = new Point(0, 0);    
            int primer_indice = richTextBox1.GetCharIndexFromPosition(pt);
            int primera_linea = richTextBox1.GetLineFromCharIndex(primer_indice);
            pt.X = ClientRectangle.Width;
            pt.Y = ClientRectangle.Height;
            int Last_Index = richTextBox1.GetCharIndexFromPosition(pt);
            int Last_Line = richTextBox1.GetLineFromCharIndex(Last_Index);
            LineNumberTextBox.SelectionAlignment = HorizontalAlignment.Center;
            LineNumberTextBox.Text = "";
            LineNumberTextBox.Width = getWidth();
            for (int i = primera_linea; i <= Last_Line + 1; i++)
            {
                LineNumberTextBox.Text += i + 1 + "\n";
            }
        }


        private void Interfaz_Load(object sender, EventArgs e)
        {
            LineNumberTextBox.Font = richTextBox1.Font;
            richTextBox1.Select();
            AddLineNumbers();
        }

        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            Point pt = this.richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart);
            if (pt.X == 1)
            {
                AddLineNumbers();
            }
        }

        private void richTextBox1_VScroll(object sender, EventArgs e)
        {
            this.LineNumberTextBox.Text = "";
            AddLineNumbers();
            LineNumberTextBox.Invalidate();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "")
            {
                AddLineNumbers();
            }
            //KeyWords();
        }

        private void KeyWords() 
        {
            string tokens = "(program|;|var|function|begin|end|const|:=|switch|" +
                              "case|repeat|for|do|if|while)";
            Regex rex = new Regex(tokens);
            MatchCollection mc = rex.Matches(richTextBox1.Text);
            int StartCursorPosition = richTextBox1.SelectionStart;
            foreach (Match m in mc)
            {
                int startIndex = m.Index;
                int StopIndex = m.Length;
                this.richTextBox1.Select(startIndex, StopIndex);
                this.richTextBox1.SelectionColor = Color.Navy;
                this.richTextBox1.SelectionStart = StartCursorPosition;
                this.richTextBox1.SelectionColor = Color.Black;
            }
        }


        private void richTextBox1_FontChanged(object sender, EventArgs e)
        {
            LineNumberTextBox.Font = richTextBox1.Font;
            richTextBox1.Select();
            AddLineNumbers();
        }

        private void LineNumberTextBox_MouseDown(object sender, MouseEventArgs e)
        {
            richTextBox1.Select();
            LineNumberTextBox.DeselectAll();
        }

        private void Interfaz_Resize(object sender, EventArgs e)
        {
            AddLineNumbers();
        }

        private void button1_Click(object sender, EventArgs e)
        {   
            MessageBox.Show(" Javier Estuardo Lima Abrego \n 201612098 \n OLC2 \n Seccion B+", "Info. del Estudiante", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedItem != null) 
            {
                if (this.comboBox1.SelectedItem.ToString().Equals("Reporte Errores"))
                {

                }
                else if (this.comboBox1.SelectedItem.ToString().Equals("Reporte Tabla de Simbolos"))
                {

                }
                else if (this.comboBox1.SelectedItem.ToString().Equals("Reporte AST"))
                {
                    System.Diagnostics.Process.Start(@"C:\compiladores2\reporte_ast.png");
                }
            }
            else {
                MessageBox.Show(" Advertencia: \n Seleccione una opcion de reporte", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string cadena = this.richTextBox1.Text;
            Analizador parser = new Analizador();
            parser.Analizar(cadena);
            this.richTextBox2.Text = parser.consola;
        }

        private void Abrir_Click(object sender, EventArgs e)
        {
            OpenFileDialog cuadroDialogo = new OpenFileDialog();
            cuadroDialogo.Title = "Abrir Archivo...";
            cuadroDialogo.Filter = "(*.pas)|*.pas";

            if ((cuadroDialogo.ShowDialog()) == DialogResult.OK)
            {
                try
                {
                    StreamReader lector = new StreamReader(cuadroDialogo.FileName);

                    string texto = lector.ReadToEnd();

                    this.richTextBox1.Text = texto;
                    this.label1.Text = Path.GetFileName(cuadroDialogo.FileName);
                    lector.Close();

                    MessageBox.Show("Archivo " + cuadroDialogo.FileName + " abierto Satisfactoriamente");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("¡Error! Ha habido un problema al intentar abrir el archivo" + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Error al intentar encontrar la dirección");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String cadena = this.richTextBox1.Text;
            String file = this.label1.Text;
            Traductor translator = new Traductor();
            translator.Traducir(cadena, file);
            this.richTextBox2.Text = translator.console;

        }
    }
}
