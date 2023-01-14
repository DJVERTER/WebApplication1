using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        private readonly LibrarieContext _librarieContext;

        public UserController(LibrarieContext librarieContext)
        {
            _librarieContext = librarieContext;
        }

        public IActionResult Inregistrare()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Inregistrare(UserInregistrare model, string ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Verific daca nu cumva deja exista un User cu asa credentiale in baza de date.
            var user = await _librarieContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email.Equals(model.Email) && x.Password.Equals(model.Password));

            if (user != null)
            {
                // Daca in baza de date deja exista un User cu asa credentiale atunci afisez un mesaj de eroare sugestiv.
                ModelState.AddModelError("", "Acest User exista deja. Va rogum sa alegeti alte credentiale.");

                return View(model);
            }

            // Salvez noul User in baza de date.
            await _librarieContext.Users.AddAsync(
                new User
                {
                    Email = model.Email,
                    Password = model.Password,
                    Role = model.Role
                });
            await _librarieContext.SaveChangesAsync();

            // Fac LogIn la noul User.
            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Email),
                new Claim(ClaimTypes.Role, model.Role)
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
    }
}
