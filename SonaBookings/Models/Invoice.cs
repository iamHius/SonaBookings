using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SonaBookings.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }
        [ForeignKey("Booking")]
        public int BookingId { get; set; } 

        public DateTime InvoiceDate { get; set; }
        [Column(TypeName = "decimal(8,0)")]
        public decimal? InvoiceAmount { get; set; }
        public bool IsPaid { get; set; }

        public Booking? Booking { get; set; }
    }
}
