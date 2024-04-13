using System.Web.Mvc;
using System.Configuration;

namespace AndroidWebService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string currentIp = ConfigurationManager.AppSettings["current_ip"].Trim();
            string currentPort = ConfigurationManager.AppSettings["current_port"].Trim();

            return Redirect($"http://{currentIp}:{currentPort}/swagger");
        }
    }
}
