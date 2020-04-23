using System;
using SimpleAdsNew.Data;

namespace SimpleAdsNew.Models
{
    public class AdViewModel
    {
        public SimpleAd Ad { get; set; }
        public bool CanDelete { get; set; }
    }
}