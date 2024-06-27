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
            var pets = _dbContext.Pets.ToList();
            return View(pets);
        }

        public IActionResult Delete(int petId)
        {
            var pet = _dbContext.Pets.FirstOrDefault(p => p.PetID == petId);
            Console.WriteLine("Delete");
            if (pet == null)
            {
                return NotFound();
            }
            Console.WriteLine("Viewing Pet");
            return View(pet);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int petId)
        {
            Console.WriteLine("Entered");
            var pet = _dbContext.Pets.FirstOrDefault(p => p.PetID == petId);
            if (pet == null)
            {
                return NotFound();
            }
            Console.WriteLine("Removed");
            _dbContext.Pets.Remove(pet);
            Console.WriteLine("Saved Changes");
            _dbContext.SaveChanges();
            Console.WriteLine("Returning to Index");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Search(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return RedirectToAction(nameof(Index));
            }

            var lowerName = name.ToLower();

            var pets = _dbContext.Pets
                .Where(p => EF.Functions.Like(p.Name.ToLower(), "%" + lowerName + "%"))
                .ToList();

            var exactMatch = pets.FirstOrDefault(p => p.Name.ToLower() == lowerName);

            if (exactMatch == null)
            {
                TempData["Message"] = $"No pet found matching '{name}'.";
                return RedirectToAction(nameof(Index));
            }

            return View(nameof(Index), pets);
        }
    }
}
