using DNS.Server;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace NintendoSwitchYouTube
{
    public class Program
    {
        private static async void StartDnsServer()
        {
            var masterFile = new MasterFile();
            var server = new DnsServer(masterFile, Config.Instance.DNSUpstreamIpAddress);
            masterFile.AddIPAddressResourceRecord("sega.jp", Config.Instance.DNSRedirectIpAddress);
            await server.Listen();
        }

        public static void Main(string[] args)
        {
            Config.Instance.Load();
            Task.Run(() => StartDnsServer());
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls($"http://{Config.Instance.WebServerIpAddress}:{Config.Instance.Port}/")
                .Build();
    }
}
