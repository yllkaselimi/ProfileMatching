using Microsoft.AspNetCore.Mvc;
using ASP.NETCoreIdentityCustom.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using ProfileMatching.Models;
using Microsoft.AspNetCore.Identity;

namespace ProfileMatching.Controllers
{
    public class JobSuggestion : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public JobSuggestion(DataContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var detail = _context.FreelancerDetails.First(x => x.UserId == user.Id);

            var cat = detail.CategoryId;

            var matchingJobPosts = _context.JobPosts.Where(x => x.CategoryId == cat).Include(j => j.Category).ToList();
            /*meqe e kena jobpost t'lidht me categoryId, qikjo .include po i shtohet qe me i include
              dmth data edhe prej tabeles category, n'rast te na me mujt me thirr mandej n'front emrin e kategorise
              me qat categoryId
            */

            if (matchingJobPosts == null)
            {
                return NotFound();
            }

            return View(matchingJobPosts);
        }
    }
    }






