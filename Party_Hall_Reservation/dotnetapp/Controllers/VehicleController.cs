using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnetapp.Models;
using dotnetapp.Data;

namespace dotnetapp.Controllers
{
    public class VehicleController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public VehicleController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var vehicles = _dbContext.Vehicles.ToList();
            return View(vehicles);
        }

        public IActionResult Delete(int vehicleId)
        {
            var vehicle = _dbContext.Vehicles.FirstOrDefault(v => v.VehicleID == vehicleId);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int vehicleId)
        {
            var vehicle = _dbContext.Vehicles.FirstOrDefault(v => v.VehicleID == vehicleId);
            if (vehicle == null)
            {
                return NotFound();
            }

            _dbContext.Vehicles.Remove(vehicle);
            _dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // public IActionResult Search(string make)
        // {
        //     if (string.IsNullOrEmpty(make))
        //     {
        //         // Handle case where make is null or empty
        //         return RedirectToAction(nameof(Index));
        //     }

        //     // Convert search string to lower case for case-insensitive comparison
        //     var lowerMake = make.ToLower();

        //     var vehicles = _dbContext.Vehicles                                                              
        //         .Where(v => EF.Functions.Like(v.Make.ToLower(), "%" + lowerMake + "%")) // Using EF.Functions.Like for wildcard search
        //         .ToList();

        //     // Check if any vehicle makes exactly match the search string
        //     var exactMatch = vehicles.FirstOrDefault(v => v.Make.ToLower() == lowerMake);

        //     if (exactMatch == null)
        //     {
        //         // Handle case where no exact match is found
        //         TempData["Message"] = $"No vehicle found matching '{make}'.";
        //         return RedirectToAction(nameof(Index));
        //     }

        //     return View(nameof(Index), vehicles);
        // }

        public IActionResult FilterByYear(int year)
        {
            var vehicles = _dbContext.Vehicles
                .Where(v => v.Year == year)
                .ToList();

            if (vehicles.Count == 0)
            {
                TempData["Message"] = $"No vehicles found manufactured in {year}.";
                return RedirectToAction(nameof(Index));
            }

            return View(nameof(Index), vehicles);
        }

    }
}
