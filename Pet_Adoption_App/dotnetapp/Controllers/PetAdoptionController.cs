using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnetapp.Exceptions;
using dotnetapp.Models;
using dotnetapp.Data;

namespace dotnetapp.Controllers
{
    public class PetAdoptionController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public PetAdoptionController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult PetAdopter(int petId)
        {
            var pet = _dbContext.Pets.FirstOrDefault(p => p.PetID == petId);

            if (pet == null)
            {
                return NotFound();
            }
            
            return View();
        }

        [HttpPost]
        public IActionResult PetAdopter(int petId, PetAdopter adopter)
        {
            try
            { 
                var pet = _dbContext.Pets.FirstOrDefault(p => p.PetID == petId);

                if (pet == null)
                {
                    return NotFound();
                }

                // Check if the pet is available for adoption
                if (!pet.Availability)
                {
                    throw new PetAdoptionException("The selected pet is not available for adoption.");
                }

                // Assign Pet ID to the adopter
                adopter.PetID = petId;

                if (!ModelState.IsValid)
                {
                    return View(adopter);
                }

                // Mark the pet as adopted
                pet.Availability = false;
                _dbContext.SaveChanges();

                // Add adopter to the database
                _dbContext.PetAdopters.Add(adopter);
                _dbContext.SaveChanges();

                // Redirect to adoption details page
                return RedirectToAction("Details", new { adopterId = adopter.PetAdopterID });
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                throw;
            }
        }

        public IActionResult Details(int adopterId)
        {
            var adopter = _dbContext.PetAdopters
                .Include(a => a.Pet)
                .FirstOrDefault(a => a.PetAdopterID == adopterId);

            if (adopter == null)
            {
                return NotFound();
            }

            return View(adopter);
        }
    }
}
