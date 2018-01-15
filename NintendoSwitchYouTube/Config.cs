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

        [JsonProperty("http_port")]
        public int HttpPort { get; set; }

        [JsonProperty("proxy_port")]
        public int ProxyPort { get; set; }

        private Config()
        {
        }

        public void Load()
        {
            Instance = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
        }
    }
}
