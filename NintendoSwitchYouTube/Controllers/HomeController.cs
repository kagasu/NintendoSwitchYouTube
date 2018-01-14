using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Mvc;
using NintendoSwitchYouTube.Models;
using NintendoSwitchYouTube.Models.Home;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;

namespace NintendoSwitchYouTube.Controllers
{
    public class HomeController : Controller
    {
        private YouTubeService YoutubeService { get; } = new YouTubeService(new BaseClientService.Initializer() { ApiKey = Config.Instance.YouTubeApiKey});

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Search()
        {
            var searchViewModel = new SearchViewModel();
            var numberOfResults = int.Parse(Request.Query["n"]);
            var pageToken = "";

            foreach (var x in Enumerable.Range(0, (int)Math.Ceiling(numberOfResults / 50.0)))
            {
                var searchListRequest = YoutubeService.Search.List("snippet");
                searchListRequest.Q = Request.Query["q"];
                searchListRequest.Type = "video";
                searchListRequest.MaxResults = (numberOfResults >= 50) ? 50 : numberOfResults;
                searchListRequest.PageToken = (string.IsNullOrEmpty(pageToken)) ? null : pageToken;

                var searchListResponse = await searchListRequest.ExecuteAsync();
                pageToken = searchListResponse.NextPageToken;
                searchViewModel.SearchResults.AddRange(searchListResponse.Items);
            }

            return View(searchViewModel);
        }

        public async Task<IActionResult> VideoStreamUrl()
        {
            var videoId = Request.Query["v"];
            var client = new YoutubeClient();
            var infos = await client.GetVideoMediaStreamInfosAsync(videoId);
            var info = infos
                .Muxed
                .Where(x => x.VideoEncoding == VideoEncoding.H264)
                .Where(X => X.VideoQuality <= VideoQuality.High720)
                .Where(x => x.AudioEncoding == AudioEncoding.Aac)
                .OrderByDescending(x => x.VideoQualityLabel)
                .First();

            return Json(info);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
