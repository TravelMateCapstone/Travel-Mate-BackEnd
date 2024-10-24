using Microsoft.AspNetCore.Mvc;

namespace TravelMateAPI.Controllers
{
    public class GroupPostsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
