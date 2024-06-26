// Booking.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace dotnetapp.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        public int VehicleID { get; set; }
        public Vehicle? Vehicle { get; set; }
        public string CustomerName { get; set; }
        public string ContactNumber { get; set; }
        public int DurationInMinutes { get; set; }
    }
}

