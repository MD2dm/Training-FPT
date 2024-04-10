using Microsoft.AspNetCore.Mvc;

namespace TrainingFPTCo.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
