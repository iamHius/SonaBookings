using Microsoft.AspNetCore.Mvc;

namespace SonaBookings.Components
{
    public class HeroSection : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("Index");
        }
    }
}
