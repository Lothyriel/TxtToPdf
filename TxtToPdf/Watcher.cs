using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace TxtToPdf
{
    public class Watcher
    {
        public static ILogger<Worker> Logger = Worker.Logger;

        private static readonly string pastaEntrada = AppConfigManager.PastaEntrada;
        private static TimeSpan tempoPassado;
        private static long arquivosConvertidos = 0;
        private static long arquivosParaConverter = 0;


        public Watcher()
        {
            Logger.LogInformation("Configurando Watcher:{time}", DateTimeOffset.Now.TimeOfDay);

            if (!Directory.Exists(pastaEntrada))
                Directory.CreateDirectory(pastaEntrada);

            var pastaSaida = AppConfigManager.PastaSaida;
            if (!Directory.Exists(pastaSaida))
                Directory.CreateDirectory(pastaSaida);

            var watcher = new FileSystemWatcher(pastaEntrada);
            Logger.LogInformation("Escutando a pasta {path}", pastaEntrada);

            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

            watcher.Created += OnCreated;
            watcher.InternalBufferSize = 65536;
            watcher.Filter = "*.txt";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
        }

        private async void OnCreated(object sender, FileSystemEventArgs e)
        {
            arquivosParaConverter++;

            Logger.LogInformation("Txt encontrado {arquivo} :{time}", e.Name, DateTimeOffset.Now.TimeOfDay);

            Stopwatch sw = Stopwatch.StartNew();

            await Task.Run(() => new PDFtransformer(new FileStream(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)));

            sw.Stop();

            if (sw.Elapsed > tempoPassado)
                tempoPassado = sw.Elapsed;

            arquivosConvertidos++;

            if (arquivosConvertidos == arquivosParaConverter)
            {
                Logger.LogInformation("Conversão de {arquivos} arquivos realizada em : {tempo}", arquivosConvertidos, tempoPassado.ToString());
                arquivosParaConverter = 0;
                arquivosConvertidos = 0;
                tempoPassado = new TimeSpan();
            }
        }
    }
}