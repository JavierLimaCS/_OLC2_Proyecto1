using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proyecto1.Analisis;
using Proyecto2.Optimización;

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
                    var proc = Process.Start(@"cmd.exe ", @"/c C:\compiladores2\reporteErrores.html");
                }
                else if (this.comboBox1.SelectedItem.ToString().Equals("Reporte Tabla de Simbolos"))
                {
                    var proc = Process.Start(@"cmd.exe ", @"/c C:\compiladores2\TS_Global.html");
                }
                else if (this.comboBox1.SelectedItem.ToString().Equals("Reporte AST"))
                {
                    var proc = Process.Start(@"cmd.exe ", @"/c C:\compiladores2\ast_report.png");
                }
            }
            else {
                MessageBox.Show(" Advertencia: \n Seleccione una opcion de reporte", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string cadena = this.richTextBox1.Text;
            Analizador parser = new Analizador(this.richTextBox2);
            parser.Analizar(cadena);
            if (!parser.consola.Equals("")) this.richTextBox2.Text = parser.consola;
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
            String cadena = this.richTextBox1.Text.ToLower();
            String file = this.label1.Text;
            Traductor translator = new Traductor();
            translator.Traducir(cadena, file);
            this.richTextBox2.Text = translator.console;

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            String cadena = this.richTextBox1.Text.ToLower();
            String file = this.label1.Text;
            Generador generator = new Generador();
            generator.generar(cadena, this.richTextBox2);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string cadena = this.richTextBox2.Text;
            Optimizador opt = new Optimizador(cadena);
            opt.Optimizacion();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string codigo = this.richTextBox2.Text;
            if (!(codigo.Equals(""))) 
            {
                WindowsClipboard.SetText(codigo);
                MessageBox.Show("Texto copiado al portapapeles", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }

    static class WindowsClipboard
    {
        public static void SetText(string text)
        {
            OpenClipboard();

            EmptyClipboard();
            IntPtr hGlobal = default;
            try
            {
                var bytes = (text.Length + 1) * 2;
                hGlobal = Marshal.AllocHGlobal(bytes);

                if (hGlobal == default)
                {
                    ThrowWin32();
                }

                var target = GlobalLock(hGlobal);

                if (target == default)
                {
                    ThrowWin32();
                }

                try
                {
                    Marshal.Copy(text.ToCharArray(), 0, target, text.Length);
                }
                finally
                {
                    GlobalUnlock(target);
                }

                if (SetClipboardData(cfUnicodeText, hGlobal) == default)
                {
                    ThrowWin32();
                }

                hGlobal = default;
            }
            finally
            {
                if (hGlobal != default)
                {
                    Marshal.FreeHGlobal(hGlobal);
                }

                CloseClipboard();
            }
        }

        public static void OpenClipboard()
        {
            var num = 10;
            while (true)
            {
                if (OpenClipboard(default))
                {
                    break;
                }

                if (--num == 0)
                {
                    ThrowWin32();
                }

                Thread.Sleep(100);
            }
        }

        const uint cfUnicodeText = 13;

        static void ThrowWin32()
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GlobalUnlock(IntPtr hMem);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseClipboard();

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetClipboardData(uint uFormat, IntPtr data);

        [DllImport("user32.dll")]
        static extern bool EmptyClipboard();
    }
}
