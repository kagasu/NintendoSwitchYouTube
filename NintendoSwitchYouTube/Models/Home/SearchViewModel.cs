using Google.Apis.YouTube.v3.Data;
using System.Collections.Generic;

namespace NintendoSwitchYouTube.Models.Home
{
    public class SearchViewModel
    {
        public List<SearchResult> SearchResults { get; set; } = new List<SearchResult>();
    }
}