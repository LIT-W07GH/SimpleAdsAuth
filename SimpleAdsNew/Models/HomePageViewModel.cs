using System.Collections.Generic;

namespace SimpleAdsNew.Models
{
    public class HomePageViewModel
    {
        public IEnumerable<AdViewModel> Ads { get; set; }
        public string Message { get; set; }
    }
}