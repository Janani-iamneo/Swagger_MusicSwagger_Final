using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnetapp.Exceptions;
using dotnetapp.Models;
using dotnetapp.Data;

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
        public IActionResult Book(int vehicleId)
        {
            var vehicle = _dbContext.Vehicles
                .Include(v => v.Bookings)
                .FirstOrDefault(v => v.VehicleID == vehicleId);

            if (vehicle == null)
            {
                return NotFound();
            }
            
            return View();
        }

        [HttpPost]
        public IActionResult Book(int vehicleId, Booking booking)
        {
            try
            {
                var vehicle = _dbContext.Vehicles
                    .Include(v => v.Bookings)
                    .FirstOrDefault(v => v.VehicleID == vehicleId);

                if (vehicle == null)
                {
                    return NotFound();
                }

                // Assign Vehicle ID to the booking
                booking.VehicleID = vehicleId;

                // Check if DurationInMinutes exceeds 200
                if (booking.DurationInMinutes > 200)
                {
                    throw new TestDriveBookingException("Booking duration cannot exceed 200 minutes");
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
                // You can log the exception here or handle it as per your requirements
                throw;
            }
        }

        public IActionResult Details(int bookingId)
        {
            var booking = _dbContext.Bookings
                .Include(b => b.Vehicle)
                .FirstOrDefault(b => b.BookingID == bookingId);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }
    }
}
