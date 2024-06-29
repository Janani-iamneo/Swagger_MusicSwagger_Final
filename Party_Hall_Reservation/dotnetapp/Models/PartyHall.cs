// PartyHall.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace dotnetapp.Models
{
    public class PartyHall
    {
        public int PartyHallID { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool Availability { get; set; }
        public List<Booking> Bookings { get; set; }
    }
}
