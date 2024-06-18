namespace AndroidWebService.Controllers
{
    using System.Web.Mvc;
    using AndroidWebService.Models.Utils;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Redirect($"{WebURL.GetWebURL()}/swagger");
        }
    }
}
