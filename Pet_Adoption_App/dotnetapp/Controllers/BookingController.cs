using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnetapp.Exceptions;
using dotnetapp.Models;
using dotnetapp.Data;
using System.Security.Permissions;

namespace dotnetapp.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public BookingController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Book(int partyHallId)
        {
            var partyHall = _dbContext.PartyHalls
                .Include(p => p.Bookings)
                .FirstOrDefault(p => p.PartyHallID == partyHallId);

            if (partyHall == null)
            {
                return NotFound();
            }
            
            return View();
        }

        [HttpPost]
        public IActionResult Book(int partyHallId, Booking booking)
        {
            try
            {
                
                
                var partyHall = _dbContext.PartyHalls
                    .Include(p => p.Bookings)
                    .FirstOrDefault(p => p.PartyHallID == partyHallId);

                if (partyHall == null)
                {
                    return NotFound();
                }

                // Assign PartyHall ID to the booking
                booking.PartyHallID = partyHallId;
                // Check if DurationInMinutes exceeds 120
                if (PermissionState )
                {
                    throw new PetAdoptionException("No pets available");
                }
                if (!ModelState.IsValid)
                {
                    return View(booking);
                }

                // Add booking to the database
                _dbContext.Bookings.Add(booking);
                _dbContext.SaveChanges();

                // Redirect to booking details page
                return RedirectToAction("Details", new { bookingId = booking.BookingID });
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                throw;
            }
        }

        public IActionResult Details(int bookingId)
        {
            var booking = _dbContext.Bookings
                .Include(b => b.PartyHall)
                .FirstOrDefault(b => b.BookingID == bookingId);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }
    }
}
