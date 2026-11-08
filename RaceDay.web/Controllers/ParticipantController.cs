using Microsoft.AspNetCore.Mvc;

namespace RaceDay.Web.Controllers
{
    public class EnrolmentModel
    {
        public string EventName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public DateTime EnrolmentDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Confirmed";
    }

    public static class EnrolmentStore
    {
        public static List<EnrolmentModel> Enrolments { get; set; } = new();
    }

    public class ParticipantController : Controller
    {
        public IActionResult MyEnrolments()
        {
            if (!UserStore.IsLoggedIn)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(EnrolmentStore.Enrolments);
        }

        [HttpPost]
        public IActionResult Enrol(string eventName, string categoryName)
        {
            // Redirect to Login if user is not authenticated
            if (!UserStore.IsLoggedIn)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!string.IsNullOrEmpty(eventName))
            {
                if (!EnrolmentStore.Enrolments.Any(e => e.EventName == eventName))
                {
                    EnrolmentStore.Enrolments.Add(new EnrolmentModel
                    {
                        EventName = eventName,
                        CategoryName = string.IsNullOrEmpty(categoryName) ? "General Open" : categoryName,
                        EnrolmentDate = DateTime.Now,
                        Status = "Confirmed"
                    });
                }
            }

            return RedirectToAction(nameof(MyEnrolments));
        }

        public IActionResult MyResults() => View();

        public IActionResult Profile()
        {
            if (!UserStore.IsLoggedIn)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.FullName = UserStore.FullName;
            ViewBag.Email = UserStore.Email;
            return View();
        } 

        [HttpPost] 
        public IActionResult Profile(IFormCollection form)
        {
            UserStore.FullName = form["FullName"];
            UserStore.Email = form["Email"];
            return RedirectToAction(nameof(Profile));
        }
    }
}