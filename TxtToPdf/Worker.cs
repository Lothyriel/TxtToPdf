using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TxtToPdf
{
    public class Worker : BackgroundService
    {
        public static ILogger<Worker> Logger;
        private readonly FileSystemWatcher Watcher;

        public Worker(ILogger<Worker> logger)
        {
            Logger = logger;
            Watcher = TxtToPdf.Watcher.Configure();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //Logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now.TimeOfDay);
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
