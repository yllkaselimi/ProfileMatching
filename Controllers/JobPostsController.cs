using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            ViewData["Categories"] = _context.Categories.ToList();
            var dataContext = _context.JobPosts.Include(j => j.Category);

            //kodi qe po na qon rolin e loggedin user te viewbag n view
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _context.Users.Where(x => x.Id == userId).First();
            var RolesForUser = await _userManager.GetRolesAsync(user);
            var roli = RolesForUser[0];
            ViewData["Role"] = roli;

            //i bartim id te puneve qe useri has already applied for me ni viewdata
            var appliedJobs = _context.ApplicantsPerJobs.Where(x => x.UserId == userId).Select(x => x.JobPostId).ToList();
            ViewData["AppliedJobs"] = appliedJobs;

            //i bartim id te puneve qe useri has already saves for me ni viewdata
            var savedJobs = _context.SavedJobs.Where(x => x.UserId == userId).Select(x => x.JobPostId).ToList();
            ViewData["SavedJobs"] = savedJobs;

            return View(await dataContext.ToListAsync());
            //return View(await dataContext.OrderByDescending(j => j.JobPostBudget).ToListAsync());
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
        public async Task<IActionResult> Create([Bind("JobPostId,JobPostName,JobPostBudget,JobLength,JobPostDescription,JobApplicationDeadline,CategoryId,UserId")] JobPost jobPost)
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
        public async Task<IActionResult> Edit(int id, [Bind("JobPostId,JobPostName,JobPostBudget,JobLength,JobPostDescription,JobApplicationDeadline,CategoryId, UserId")] JobPost jobPost)
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


        public async Task<IActionResult> SearchJobPost(string teksti)
        {
            //kodi per me rujt rolin e logged in user me ni viewdata
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _context.Users.Where(x => x.Id == userId).First();
            var RolesForUser = await _userManager.GetRolesAsync(user);
            var roli = RolesForUser[0];
            ViewData["Role"] = roli;

            ViewData["Categories"] = _context.Categories.ToList();

            ViewData["SearchInput"] = teksti;

            //i bartim id te puneve qe useri has already applied for me ni viewdata
            var appliedJobs = _context.ApplicantsPerJobs.Where(x => x.UserId == userId).Select(x => x.JobPostId).ToList();
            ViewData["AppliedJobs"] = appliedJobs;

            //i bartim id te puneve qe useri has already saves for me ni viewdata
            var savedJobs = _context.SavedJobs.Where(x => x.UserId == userId).Select(x => x.JobPostId).ToList();
            ViewData["SavedJobs"] = savedJobs;

            var allJobs = _context.JobPosts.Include(j => j.Category);
            var result = allJobs.Where(f => f.Category.CategoryName.ToLower().Contains(teksti.ToLower())
                || f.JobPostName.ToLower().Contains(teksti.ToLower()));

            return View(await result.ToListAsync());
        }

        public async Task<IActionResult> FilterJobPost(int? id)
        {
            //kodi per me rujt rolin e logged in user me ni viewdata
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _context.Users.Where(x => x.Id == userId).First();
            var RolesForUser = await _userManager.GetRolesAsync(user);
            var roli = RolesForUser[0];
            ViewData["Role"] = roli;

            ViewData["Categories"] = _context.Categories.ToList();

            var category = _context.Categories.Where(x => x.CategoryId == id).First();
            ViewData["FilteredCategoryName"] = category.CategoryName;

            //i bartim id te puneve qe useri has already applied for me ni viewdata
            var appliedJobs = _context.ApplicantsPerJobs.Where(x => x.UserId == userId).Select(x => x.JobPostId).ToList();
            ViewData["AppliedJobs"] = appliedJobs;

            //i bartim id te puneve qe useri has already saves for me ni viewdata
            var savedJobs = _context.SavedJobs.Where(x => x.UserId == userId).Select(x => x.JobPostId).ToList();
            ViewData["SavedJobs"] = savedJobs;

            var jobpostvar = _context.JobPosts
                .Include(f => f.Category)
                .Where(f => f.Category.CategoryId == id);

            if (jobpostvar == null)
            {
                return NotFound();
            }
            return View(await jobpostvar.ToListAsync());
        }

    }

}
