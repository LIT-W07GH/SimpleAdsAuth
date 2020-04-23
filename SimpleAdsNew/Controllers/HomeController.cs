using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleAdsNew.Data;
using SimpleAdsNew.Models;

namespace SimpleAdsNew.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString =
            "Data Source=.\\sqlexpress;Initial Catalog=SimpleAdsAuth;Integrated Security=True";
        public IActionResult Index()
        {
            SimpleAdDb db = new SimpleAdDb(_connectionString);
            IEnumerable<SimpleAd> ads = db.GetAds();
            var currentUserId = GetCurrentUserId();
            var vm = new HomePageViewModel
            {
                Ads = ads.Select(ad => new AdViewModel
                {
                    Ad = ad,
                    CanDelete = currentUserId != null && ad.UserId == currentUserId
                })
            };

            return View(vm);
        }

        [Authorize]
        public IActionResult NewAd()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult NewAd(SimpleAd ad)
        {
            var userId = GetCurrentUserId();
            ad.UserId = userId.Value;
            SimpleAdDb db = new SimpleAdDb(_connectionString);
            db.AddSimpleAd(ad);

            return Redirect("/");
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteAd(int id)
        {
            SimpleAdDb db = new SimpleAdDb(_connectionString);
            var userIdForAd = db.GetUserIdForAd(id);
            var currentUserId = GetCurrentUserId().Value;
            if (currentUserId == userIdForAd)
            {
                db.Delete(id);
            }

            return Redirect("/");
        }

        [Authorize]
        public IActionResult MyAccount()
        {
            SimpleAdDb db = new SimpleAdDb(_connectionString);
            var userId = GetCurrentUserId().Value;
            return View(db.GetAdsForUser(userId));
        }

        private int? GetCurrentUserId()
        {
            var userDb = new UserDb(_connectionString);
            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }
            var user = userDb.GetByEmail(User.Identity.Name);
            if (user == null)
            {
                return null;
            }

            return user.Id;
        }
    }


}
