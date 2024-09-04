using System.ComponentModel.DataAnnotations;

namespace SonaBookings.Models
{
    public class Capacity
    {
        [Key]
        public int CapacityId { get; set; }
        [Required]
        [StringLength(200)]
        public string? CapacityName { get; set; }
    }
}
