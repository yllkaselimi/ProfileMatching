using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileMatching.Models;

namespace ProfileMatching.Controllers
{
    public class FreeLancerHomePage : Controller
    {

        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public FreeLancerHomePage(DataContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _context.Users.Where(x => x.Id == userId).First();

            //ViewData["Categories"] = _context.Categories.ToList();
            var dataContext = _context.JobPosts.Include(j => j.Category).Include(j => j.ApplicationUser);

            var userCategory = _context.FreelancerDetails.FirstOrDefault(x => x.UserId == userId).CategoryId;
            var matchingJobPosts = _context.JobPosts.Where(x => x.CategoryId == userCategory).Include(j => j.Category).Include(j => j.ApplicationUser).OrderByDescending(j => j.JobPostId).Take(3);
            ViewData["MatchingJobPosts"] = matchingJobPosts;

            //i bartim id te puneve qe useri has already applied for me ni viewdata
            var appliedJobs = _context.ApplicantsPerJobs.Where(x => x.UserId == userId).Select(x => x.JobPostId).ToList();
            ViewData["AppliedJobs"] = appliedJobs;

            //i bartim id te puneve qe useri has already saves for me ni viewdata
            var savedJobs = _context.SavedJobs.Where(x => x.UserId == userId).Select(x => x.JobPostId).ToList();
            ViewData["SavedJobs"] = savedJobs;

            return View(dataContext.OrderByDescending(j => j.JobPostId).Take(3));

        }
    }
}
