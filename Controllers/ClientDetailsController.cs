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
    public class ClientDetailsController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ClientDetailsController(DataContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ClientDetails
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.ClientDetails.Include(c => c.ApplicationUser).Include(c => c.City);
            return View(await dataContext.ToListAsync());
        }

        // GET: ClientDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ClientDetails == null)
            {
                return NotFound();
            }

            var clientDetail = await _context.ClientDetails
                .Include(c => c.ApplicationUser)
                .Include(c => c.City)
                .FirstOrDefaultAsync(m => m.ClientDetailsId == id);
            if (clientDetail == null)
            {
                return NotFound();
            }

            return View(clientDetail);
        }

        // GET: ClientDetails/Create
        public async Task<IActionResult> Create()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _context.Users.Where(x => x.Id == userId).First();
            var RolesForUser = await _userManager.GetRolesAsync(user);
            var roli = RolesForUser[0];
            ViewData["Role"] = roli;

            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId");
            return View();
        }

        // POST: ClientDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientDetailsId,UserId,Position,CompanyName,Description,CityId")] ClientDetail clientDetail)
        {
            //if (ModelState.IsValid)
            //{
                _context.Add(clientDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", clientDetail.UserId);
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId", clientDetail.CityId);
            return View(clientDetail);
        }

        // GET: ClientDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _context.Users.Where(x => x.Id == userId).First();
            var RolesForUser = await _userManager.GetRolesAsync(user);
            var roli = RolesForUser[0];
            ViewData["Role"] = roli;

            if (id == null || _context.ClientDetails == null)
            {
                return NotFound();
            }

            var clientDetail = await _context.ClientDetails.FindAsync(id);
            if (clientDetail == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", clientDetail.UserId);
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId", clientDetail.CityId);
            return View(clientDetail);
        }

        // POST: ClientDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClientDetailsId,UserId,Position,CompanyName,Description,CityId")] ClientDetail clientDetail)
        {
            if (id != clientDetail.ClientDetailsId)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    _context.Update(clientDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientDetailExists(clientDetail.ClientDetailsId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
           // }
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", clientDetail.UserId);
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId", clientDetail.CityId);
            return View(clientDetail);
        }

        // GET: ClientDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ClientDetails == null)
            {
                return NotFound();
            }

            var clientDetail = await _context.ClientDetails
                .Include(c => c.ApplicationUser)
                .Include(c => c.City)
                .FirstOrDefaultAsync(m => m.ClientDetailsId == id);
            if (clientDetail == null)
            {
                return NotFound();
            }

            return View(clientDetail);
        }

        // POST: ClientDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ClientDetails == null)
            {
                return Problem("Entity set 'DataContext.ClientDetails'  is null.");
            }
            var clientDetail = await _context.ClientDetails.FindAsync(id);
            if (clientDetail != null)
            {
                _context.ClientDetails.Remove(clientDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientDetailExists(int id)
        {
          return _context.ClientDetails.Any(e => e.ClientDetailsId == id);
        }
    }
}
