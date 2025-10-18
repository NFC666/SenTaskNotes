using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using SenNotes.Common.Global;

namespace SenNotes.Services
{
    public class ConfigService
    {
        public AiInfo? GetSettings()
        {
            string json = File.ReadAllText("Configs/ai_client_config.json");
            return JsonConvert.DeserializeObject<AiInfo>(json);
        }
        public void SaveSettings(AiInfo aiInfo)
        {
            string json = JsonConvert.SerializeObject(aiInfo);
            File.WriteAllText("Configs/ai_client_config.json", json);
        }
    }
}
