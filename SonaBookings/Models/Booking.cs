using SonaBookings.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SonaBookings.Models
{
    public class Reservation
    {
        public List<Booking> Books { get; set; } = new List<Booking>();
        public void AddBooking(Room room)
        {
            Booking? book = Books.Where(r => r.Room.RoomId == room.RoomId).FirstOrDefault();
            if (book == null)
            {
                Books.Add(new Booking()
                {
                    Room = room,

                });
            }

        }
    }
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
        public DateTime? BookingDate { get; set; }
        [ForeignKey("BookingStatus")]
        public string? Status { get; set; }

        public ApplicationUser? User { get; set; }
        public Room? Room { get; set; } = new Room();
    }
}
