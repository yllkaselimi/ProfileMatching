using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NETCoreIdentityCustom.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProfileMatching.Models;

namespace ProfileMatching.Controllers
{
    public class FreelancerDetailsController : Controller
    {
        private readonly DataContext _context;

        public FreelancerDetailsController(DataContext context)
        {
            _context = context;
        }

        // GET: FreelancerDetails
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.FreelancerDetails.Include(f => f.ApplicationUser).Include(f => f.Category).Include(f => f.City);
            return View(await dataContext.ToListAsync());
        }

        // GET: FreelancerDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FreelancerDetails == null)
            {
                return NotFound();
            }

            var freelancerDetails = await _context.FreelancerDetails
                .Include(f => f.ApplicationUser)
                .Include(f => f.Category)
                .Include(f => f.City)
                .FirstOrDefaultAsync(m => m.FreelancerDetailsId == id);
            if (freelancerDetails == null)
            {
                return NotFound();
            }

            return View(freelancerDetails);
        }

        // GET: FreelancerDetails/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "FirstName");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName");
            return View();
        }

        // POST: FreelancerDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FreelancerDetailsId,CategoryId,CityId,HourlyRate,Languages,Overview,UserId")] FreelancerDetails freelancerDetails)
        {
          //  if (ModelState.IsValid)
           // {
                _context.Add(freelancerDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
          //  }
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", freelancerDetails.UserId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", freelancerDetails.CategoryId);
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId", freelancerDetails.CityId);
            return View(freelancerDetails);
        }

        // GET: FreelancerDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FreelancerDetails == null)
            {
                return NotFound();
            }

            var freelancerDetails = await _context.FreelancerDetails.FindAsync(id);
            if (freelancerDetails == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "FirstName", freelancerDetails.UserId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", freelancerDetails.CategoryId);
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", freelancerDetails.CityId);
            return View(freelancerDetails);
        }

        // POST: FreelancerDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FreelancerDetailsId,CategoryId,CityId,HourlyRate,Languages,Overview,UserId")] FreelancerDetails freelancerDetails)
        {
            if (id != freelancerDetails.FreelancerDetailsId)
            {
                return NotFound();
            }

           // if (ModelState.IsValid)
            //{
                try
                {
                    _context.Update(freelancerDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FreelancerDetailsExists(freelancerDetails.FreelancerDetailsId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
               }
                return RedirectToAction(nameof(Index));
          //  }
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", freelancerDetails.UserId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", freelancerDetails.CategoryId);
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId", freelancerDetails.CityId);
            return View(freelancerDetails);
        }

        // GET: FreelancerDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FreelancerDetails == null)
            {
                return NotFound();
            }

            var freelancerDetails = await _context.FreelancerDetails
                .Include(f => f.ApplicationUser)
                .Include(f => f.Category)
                .Include(f => f.City)
                .FirstOrDefaultAsync(m => m.FreelancerDetailsId == id);
            if (freelancerDetails == null)
            {
                return NotFound();
            }

            return View(freelancerDetails);
        }

        // POST: FreelancerDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FreelancerDetails == null)
            {
                return Problem("Entity set 'DataContext.FreelancerDetails'  is null.");
            }
            var freelancerDetails = await _context.FreelancerDetails.FindAsync(id);
            if (freelancerDetails != null)
            {
                _context.FreelancerDetails.Remove(freelancerDetails);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FreelancerDetailsExists(int id)
        {
          return _context.FreelancerDetails.Any(e => e.FreelancerDetailsId == id);
        }
    }
}
