using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NETCoreIdentityCustom.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProfileMatching.Models;

namespace ProfileMatching.Controllers
{
    public class FreelancerDetailsController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public FreelancerDetailsController(DataContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
        public async Task<IActionResult> Create()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var freeLancerInfo = _context.FreelancerDetails.Include(f => f.ApplicationUser)
           .Include(f => f.City)
           .Include(f => f.Category)
           .FirstOrDefault(f => f.UserId == userId);

            if (freeLancerInfo != null)
            {
                return RedirectToAction("Index", "FreelancerProfile");
            }

            var user = _context.Users.Where(x => x.Id == userId).First();
            var RolesForUser = await _userManager.GetRolesAsync(user);
            var roli = RolesForUser[0];
            ViewData["Role"] = roli;

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

            _context.Add(freelancerDetails);
            await _context.SaveChangesAsync();

            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _context.Users.Where(x => x.Id == userId).First();
            var RolesForUser = await _userManager.GetRolesAsync(user);
            var roli = RolesForUser[0];

            if (roli == "Freelancer")
            {
                return RedirectToAction("Index", "FreelancerProfile");
            } 

            return RedirectToAction(nameof(Index));
          
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", freelancerDetails.UserId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", freelancerDetails.CategoryId);
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId", freelancerDetails.CityId);
            return Redirect(HttpContext.Request.Headers["Referer"]);
        }

        // GET: FreelancerDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _context.Users.Where(x => x.Id == userId).First();
            var RolesForUser = await _userManager.GetRolesAsync(user);
            var roli = RolesForUser[0];
            ViewData["Role"] = roli;

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

            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _context.Users.Where(x => x.Id == userId).First();
            var RolesForUser = await _userManager.GetRolesAsync(user);
            var roli = RolesForUser[0];

            if (roli == "Freelancer")
            {
                return RedirectToAction("Index", "FreelancerProfile");
            }
            return RedirectToAction(nameof(Index));

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

            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _context.Users.Where(x => x.Id == userId).First();
            var RolesForUser = await _userManager.GetRolesAsync(user);
            var roli = RolesForUser[0];

            if (roli == "Freelancer")
            {
                return RedirectToAction("Index", "FreelancerProfile");
            }

            return RedirectToAction(nameof(Index));
        }

        private bool FreelancerDetailsExists(int id)
        {
          return _context.FreelancerDetails.Any(e => e.FreelancerDetailsId == id);
        }
    }
}
