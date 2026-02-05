using BookingOct.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingOct.Models
{
    public class Room
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}