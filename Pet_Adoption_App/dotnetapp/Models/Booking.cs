// Booking.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace dotnetapp.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        public int PartyHallID { get; set; } 
        public PartyHall? PartyHall { get; set; }
        public string CustomerName { get; set; }
        public string ContactNumber { get; set; }
        public int DurationInMinutes { get; set; }
    }
}
