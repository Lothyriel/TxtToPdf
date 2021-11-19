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

        public PDFtransformer(FileStream arquivo)
        {
            var nomeArquivoCompleto = arquivo.Name.Split('\\').ToList().Last();
            var nomeArquivo = nomeArquivoCompleto.Split('.').ToList().First() + ".pdf";

            Logger.LogInformation("Gerando PDF: {arquivo } {time}", nomeArquivo, DateTimeOffset.Now.TimeOfDay);

            using var writer = new PdfWriter($"{AppConfigManager.PastaSaida}\\{nomeArquivo}");
            var pdfDocument = new PdfDocument(writer);
            var pdf = new Document(pdfDocument);

            #region Estilos
            PdfFont fontHeader = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            Style helvetica24b = new();
            helvetica24b.SetFont(fontHeader).SetFontSize(24);

            SolidLine linhaStyle = new(1f);
            LineSeparator linhaHorizontal = new(linhaStyle);

            #endregion

            var header = new Paragraph().SetTextAlignment(TextAlignment.CENTER).AddStyle(helvetica24b);

            foreach (var linha in ReadLines(arquivo, Encoding.UTF8))
                header.Add(new Text(linha));

            pdf.Add(header);
            pdf.Add(linhaHorizontal);

            pdf.Close();
            Logger.LogInformation("PDF Gerado: {arquivo } {time}", nomeArquivo, DateTimeOffset.Now.TimeOfDay);
        }

        private static IEnumerable<string> ReadLines(Stream stream, Encoding encoding)
        {
            using var reader = new StreamReader(stream, encoding);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }
    }
}
