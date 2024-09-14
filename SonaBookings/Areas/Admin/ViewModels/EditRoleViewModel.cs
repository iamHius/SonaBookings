using System.ComponentModel.DataAnnotations;

namespace SonaBookings.Areas.Admin.ViewModels
{
    public class EditRoleViewModel
    {
        
        public int Id {  get; set; }
        [Required(ErrorMessage ="Role Name is required")]
        public string RoleName { get; set; }
        public List<string> Users { get; set; }
    }
}
