using Microsoft.AspNetCore.Mvc;

namespace RaceDay.Web.Controllers
{
    public static class UserStore
    {
        public static string FullName { get; set; } = string.Empty;
        public static string Email { get; set; } = string.Empty;
        public static string Role { get; set; } = "Participant";
        public static bool IsLoggedIn { get; set; } = false;
    }

    public class AccountController : Controller
    {
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(IFormCollection form)
        {
            UserStore.Email = form["Email"];
            if (string.IsNullOrEmpty(UserStore.FullName))
            {
                UserStore.FullName = form["Email"].ToString().Split('@')[0];
            }
            UserStore.IsLoggedIn = true;
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(IFormCollection form)
        {
            UserStore.FullName = form["FullName"];
            UserStore.Email = form["Email"];
            UserStore.Role = form["Role"];
            UserStore.IsLoggedIn = true; // Auto log-in after registration

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            UserStore.IsLoggedIn = false;
            UserStore.FullName = string.Empty;
            UserStore.Email = string.Empty;
            return RedirectToAction("Index", "Home");
        }
    }
}