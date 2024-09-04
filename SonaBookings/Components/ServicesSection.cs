using Microsoft.AspNetCore.Mvc;

namespace SonaBookings.Components
{
    public class ServicesSection : ViewComponent 
    {
        public IViewComponentResult Invoke()
        {
            return View("Index");
        }
    }
}
