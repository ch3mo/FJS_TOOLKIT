using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
using Ghostscript.NET.Processor;


namespace FJS_TOOLKIT
{
    public partial class FJS_PDF_2_JPG : Form
    {
        private GhostscriptRasterizer _rasterizer;
        public FJS_PDF_2_JPG()
        {
            InitializeComponent();
            this.AllowDrop = true;
            this.DragEnter += FJS_PDF_2_JPG_DragEnter;
            this.DragDrop += FJS_PDF_2_JPG_DragDrop;

            // Initialize the GhostscriptRasterizer with desired width and height
            _rasterizer = new GhostscriptRasterizer();
            _rasterizer.GraphicsAlphaBits = 4;
            _rasterizer.TextAlphaBits = 4;
        }

        private void FJS_PDF_2_JPG_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void FJS_PDF_2_JPG_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void FJS_PDF_2_JPG_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files.Length > 0)
            {
                string pdfFilePath = files[0];

                if (File.Exists(pdfFilePath) && pdfFilePath.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    // Automatically convert the dropped PDF file to PNG and save it in the application's directory
                    ConvertPdfToPng(pdfFilePath);
                }
            }
        }

        private void ConvertPdfToPng(string pdfFilePath)
        {
            string outputDirectory = Path.Combine(Application.StartupPath, "PDF2PNG");

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Use the Ghostscript command-line tool to convert PDF to PNG for each page
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "gswin64c.exe", // Make sure Ghostscript is installed and its executable is in the PATH
                Arguments = $"-sDEVICE=png16m -r300 -o \"{outputDirectory}\\page%d.png\" \"{pdfFilePath}\"",
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process())
            {
                process.StartInfo = psi;
                process.Start();
                process.WaitForExit();

                int exitCode = process.ExitCode;
                process.Close();

                if (exitCode == 0)
                {
                    MessageBox.Show($"PDF converted to PNG images successfully and saved in the 'PDF2PNG' subdirectory!");
                }
                else
                {
                    MessageBox.Show($"Error converting PDF to PNG. Exit code: {exitCode}");
                }
            }
        }
    }
}
