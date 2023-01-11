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
    public class FreelancerExperiencesController : Controller
    {
        private readonly DataContext _context;

        public FreelancerExperiencesController(DataContext context)
        {
            _context = context;
        }

        // GET: FreelancerExperiences
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.FreelancerExperiences.Include(f => f.ApplicationUser).Include(f => f.EmploymentType);
            return View(await dataContext.ToListAsync());
        }

        // GET: FreelancerExperiences/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FreelancerExperiences == null)
            {
                return NotFound();
            }

            var freelancerExperience = await _context.FreelancerExperiences
                .Include(f => f.ApplicationUser)
                .Include(f => f.EmploymentType)
                .FirstOrDefaultAsync(m => m.FreelancerExperienceID == id);
            if (freelancerExperience == null)
            {
                return NotFound();
            }

            return View(freelancerExperience);
        }

        // GET: FreelancerExperiences/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            ViewData["EmploymentTypeId"] = new SelectList(_context.EmploymentTypes, "EmploymentTypeId", "EmploymentTypeId");
            return View();
        }

        // POST: FreelancerExperiences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FreelancerExperienceID,UserId,EmploymentTypeId,CompanyName,StartDate,EndDate")] FreelancerExperience freelancerExperience)
        {
            if (ModelState.IsValid)
            {
                _context.Add(freelancerExperience);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", freelancerExperience.UserId);
            ViewData["EmploymentTypeId"] = new SelectList(_context.EmploymentTypes, "EmploymentTypeId", "EmploymentTypeId", freelancerExperience.EmploymentTypeId);
            return View(freelancerExperience);
        }

        // GET: FreelancerExperiences/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FreelancerExperiences == null)
            {
                return NotFound();
            }

            var freelancerExperience = await _context.FreelancerExperiences.FindAsync(id);
            if (freelancerExperience == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", freelancerExperience.UserId);
            ViewData["EmploymentTypeId"] = new SelectList(_context.EmploymentTypes, "EmploymentTypeId", "EmploymentTypeId", freelancerExperience.EmploymentTypeId);
            return View(freelancerExperience);
        }

        // POST: FreelancerExperiences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FreelancerExperienceID,UserId,EmploymentTypeId,CompanyName,StartDate,EndDate")] FreelancerExperience freelancerExperience)
        {
            if (id != freelancerExperience.FreelancerExperienceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(freelancerExperience);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FreelancerExperienceExists(freelancerExperience.FreelancerExperienceID))
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
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", freelancerExperience.UserId);
            ViewData["EmploymentTypeId"] = new SelectList(_context.EmploymentTypes, "EmploymentTypeId", "EmploymentTypeId", freelancerExperience.EmploymentTypeId);
            return View(freelancerExperience);
        }

        // GET: FreelancerExperiences/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FreelancerExperiences == null)
            {
                return NotFound();
            }

            var freelancerExperience = await _context.FreelancerExperiences
                .Include(f => f.ApplicationUser)
                .Include(f => f.EmploymentType)
                .FirstOrDefaultAsync(m => m.FreelancerExperienceID == id);
            if (freelancerExperience == null)
            {
                return NotFound();
            }

            return View(freelancerExperience);
        }

        // POST: FreelancerExperiences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FreelancerExperiences == null)
            {
                return Problem("Entity set 'DataContext.FreelancerExperiences'  is null.");
            }
            var freelancerExperience = await _context.FreelancerExperiences.FindAsync(id);
            if (freelancerExperience != null)
            {
                _context.FreelancerExperiences.Remove(freelancerExperience);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FreelancerExperienceExists(int id)
        {
          return _context.FreelancerExperiences.Any(e => e.FreelancerExperienceID == id);
        }
    }
}
