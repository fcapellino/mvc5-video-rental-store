using System.Web.Mvc;

namespace VideoRentalStore.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToActionPermanent("Index", "Rentals");
        }
    }
}
