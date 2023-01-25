using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileMatching.Models;

namespace ProfileMatching.Controllers
{
    public class SavedJobs : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SavedJobs(DataContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        public async Task<IActionResult> Index()
        {
            return View(await _context.SavedJobs.ToListAsync());
        }



        [HttpPost]
        public async Task<IActionResult> SaveJob(int id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var itemToAdd = new SavedJob
            {
                UserId = userId,
                JobPostId = id
            };

            if ((_context.SavedJobs.Any(p => p.JobPostId == id && p.UserId == userId)))
            {
                TempData["Message2"] = "You already saved this job";
                return Redirect(HttpContext.Request.Headers["Referer"]);
            }

            TempData["SuccessfulSave"] = "Successful Save!";
            _context.SavedJobs.Add(itemToAdd);
            await _context.SaveChangesAsync();
            return Redirect(HttpContext.Request.Headers["Referer"]);
        }

        //kjo e merr id te logged in user edhe jobpostId, aplikohet veq kur freelancer e bon vet unsave 
        public async Task<IActionResult> Unsave(int? id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var job = _context.SavedJobs.Where(x => x.JobPostId == id && x.UserId == userId).First();

            if (job != null)
            {
                _context.SavedJobs.Remove(job);
            }

            await _context.SaveChangesAsync();
            return Redirect(HttpContext.Request.Headers["Referer"]);
        }


        //kto e perdor veq admini, e merr id te savedjob
        public async Task<IActionResult> DeleteSave(int? id)
        {
            if (_context.SavedJobs == null)
            {
                return Problem("Entity set 'DataContext.Savedjobs'  is null.");
            }
            var job = await _context.SavedJobs.FindAsync(id);

            if (job != null)
            {
                _context.SavedJobs.Remove(job);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> MySavedJobs()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            return View(await _context.SavedJobs.Where(m => m.UserId == userId).ToListAsync());
        }



    }
}
