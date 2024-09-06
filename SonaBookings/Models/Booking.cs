using SonaBookings.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SonaBookings.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }
        [ForeignKey("Room")]
        public int RoomId { get; set; }
        [Required]
        public DateTime? CheckInDate { get; set; }
        [Required]
        public DateTime? CheckOutDate { get; set; }
        [Required]
        public DateTime? BookingDate { get; set; } = DateTime.Now;
        [ForeignKey("BookingStatus")]
        public string? Status { get; set; }

        public ApplicationUser? User { get; set; }
        public Room? Room { get; set; }
    }
}
