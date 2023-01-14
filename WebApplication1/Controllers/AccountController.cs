using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly LibrarieContext _librarieContext;

        public AccountController(LibrarieContext librarieContext)
        {
            _librarieContext = librarieContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel model, string ReturnUrl)
        {
            // Verific daca utilizatorul a introdus valori in ambele campuri: Email si Parola
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Verific daca in baza de date exista User cu aceste credentiale (Email si Parola).
            var user = await _librarieContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email.Equals(model.Email) && x.Password.Equals(model.Password));
            if (user == null)
            {
                // Daca in baza de date nu exista un User cu aceste credentiale, atunci returnez un mesaj de eroare sugestiv.
                ModelState.AddModelError("", "Userul nu exista.");

                return View(model);
            }

            // Daca firul de executie a ajuns pana aici rezulta ca in baza de date exista un User cu aceste credentaile.
            // Fac LogIn la Userul gasit in baza de date.
            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var claimsIdentity = new ClaimsIdentity(claim, "Cookie and Claims");
            var claimsPrincipal = new ClaimsPrincipal(new[] { claimsIdentity });
            await HttpContext.SignInAsync(claimsPrincipal);

            // Redirectionez call-ul spre pagina necesara.
            if (!String.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                return Redirect(ReturnUrl);
            else
                return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("AdministratorAuth");

            return RedirectToAction("Index", "Home");
        }
    }
}
