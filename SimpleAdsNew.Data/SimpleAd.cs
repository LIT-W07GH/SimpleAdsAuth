using System;

namespace SimpleAdsNew.Data
{
    public class SimpleAd
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public string PosterName { get; set; }
        public DateTime Date { get; set; }
    }
}
