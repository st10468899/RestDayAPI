using Microsoft.AspNetCore.Mvc;

namespace RaceDay.Web.Controllers
{
    public class EventModel
    {
        public string Name { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public double Distance { get; set; }
    }

    public static class EventStore
    {
        public static List<EventModel> Events { get; set; } = new()
        {
            new EventModel { Name = "City Grand Marathon", EventType = "Marathon", Date = "25 Nov 2026", Location = "Central Stadium", Distance = 42.2 },
            new EventModel { Name = "Coastal Ridge Challenge", EventType = "Trail Run", Date = "10 Dec 2026", Location = "Coastal Reserve", Distance = 15.0 },
            new EventModel { Name = "Sunset Beach Sprint", EventType = "Sprint", Date = "05 Jan 2027", Location = "Promenade Beach", Distance = 5.0 },
            new EventModel { Name = "Mountain Peak Ultra", EventType = "Trail Run", Date = "20 Feb 2027", Location = "Highland National Park", Distance = 50.0 },
            new EventModel { Name = "Metro Night Cycling Tour", EventType = "Cycling", Date = "12 Mar 2027", Location = "Downtown Arena", Distance = 80.0 },
            new EventModel { Name = "Autumn Forest Half Marathon", EventType = "Marathon", Date = "18 Apr 2027", Location = "Pine Ridge Reserve", Distance = 21.1 }
        };
    }

    public class OrganiserController : Controller
    {
        public IActionResult Dashboard()
        {
            return View(EventStore.Events);
        }

        public IActionResult CreateEvent() => View();

        [HttpPost]
        public IActionResult CreateEvent(IFormCollection form)
        {
            var newEvent = new EventModel
            {
                Name = form["Name"],
                EventType = form["EventType"],
                Date = form["Date"],
                Location = form["Location"],
                Distance = double.TryParse(form["Distance"], out var d) ? d : 0
            };

            EventStore.Events.Add(newEvent);
            return RedirectToAction(nameof(Dashboard));
        }

        // POST: /Organiser/DeleteEvent
        [HttpPost]
        public IActionResult DeleteEvent(string name)
        {
            var targetEvent = EventStore.Events.FirstOrDefault(e => e.Name == name);
            if (targetEvent != null)
            {
                EventStore.Events.Remove(targetEvent);
            }
            return RedirectToAction(nameof(Dashboard));
        }

        public IActionResult ResultsEntry() => View();
    }
}