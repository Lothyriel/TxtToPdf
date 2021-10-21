using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TxtToPdf
{
    public class Watcher
    {
        public static ILogger<Worker> Logger = Worker.Logger;

        public static FileSystemWatcher Configure()
        {
            Logger.LogInformation("Configurando Watcher:{time}", DateTimeOffset.Now.TimeOfDay);

            var watcher = new FileSystemWatcher(AppConfigManager.PastaWatcher);

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
            return watcher;
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            Logger.LogInformation("Txt encontrado {arquivo} :{time}", e.Name, DateTimeOffset.Now.TimeOfDay);

            Task.Run(() => PDFtransformer.GerarPDF(new FileStream(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)));
        }
    }
}