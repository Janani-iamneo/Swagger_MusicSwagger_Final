using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnetapp.Models;
using dotnetapp.Data;
using dotnetapp.Exceptions;


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

            var adoption = new PetAdoption { PetID = petId };
            return View(adoption);
        }

        [HttpPost]
        public IActionResult PetAdopter(PetAdoption adoption)
        {
            var pet = _dbContext.Pets.FirstOrDefault(p => p.PetID == adoption.PetID);
            //Console.WriteLine("entered");
            if (pet == null)
            {
                return NotFound();
            }

            // if (!pet.Availability)
            // {
            //     //Console.WriteLine("!pet");
            //     ModelState.AddModelError(string.Empty, "The selected pet is not available for adoption.");
            //     return View(adoption);
            // }
            if (!pet.Availability)
            {
                throw new PetAdoptionException("This pet has already been adopted");
            }


            if (!ModelState.IsValid)
            {
                //Console.WriteLine("!ModelState");
                return View(adoption);
            }

            pet.Availability = false;
            _dbContext.Pets.Update(pet);
            _dbContext.PetAdoptions.Add(adoption);
            _dbContext.SaveChanges();

            return RedirectToAction("Details", new { adopterId = adoption.PetAdopterID });
        }

        public IActionResult Details(int adopterId)
        {
            var adoption = _dbContext.PetAdoptions
                .Include(a => a.Pet)
                .FirstOrDefault(a => a.PetAdopterID == adopterId);

            if (adoption == null)
            {
                return NotFound();
            }

            return View(adoption);
        }
    }
}
