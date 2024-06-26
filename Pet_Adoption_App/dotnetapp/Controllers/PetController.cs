using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnetapp.Models;
using dotnetapp.Data;

namespace dotnetapp.Controllers
{
    public class PetController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public PetController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var pets = _dbContext.Pet.ToList();
            return View(pets);
        }

        public IActionResult Delete(int petId)
        {
            var pet = _dbContext.Pet.FirstOrDefault(p => p.PartyHallID == petId);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int petId)
        {
            var pet = _dbContext.Pet.FirstOrDefault(p => p.PartyHallID == petId);
            if (pet == null)
            {
                return NotFound();
            }

            _dbContext.Pet.Remove(pet);
            _dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Search(string name)
{
    if (string.IsNullOrEmpty(name))
    {
        // Handle case where name is null or empty
        return RedirectToAction(nameof(Index));
    }

    // Convert search string to lower case for case-insensitive comparison
    var lowerName = name.ToLower();

    var pets = _dbContext.Pet                                                              
        .Where(p => EF.Functions.Like(p.Name.ToLower(), "%" + lowerName + "%")) // Using EF.Functions.Like for wildcard search
        .ToList();

    // Check if any party hall names exactly match the search string
    var exactMatch = pets.FirstOrDefault(p => p.Name.ToLower() == lowerName);

    if (exactMatch == null)
    {
        // Handle case where no exact match is found
        TempData["Message"] = $"No party hall found matching '{name}'.";
        return RedirectToAction(nameof(Index));
    }

    return View(nameof(Index), pets);
}

    }
}