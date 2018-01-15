using Newtonsoft.Json;
using System.IO;

namespace NintendoSwitchYouTube
{
    class Config
    {
        public static Config Instance { get; private set; } = new Config();

        [JsonProperty("youtube_api_key")]
        public string YouTubeApiKey { get; set; }

        [JsonProperty("web_server_ip_address")]
        public string WebServerIpAddress { get; set; }

        [JsonProperty("dns_redirect_ip_address")]
        public string DNSRedirectIpAddress { get; set; }

        [JsonProperty("dns_upstream_ip_address")]
        public string DNSUpstreamIpAddress { get; set; }

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
