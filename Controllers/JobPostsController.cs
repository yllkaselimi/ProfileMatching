using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProfileMatching.Models;

namespace ProfileMatching.Controllers
{
    public class JobPostsController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public JobPostsController(DataContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: JobPosts
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.JobPosts.Include(j => j.Category);

            return View(await dataContext.ToListAsync());
            //return View(await dataContext.OrderByDescending(j => j.JobPostBudget).ToListAsync());
        }

        // GET: JobPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.JobPosts == null)
            {
                return NotFound();
            }

            var jobPost = await _context.JobPosts
                .Include(j => j.Category)
                .FirstOrDefaultAsync(m => m.JobPostId == id);
            if (jobPost == null)
            {
                return NotFound();
            }

            return View(jobPost);
        }

        // GET: JobPosts/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: JobPosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobPostId,JobPostName,JobPostBudget,JobLength,JobPostDescription,JobApplicationDeadline,CategoryId")] JobPost jobPost)
        {
            //if (ModelState.IsValid)
            //{
                _context.Add(jobPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", jobPost.CategoryId);
            return View(jobPost);
        }

        // GET: JobPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.JobPosts == null)
            {
                return NotFound();
            }

            var jobPost = await _context.JobPosts.FindAsync(id);
            if (jobPost == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", jobPost.CategoryId);
            return View(jobPost);
        }

        // POST: JobPosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JobPostId,JobPostName,JobPostBudget,JobLength,JobPostDescription,JobApplicationDeadline,CategoryId")] JobPost jobPost)
        {
            if (id != jobPost.JobPostId)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    _context.Update(jobPost);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
            }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobPostExists(jobPost.JobPostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                //}
                //return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", jobPost.CategoryId);
            return View(jobPost);
        }

        // GET: JobPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.JobPosts == null)
            {
                return NotFound();
            }

            var jobPost = await _context.JobPosts
                .Include(j => j.Category)
                .FirstOrDefaultAsync(m => m.JobPostId == id);
            if (jobPost == null)
            {
                return NotFound();
            }

            return View(jobPost);
        }

        // POST: JobPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.JobPosts == null)
            {
                return Problem("Entity set 'DataContext.JobPosts'  is null.");
            }
            var jobPost = await _context.JobPosts.FindAsync(id);
            if (jobPost != null)
            {
                _context.JobPosts.Remove(jobPost);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool JobPostExists(int id)
        {
          return _context.JobPosts.Any(e => e.JobPostId == id);
        }
    }
}
