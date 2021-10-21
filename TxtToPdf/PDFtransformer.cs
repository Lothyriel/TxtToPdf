using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TxtToPdf
{
    public class PDFtransformer
    {
        public static ILogger<Worker> Logger = Worker.Logger;

        public static void GerarPDF(FileStream arquivo)
        {

            var nomeArquivoCompleto = arquivo.Name.Split('\\').ToList().Last();
            var nomeArquivo = nomeArquivoCompleto.Split('.').ToList().First() + ".pdf";

            Logger.LogInformation("Gerando PDF: {arquivo }{time}", nomeArquivo, DateTimeOffset.Now.TimeOfDay);

            var writer = new PdfWriter(AppConfigManager.PastaPath + $"\\{nomeArquivo}");
            var pdfDocument = new PdfDocument(writer);
            var pdf = new Document(pdfDocument);

            #region Estilos
            PdfFont fontCorpo = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            PdfFont fontHeader = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            Style helvetica32b = new();
            helvetica32b.SetFont(fontHeader).SetFontSize(32);

            Style helvetica20b = new();
            helvetica20b.SetFont(fontHeader).SetFontSize(20);

            Style helvetica14r = new();
            helvetica14r.SetFont(fontCorpo).SetFontSize(14);

            Style helvetica14b = new();
            helvetica14b.SetFont(fontHeader).SetFontSize(14);

            Style helvetica24b = new();
            helvetica24b.SetFont(fontHeader).SetFontSize(24);

            SolidLine linhaStyle = new(1f);
            LineSeparator linhaHorizontal = new(linhaStyle);

            #endregion

            Paragraph header = new Paragraph().SetTextAlignment(TextAlignment.CENTER).AddStyle(helvetica24b);

            foreach (var linha in ReadLines(arquivo, Encoding.UTF8))
                header.Add(new Text(linha));

            pdf.Add(header);
            pdf.Add(linhaHorizontal);

            pdf.Close();
        }
        private static IEnumerable<string> ReadLines(Stream stream, Encoding encoding)
        {
            using (var reader = new StreamReader(stream, encoding))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}
