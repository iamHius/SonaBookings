using Microsoft.AspNetCore.Mvc;

namespace SonaBookings.Components
{
    public class AboutUsSection : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("Index");
        }
    }
}
