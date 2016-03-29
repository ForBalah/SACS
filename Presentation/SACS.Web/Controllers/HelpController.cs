using System.Web.Mvc;

namespace SACS.Web.Controllers
{
    /// <summary>
    /// The help controller class.
    /// </summary>
    public class HelpController : Controller
    {
        /// <summary>
        /// The get action returning the index.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}