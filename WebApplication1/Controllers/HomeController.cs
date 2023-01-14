using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    // La acest Controller vor avea acces doar userii care au facut LogIn si care au rolul "Admin".
    [Authorize(Roles = "Admin", AuthenticationSchemes = "AdministratorAuth")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}