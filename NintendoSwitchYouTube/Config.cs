using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NintendoSwitchYouTube
{
    class Config
    {
        public static Config Instance { get; private set; } = new Config();

        [JsonProperty("youtube_api_key")]
        public string YouTubeApiKey { get; set; }

        [JsonProperty("ip_address")]
        public string IpAddress { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        private Config()
        {
        }

        public void Load()
        {
            Instance = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
        }
    }
}
