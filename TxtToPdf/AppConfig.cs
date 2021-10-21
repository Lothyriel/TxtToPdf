using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace TxtToPdf
{
    public class AppConfigManager
    {
        protected const string pathAppConfig = "appsettings.json";

        protected static void Save(JObject newAppConfig)
        {
            File.WriteAllText(pathAppConfig, JsonConvert.SerializeObject(newAppConfig));
        }
        public static JObject AppConfig => JObject.Parse(File.ReadAllText(pathAppConfig));

        public static string PastaWatcher => AppConfig["PathWatcher"].ToString();
        public static string PastaPath => AppConfig["PathPasta"].ToString();

    }
}
