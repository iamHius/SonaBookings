using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SonaBookings.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        [Required]
        [StringLength(200)]
        public string? RoomNo { get; set; }

        [Required]
        [Column(TypeName = "decimal(8,0)")]
        public decimal? FeePerNight { get; set; }
        [Required]
        [StringLength(200)]
        public string? RoomPhoto { get; set; }
        public bool IsAvailable { get; set; } = true;
        [Required]
        [StringLength(400)]
        public string? RoomDescription { get; set; }
        [ForeignKey("RoomType")]
        public int RoomTypeId { get; set; }
        [ForeignKey("Size")]
        public int SizeId { get; set; }
        [ForeignKey("Capacity")]
        public int CapacityId { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
        public RoomType? RoomType { get; set; }
        public Capacity? Capacity { get; set; }
        public Size? Size { get; set; }
    }
}
