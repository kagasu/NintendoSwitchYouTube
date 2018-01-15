using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Fiddler;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace NintendoSwitchYouTube
{
    public class Program
    {
        static HttpClient client = new HttpClient();

        public static void Main(string[] args)
        {
            Config.Instance.Load();

            FiddlerApplication.BeforeRequest += FiddlerApplication_BeforeRequest;
            FiddlerApplication.BeforeResponse += FiddlerApplication_BeforeResponse;
            
            FiddlerApplication.Startup(Config.Instance.ProxyPort, false, false, true);
                        
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls($"http://{Config.Instance.IpAddress}:{Config.Instance.HttpPort}/")
                .Build();

        private static void FiddlerApplication_BeforeResponse(Session oSession)
        {
            if (oSession.fullUrl.Contains("http://sega.jp/"))
            {
                oSession.ResponseHeaders.SetStatus(200, "OK");
                oSession.ResponseHeaders.Remove("Location");

                var bytes = client.GetByteArrayAsync($"http://127.0.0.1:{Config.Instance.HttpPort}{new Uri(oSession.fullUrl).PathAndQuery}").Result;
                oSession.ResponseHeaders["Content-Length"] = bytes.Length.ToString();
                oSession.responseBodyBytes = bytes;
            }
        }

        private static void FiddlerApplication_BeforeRequest(Session oSession)
        {
            if (oSession.fullUrl.Contains("http://sega.jp/"))
            {
                oSession.bBufferResponse = true;
            }
        }
    }
}
