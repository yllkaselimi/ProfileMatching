using Microsoft.AspNetCore.Mvc;
using ASP.NETCoreIdentityCustom.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using ProfileMatching.Models;
using Microsoft.AspNetCore.Identity;

namespace ProfileMatching.Controllers
{
    public class JobSuggestions : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public JobSuggestions(DataContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
 
            //i marrim freelancerDetails te freelancer qe osht logged in
            var detail = _context.FreelancerDetails.FirstOrDefault(x => x.UserId == user.Id);

            //niher check qaj user a i ka qit freelancer detials nprofil (se na vyn kategoria), nqs jo, e qon te faqja me create freelancer details
            if (detail == null)
            {
                return RedirectToAction("Create", "FreelancerDetails");
            }

            var cat = detail.CategoryId; //prej freelancers details t'qatij useri, po e marrim veq kategorine

            var userId = _userManager.GetUserId(HttpContext.User);
            var RolesForUser = await _userManager.GetRolesAsync(user);
            var roli = RolesForUser[0];
            ViewData["Role"] = roli;

            var matchingJobPosts = _context.JobPosts.Where(x => x.CategoryId == cat).Include(j => j.Category).ToList();
            /*meqe e kena jobpost t'lidht me categoryId, qikjo .include po i shtohet qe me i include
              dmth data edhe prej tabeles category, n'rast te na me mujt me thirr mandej n'front emrin e kategorise
              me qat categoryId
            */


            //i bartim id te puneve qe useri has already applied for me ni viewdata
            var appliedJobs = _context.ApplicantsPerJobs.Where(x => x.UserId == userId).Select(x => x.JobPostId).ToList();
            ViewData["AppliedJobs"] = appliedJobs;

            //i bartim id te puneve qe useri has already saved me ni viewdata
            var savedJobs = _context.SavedJobs.Where(x => x.UserId == userId).Select(x => x.JobPostId).ToList();
            ViewData["SavedJobs"] = savedJobs;

            if (matchingJobPosts == null)
            {
                return NotFound();
            }

            return View(matchingJobPosts);
        }
    }
    }






