using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnetapp.Models;
using dotnetapp.Data;

namespace dotnetapp.Controllers
{
    public class PartyHallController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public PartyHallController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var partyHalls = _dbContext.PartyHalls.ToList();
            return View(partyHalls);
        }

        public IActionResult Delete(int partyHallId)
        {
            var partyHall = _dbContext.PartyHalls.FirstOrDefault(p => p.PartyHallID == partyHallId);
            if (partyHall == null)
            {
                return NotFound();
            }

            return View(partyHall);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int partyHallId)
        {
            var partyHall = _dbContext.PartyHalls.FirstOrDefault(p => p.PartyHallID == partyHallId);
            if (partyHall == null)
            {
                return NotFound();
            }

            _dbContext.PartyHalls.Remove(partyHall);
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

    var partyHalls = _dbContext.PartyHalls                                                              
        .Where(p => EF.Functions.Like(p.Name.ToLower(), "%" + lowerName + "%")) // Using EF.Functions.Like for wildcard search
        .ToList();

    // Check if any party hall names exactly match the search string
    var exactMatch = partyHalls.FirstOrDefault(p => p.Name.ToLower() == lowerName);

    if (exactMatch == null)
    {
        // Handle case where no exact match is found
        TempData["Message"] = $"No party hall found matching '{name}'.";
        return RedirectToAction(nameof(Index));
    }

    return View(nameof(Index), partyHalls);
}

    }
}