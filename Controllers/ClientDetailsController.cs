using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProfileMatching.Models;

namespace ProfileMatching.Controllers
{
    public class ClientDetailsController : Controller
    {
        private readonly DataContext _context;

        public ClientDetailsController(DataContext context)
        {
            _context = context;
        }

        // GET: ClientDetails
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.ClientDetails.Include(c => c.City);
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
                .Include(c => c.City)
                .FirstOrDefaultAsync(m => m.ClientDetailsId == id);
            if (clientDetail == null)
            {
                return NotFound();
            }

            return View(clientDetail);
        }

        // GET: ClientDetails/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId");
            return View();
        }

        // POST: ClientDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientDetailsId,Position,CompanyName,Description,CityId")] ClientDetail clientDetail)
        {
          //  if (ModelState.IsValid)
           // {
                _context.Add(clientDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
           // }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId", clientDetail.CityId);
            return View(clientDetail);
        }

        // GET: ClientDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ClientDetails == null)
            {
                return NotFound();
            }

            var clientDetail = await _context.ClientDetails.FindAsync(id);
            if (clientDetail == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId", clientDetail.CityId);
            return View(clientDetail);
        }

        // POST: ClientDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClientDetailsId,Position,CompanyName,Description,CityId")] ClientDetail clientDetail)
        {
            if (id != clientDetail.ClientDetailsId)
            {
                return NotFound();
            }

           // if (ModelState.IsValid)
          //  {
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
           //     }
             //   return RedirectToAction(nameof(Index));
            }
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
