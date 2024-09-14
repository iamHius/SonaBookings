using System.ComponentModel.DataAnnotations;

namespace SonaBookings.Areas.Admin.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
