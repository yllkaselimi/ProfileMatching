using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileMatching.Models;

namespace ProfileMatching.Controllers
{
    public class ApplicantsPerJobs : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ApplicantsPerJobs(DataContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        public async Task<IActionResult> Index()
        {
            return View(await _context.ApplicantsPerJobs.ToListAsync());
        }



        [HttpPost]
        public async Task<IActionResult> ApplyForJob(int id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var itemToAdd = new ApplicantsPerJob
            {
                UserId = userId,
                JobPostId = id
            };


            if((_context.ApplicantsPerJobs.Any(p => p.JobPostId == id && p.UserId == userId)))
            {
                TempData["Message"] = "You already applied for this job";
                return Redirect(HttpContext.Request.Headers["Referer"]);
            }

            TempData["SuccessfulApplication"] = "Successful Application!";
            _context.ApplicantsPerJobs.Add(itemToAdd);
            await _context.SaveChangesAsync();
            return Redirect(HttpContext.Request.Headers["Referer"]); 
        }

        public async Task<IActionResult> UnApply(int? id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var job = _context.ApplicantsPerJobs.Where(x => x.JobPostId == id && x.UserId == userId).First();

            if (job != null)
            {
               _context.ApplicantsPerJobs.Remove(job);
            }

            await _context.SaveChangesAsync();
            return Redirect(HttpContext.Request.Headers["Referer"]);
        }



        public async Task<IActionResult> ShowJobApplicants(int? id)
        {
            if (id == null || _context.ApplicantsPerJobs == null)
            {
                return NotFound();
            }

            var job =_context.JobPosts.Where(x => x.JobPostId == id).First();
            ViewData["JobName"] = job.JobPostName;

            var applicants = await _context.ApplicantsPerJobs.Where(m => m.JobPostId == id).ToListAsync();
            if (applicants == null)
            {
                return NotFound();
            }

            return View(applicants);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.ApplicantsPerJobs == null)
            {
                return Problem("Entity set 'DataContext.ApplicantsPerJob'  is null.");
            }
            var application = await _context.ApplicantsPerJobs.FindAsync(id);

            var jobId = application.JobPostId;

            if (application != null)
            {
                _context.ApplicantsPerJobs.Remove(application);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("ShowJobApplicants", new { id = jobId });
        }


        public async Task<IActionResult> MyJobApplications()
        {
            //kodi qe po na qon rolin e loggedin user te viewbag n view
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _context.Users.Where(x => x.Id == userId).First();
            var RolesForUser = await _userManager.GetRolesAsync(user);
            var roli = RolesForUser[0];
            ViewData["Role"] = roli;

            //i bartim id te puneve qe useri has already applied for me ni viewdata
            var appliedJobs = _context.ApplicantsPerJobs.Where(x => x.UserId == userId).Select(x => x.JobPostId).ToList();
            ViewData["AppliedJobs"] = appliedJobs;


            return View(await _context.ApplicantsPerJobs.Where(m => m.UserId == userId).ToListAsync());
        }



    }
}
