using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Car_net.Models;

namespace Car_net.Controllers
{
    public class InsureeController : Controller
    {
        private readonly InsuranceContext _context;

        public InsureeController(InsuranceContext context)
        {
            _context = context;
        }

        // GET: Insuree
        public async Task<IActionResult> Index()
        {
            return View(await _context.Insurees.ToListAsync());
        }

        // GET: Insuree/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuree = await _context.Insurees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuree == null)
            {
                return NotFound();
            }

            return View(insuree);
        }

        // GET: Insuree/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,SpeedingTickets,HasDUI,IsFullCoverage")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                // Calculate the quote
                CalculateQuote(insuree);

                _context.Add(insuree);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(insuree);
        }

        // GET: Insuree/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuree = await _context.Insurees.FindAsync(id);
            if (insuree == null)
            {
                return NotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,SpeedingTickets,HasDUI,IsFullCoverage,Quote")] Insuree insuree)
        {
            if (id != insuree.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Recalculate the quote
                    CalculateQuote(insuree);

                    _context.Update(insuree);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsureeExists(insuree.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(insuree);
        }

        // GET: Insuree/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuree = await _context.Insurees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuree == null)
            {
                return NotFound();
            }

            return View(insuree);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var insuree = await _context.Insurees.FindAsync(id);
            if (insuree != null)
            {
                _context.Insurees.Remove(insuree);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsureeExists(int id)
        {
            return _context.Insurees.Any(e => e.Id == id);
        }

        // GET: Insuree/Admin
        public async Task<IActionResult> Admin()
        {
            return View(await _context.Insurees.ToListAsync());
        }

        private void CalculateQuote(Insuree insuree)
        {
            // Start with a base of $50 / month
            decimal baseQuote = 50;

            // Calculate age
            int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
            if (DateTime.Now.DayOfYear < insuree.DateOfBirth.DayOfYear)
            {
                age--;
            }

            // Age-based adjustments
            if (age <= 18)
            {
                baseQuote += 100;
            }
            else if (age >= 19 && age <= 25)
            {
                baseQuote += 50;
            }
            else // age >= 26
            {
                baseQuote += 25;
            }

            // Car year adjustments
            if (insuree.CarYear < 2000)
            {
                baseQuote += 25;
            }
            else if (insuree.CarYear > 2015)
            {
                baseQuote += 25;
            }

            // Car make and model adjustments
            if (insuree.CarMake.ToLower() == "porsche")
            {
                baseQuote += 25;

                if (insuree.CarModel.ToLower() == "911 carrera")
                {
                    baseQuote += 25;
                }
            }

            // Speeding tickets
            baseQuote += 10 * insuree.SpeedingTickets;

            // DUI adjustment (25% increase)
            if (insuree.HasDUI)
            {
                baseQuote *= 1.25m;
            }

            // Full coverage adjustment (50% increase)
            if (insuree.IsFullCoverage)
            {
                baseQuote *= 1.5m;
            }

            // Set the calculated quote
            insuree.Quote = baseQuote;
        }
    }
} 