using System;
using System.ComponentModel.DataAnnotations;

namespace BookingOct.Models
{
    public class Booking
    {
        public int Id { get; set; }
        [Required]
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }
        public string UserId { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}